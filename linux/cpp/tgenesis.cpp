#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <fcntl.h>
#include <errno.h>
#include <termios.h>
#include <unistd.h>
#include <malloc.h>
#include <stdint.h>

// Poner en 1 la siguiente variable para ver detalles de los byte sintercambiados
#define DBGOUT 1

// Define la macro/función para mostrar mensajes de DEBUG
#if DBGOUT
char msg[5000];

static void DBG(const char* smsg) {
	FILE* f = fopen("genesis.log", "a");
	if (f != NULL) {
		fprintf(f, "%s", smsg);
		fclose(f);
	}
}
#else
#define DBG(x)
#endif

// Los distintos caracteres de control usados por el protocolo
#define STX 0x02
#define ETX 0x03
#define ESC 0x1B
#define FLD 0x1C
#define NAK 0x15
#define DC2 0x12
#define DC4 0x14

// Variables globales
int portHandle;				// El OS handle al port serial
int sequence;				// El número de secuencia del último paquete intercambiado
char *portName;				// El nombre del port serial a usar (pasado como argumento)
char *sysMsg;				// Mensaje producido por el OS
bool nakReceived;			// Recibio un NAK, por lo que debe reenviar datos
int repeCount = 1;			// La cantidad de veces que se repetirá el test.
int paperCount = 0;			// Para que el mensaje de "falta papel" no salga tan rápido

// Contenido ya parseado de los distintos campos recibidos del controlador fiscal.
uint32_t 
	rxPrinterStatus, 		// Estado del mecanismo impresor
	rxFiscalStatus, 		// Estado fiscal (por ejemplo, si hay o no ticket abierto)
	rxErrorCode, 			// Código de error producido (0 si no hay error)
	rxSequence;				// Secuencia del paquete que se recibió.
char rxArguments[16000];	// Otros argumentos específicos del comando.

//  
//  Maneja la falta de papel opcional
// 
static void HandleWait(int code)
{
	if (code == DC4) {		// El impresor está procesando
		printf(".");
		return;
	}

	if (code == DC2) {		// Se quedó sin papel
		if (--paperCount > 0) 
			return;
		paperCount = 4;
		printf("Falta papel!\n");
		return;
	}
}

//
//  Función que muestra un mensaje de error.
//
static bool DisplayErrno(const char *msg)
{
	printf("Error %d %s\n%s\n", errno, msg, strerror(errno));
	return false;
}

//
//  Abre el puerto serial a usar para intercambiar mensajes con el genesis
//
bool OpenPort()
{
	termios tty;

	// Abre el port
	portHandle = open(portName, O_RDWR | O_NOCTTY);
	if(portHandle < 0)
		return DisplayErrno("abriendo port serie");

	// Trae los atributos actuales, para actualizarlos
	if(tcgetattr(portHandle, &tty) != 0) {
		DisplayErrno("leyendo configuración port serie");
		close(portHandle);
		return false;
	}

	// Modifica los parametros de comunicación.
	tty.c_cflag &= ~PARENB;			// 8,n,1
	tty.c_cflag &= ~CSTOPB;
	tty.c_cflag &= ~CSIZE;
	tty.c_cflag |= CS8;				
	tty.c_cflag &= ~CRTSCTS;
	tty.c_cflag |= CREAD | CLOCAL;	// Habilita READ, deshabilita ctl lines
	tty.c_cc[VTIME] = 5;			// timeout 0.5 segundo para leer
	tty.c_cc[VMIN] = 1;				// Cantidad min de bytes in antes de volver

	// Velocidad de comunicación.
	cfmakeraw(&tty);
	cfsetispeed(&tty, B115200);
	cfsetospeed(&tty, B115200);

	// Aplica los cambios de parámetros.
	tcflush(portHandle, TCIFLUSH);
	if(tcsetattr(portHandle, TCSANOW, &tty) != 0) {
		DisplayErrno("estableciendo propiedades");
		close(portHandle);
		return false;
	}

	DBG("Puerto abierto correctamente\n");
	return true;
}

//
// Extrae del comando recibido los siguientes 4 caracters que representa un número en
// hexadecimal. Si el número terminaba con el separador de campos '|', lo saltea.
//
static uint32_t ParseHexa(char **str)
{
	uint32_t res = 0;
	while(**str && **str != '|') {
		res = res * 16;
		switch(**str) {
			case '0':
			case '1':
			case '2':
			case '3': 
			case '4': 
			case '5':
			case '6':
			case '7':
			case '8': 
			case '9': res += (**str - '0'); break;
			case 'a': case 'A': res += 10; break;
			case 'b': case 'B': res += 11; break;
			case 'c': case 'C': res += 12; break;
			case 'd': case 'D': res += 13; break;
			case 'e': case 'E': res += 14; break;
			case 'f': case 'F': res += 15; break;
		}
		(*str)++;
	}
	
	if(**str == '|')
		(*str)++;

	return res;
}

//
//  Coloca un byte en el buffer que posteriormente se transmitirá al controlador
//  fiscal. Si el byte a enviar fuera uno que es especial al protocolo (por ejemplo, STX)
//  entonces ANTES le agrega el caracter de escape ESC.
//
static uint8_t* SendByte(uint8_t data, uint8_t* ptr, uint32_t* crc)
{
	if(data == STX || data == ETX || data == ESC) {
		*ptr++ = ESC;
		*crc += ESC;
	}

	*ptr++ = data;
	*crc += data;
	return ptr;
}

//
//  Prepara y envía por el port serial el comando que se indica. 'Arma' el paquete
//  con los caracteres de control y CRC necesarios.
//
static bool SendData(char *cmd) 
{
	uint8_t* tempbf = (uint8_t*) malloc(50000);
	if(tempbf == NULL) {
		printf("Error ubicando memoria\n");
		return false;
	}

	uint8_t* ptr = tempbf;
	uint32_t crc = 0;

	// Comienzo del paquete
	*ptr = STX; crc += *ptr++;

	// Numero secuencial
	if(sequence < 0x80 || sequence > 0xff)
		sequence = 0x80;
	*ptr = (uint8_t)sequence++; crc += *ptr++;

	// El código de comando lo recibimos como cuatro caracteres hexadecimal
	uint32_t n = ParseHexa(&cmd);
	ptr = SendByte((uint8_t)((n / 0x100) & 0xFF), ptr, &crc);
	ptr = SendByte((uint8_t)(n & 0xFF), ptr, &crc);

	// Separador
	ptr = SendByte(FLD, ptr, &crc);

	// La extensión del comando lo recibimos también como cuatro caracteres hexa
	n = ParseHexa(&cmd);
	ptr = SendByte((uint8_t)((n / 0x100) & 0xFF), ptr, &crc);
	ptr = SendByte((uint8_t)(n & 0xFF), ptr, &crc);


	// Envía el resto de los bytes del comando
	if(*cmd)
		ptr = SendByte(FLD, ptr, &crc);

	while(*cmd) {
		ptr = SendByte((*cmd == '|')? FLD: *cmd, ptr, &crc);
		cmd++;
	}

	// Agrega el ETX que marca el final del paquete
	*ptr = ETX; crc += *ptr++;

	// Agrega el CRC de los datos enviados
	char tempcrc[6];
	sprintf(tempcrc, "%04X", crc);
	*ptr++ = tempcrc[0];
	*ptr++ = tempcrc[1];
	*ptr++ = tempcrc[2];
	*ptr++ = tempcrc[3];

	// Si debug está activado, imprimimos los datos que se enviarán al impresor fiscal.
	int sendlen = (ptr - tempbf);
	#if DBGOUT
	sprintf(msg, "Enviando %d bytes: \n", sendlen);
	DBG(msg);
	for(int i = 0; i < sendlen; i++) {
		sprintf(msg, "%02X ", tempbf[i]);
		DBG(msg);
	}
	DBG("\n\n");
	#endif

	// Envía los datos por el port serial
	bool res = true;
	int nwrote = write(portHandle, tempbf, sendlen);
	if(nwrote < 0) {
		DisplayErrno("enviando bytes");
		res = false;
	}

	// Libera la memoria usada y vuelve
	free(tempbf);
	return res;
}

//
//  Lee un byte de lo enviado por el controlador fiscal.
//
static int ReadByte()
{
	char buffer[2];

	// Timeout 2 segundos
	for(int retries = 4; retries > 0; retries--) {
		int n = read(portHandle, buffer, 1);
		if(n < 0) {
			DisplayErrno("recibiendo bytes");
			return -1;
		}

		if (n > 0) {
			#if DBGOUT
			sprintf(msg, "%02X ", buffer[0] & 0xff);
			DBG(msg);
			#endif

			return buffer[0] & 0xff;
		}
	}

	DBG("Timeout\n");
	return -1;
}

//
//  Recibe datos del impresor fiscal, parseando los distintos campos de la respuesta.
static bool ReceiveData()
{
	int b;

	DBG("Recibido:\n");

	// Esperamos el STX que marca el comienzo de la respuesta
	nakReceived = false;
	while(true) {
		b = ReadByte();
		if (b == NAK) {
			DBG("Nak recibido\n");
			nakReceived = true;
			return false;
		}

		if (b == DC2 || b == DC4)
			HandleWait(b);

		if(b < 0)
			return false;
		
		if(b == STX)
			break;
	}

	uint32_t crc = STX;
	int rxStage = 0;
	bool inEscape = false;
	char *rxPtr = rxArguments;
	while(rxStage <= 16) {
		if((b = ReadByte()) < 0)
			return false;
		
		switch(rxStage) {

			// Recibió el número de secuencia
			case 0: rxSequence = (uint8_t)b; rxStage++; break;

			// Recibe los 2 byte del printer status
			case 1: rxPrinterStatus = 0x100 * (uint8_t)b; rxStage++; break;
			case 2: rxPrinterStatus += (uint8_t)b; rxStage++; break;

			// Recibe separadores
			case 3: 
			case 6:
			case 7:					// Campo 'reservado'
			case 10:				// Separador de headers
				if(b == FLD)
					rxStage++; 
				break;

			// Recibe los 2 byte del printer status
			case 4: rxFiscalStatus = 0x100 * (uint8_t)b; rxStage++; break;
			case 5: rxFiscalStatus += (uint8_t)b; rxStage++; break;

			// Recibe los 2 byte con el código de error
			case 8: rxErrorCode = 0x100 * (uint8_t)b; rxStage++; break;
			case 9: rxErrorCode += (uint8_t)b; rxStage++; break;

			// Recibe el resto de la respuesta, hasta encontrar un ETX
			case 11:
				if(b == ETX)
					rxStage  = 13;
				else if(b == ESC)
					rxStage = 12;
				else
					*rxPtr++ = (b == FLD)? '|': (char)b;
				break;

			// Recibió un ESC que nos indica quen este byte no se debe analizar
			case 12:
				*rxPtr++ = (char)b;
				rxStage = 10;
				break;

			// Caracteres correspondientes al CRC, los ignora
			// rxStage == 13, 14, 15 y 16
			default:
				rxStage++;
				break;
		}
	}

	// Marca el final de la respuesta recibida
	*rxPtr = 0;

	DBG("\n\n");
	return true;
}

//
//  Método que intercambia un mensaje con el controlador fiscal.
static bool ExchangeCommand(char *cmd)
{
	bool res = false;

	// Resetea las variables a usar para manejar errores.
	sysMsg = NULL;

	// Abre el port serial
	uint32_t crc = 0;
	if(OpenPort()) {
		do {
			res = SendData(cmd);
		}  while (!res && nakReceived);

		if(res)
			if(ReceiveData())
				res = true;

		DBG("Cerrando puerto\n\n");
		close(portHandle);
	}

	return res;
}

//
// Método utilitario que se puede usar en cualquier momento para imprimir los campos
// de estado devueltos por el impresor fiscal.
//
static void ShowResult()
{
	if(sysMsg != NULL)
		printf("Error '%s'\n", sysMsg);
	else {
		printf("Printer Status: %04X\n", rxPrinterStatus);
		printf("Fiscal Status.: %04X\n", rxFiscalStatus);
		printf("Error Code....: %04X\n", rxErrorCode);
		
		int argLen = strlen(rxArguments);
		if(argLen > 0)
			printf("Argumentos....: (%d) -> '%s'\n\n", argLen, rxArguments);
	}
}

//
// Método que emite una consulta de estado e imprime el resultado devuelto.
//
static void TestStatus()
{
	printf("\nConsultando estado...\n");
	ExchangeCommand((char *)"0001|0000");
	ShowResult();
}

// 
// Método que imprime un ticket a consumidor final con variantes de ítems.
//
static void TestTicketConsumidorFinal()
{
	printf("\nTicket a Consumidor Final\n");
	
	ExchangeCommand((char *)"0A01|0180");
	ExchangeCommand((char *)"0A02|0000|||||Item basico|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0000|||||Item con cantidad|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0000|||||Item a anular|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0001|||||Item a anular|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0000|||||Item IVA no general|100000|100000|1050|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0000|||||Item IVA exento|100000|100000|0|||||1234|1|2");
	ExchangeCommand((char *)"0A02|0000|||||Item no gravado|100000|100000|0|||||1234|1|1");
	ExchangeCommand((char *)"0A02|0000|||||Item imp.int. fijo|100000|100000|2100|200000||||1234|1|7");
	ExchangeCommand((char *)"0A02|0000|Linea de descripcion 4|Linea de descripcion 3|Linea de descripcion 2|Linea de descripcion 1|Item completo|95555|95545|1050|30000||||1234|1|7");
	ExchangeCommand((char *)"0A02|0004|||||Oferta sobre un item|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0000|||||Item con cantidad|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0006|||||Descuento en $ sobre un item|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A02|0008|||||Recargo en $ sobre un item|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0A04|0000|Oferta del dia|10000||101||Linea 2 de la oferta|Linea 3 de la oferta");
	ExchangeCommand((char *)"0A04|0001|Recargo Tarjeta de Credito|5000||101||Linea 2 del recargo|Linea 3 del recargo");
	ExchangeCommand((char *)"0A06|0107|3|Linea reemplazada 0123456789 0123456789 01234567|||||2");
}

//
//  Método que imprime un ticket factura A con variantes de ítems.
//
static void TestTicketFacturaA()
{
	printf("\nTicket Factura 'A'\n");

	ExchangeCommand((char *)"0B01|0000|Seven Group S.A.||Neochea 415|Monterrico - Jujuy||T|30578411174|I|083-00001-0000015|||");
	ExchangeCommand((char *)"0B02|0000|||||Item basico|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0000|||||Item con cantidad|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0000|||||Item a anular|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0001|||||Item a anular|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0000|||||Item IVA no general|100000|100000|1050|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0000|||||Item IVA exento|225000|100000|0|||||1234|1|2");
	ExchangeCommand((char *)"0B02|0000|||||Item no gravado|100000|100000|0|||||1234|1|1");
	ExchangeCommand((char *)"0B02|0000|||||Item imp.int. fijo|100000|100000|2100|200000||||1234|1|7");
	ExchangeCommand((char *)"0B02|0000|||||Item imp.int. porc.|100000|100000|2100||83330000|||1234|1|7");
	ExchangeCommand((char *)"0B02|0000|Linea de descripcion 4|Linea de descripcion 3|Linea de descripcion 2|Linea de descripcion 1|Item completo|95555|95545|1050||82690000|||1234|1|7");
	ExchangeCommand((char *)"0B02|0004|||||Oferta sobre un item|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0000|||||Item con cantidad|100000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0006|||||Descuento (en $) sobre un item|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B02|0008|||||Recargo (en $) sobre un item|10000|100000|2100|||||1234|1|7");
	ExchangeCommand((char *)"0B04|0000|Oferta del dia|10000||101||Linea 2 de la oferta|Linea 3 de la oferta");
	ExchangeCommand((char *)"0B04|0001|Recargo Tarjeta de Credito|5000||101||Linea 2 del recargo|Linea 3 del recargo");
	ExchangeCommand((char *)"0B20|0140|Prueba 1|2000|2100");
	ExchangeCommand((char *)"0B20|01C0|Prueba 2|8000|2100");
	ExchangeCommand((char *)"0B05|0000|Extra 1|Extra 2|6|Detalle|1051|20|50000");
	ExchangeCommand((char *)"0B05|0000||||||8|50000");
	ExchangeCommand((char *)"0B06|0107||||||");
}

//
//  Método que emite un cierre diario zeta.
//
static void TestZeta()
{
	printf("\nCierre diario (Zeta)\n");

	ExchangeCommand((char *)"0801|0000");
}

static bool ProcessCmdLine(int argc, char* argv[])
{
	int pState = 0;
	bool portUsed = false, repeatUsed = false;

	for (int i = 1; i < argc; i++) {

		char* opc = argv[i];
		switch(pState) {
			case 0:
				if (0 == strcmp("-r", opc))
					pState = 1;
				else {

					if (portUsed) return false;

					portUsed = true;
					portName = opc;
				}
				break;

			case 1:
				if (repeatUsed) return false;
				repeCount = atoi(opc);
				if (repeCount < 1 || repeCount >1000)
					repeCount = 1;
				repeatUsed = true;
				pState = 0;
				break;
		}
	}

	// Mínimo, tiene que haber indicado el port serial a usar.
	return portUsed;
}

//
//  Función principal
//
int main(int argc, char *argv[])
{
	// Establece el port serial a usar 
	if(!ProcessCmdLine(argc, argv)) {
		printf("Usar: tgenesis [-r num] <serialport>\nPor ejemplo: tgenesis -r 5 /dev/ttyS0\n\n");
		printf("  -r num: Indica la cantidadn de repeticiones de tickets a emitir\n");
		return 1;
	}

	// Ejecuta algunos comandos de prueba
	for (int iRepe = 1; iRepe <= repeCount; iRepe++) {

		if (repeCount > 1)
			printf("\n\n\n\nRepetición %d de %d\n", iRepe, repeCount);

		TestStatus();
		TestTicketConsumidorFinal();
		TestTicketFacturaA();
	}
	TestZeta();

	printf("\nListo. Pruebas concluídas\n");
	return 0;
}
