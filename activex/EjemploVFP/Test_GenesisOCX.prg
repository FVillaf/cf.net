**
** Ejemplo de uso, FiscalProtoOCX (nueva generación) de Moretti.
**

x = CREATEOBJECT("Moretti.FiscalProto")

** Las posibilidades on:
**
** Usar canal serial. Indicar el com y, opcionalmente, la velocidad. La velocidad 
** default es 115200.
**			"serial=com3"
**			"serial=com1:115200"
** 
** Usar canal ethernet. En este caso, indicar dirección IP (no nombre DNS). El puerto
** SIEMPRE SERA el 5003
**			"net=192.168.1.1"
**
IF !x.Open("net=192.168.1.11")
   MESSAGEBOX("No pudo abrir el port indicado")
   RETURN
ENDIF

** Ejemplo de acceso directo a funciones frecuentes
? "Numero de serie: " + x.GetSerial()

** Ejemplo de uso de comando para construir transacciones
TestTicketComun()
TestTicketFacturaA()
TestTicketFacturaB()
TestZeta()

** Listo, cierra el objeto
x.Close()
RELEASE x




*************************************************************************************
** ticket comun, a consumidor final                                                **
*************************************************************************************
PROCEDURE TestTicketComun

x.Execute(x.Ticket.TKAbrir)

x.Ticket.TKItem.Input.Descrip = "ACEITE PATITO LATA"
x.Ticket.TKItem.Input.Cantidad = 3
x.Ticket.TKItem.Input.Unitario = 13.50
x.Ticket.TKItem.Input.TasaIVA = 2100
x.Execute(x.Ticket.TKItem)

x.ticket.TKPago.Input.Codigo = 7
x.ticket.TKPago.Input.Monto = 100
x.Execute(x.ticket.tkpago)

x.Execute(x.ticket.tkcerrar)

ENDPROC



*************************************************************************************
** ticket factura "A" a responsable inscripto                                      **
*************************************************************************************
procedure TestTicketFacturaA

x.TicketFactura.TFAbrir.input.nomcliente_1 = "Pescado SRL"
x.TicketFactura.TFAbrir.input.direcCliente_1 = "Solorzano 950"
x.TicketFactura.TFAbrir.input.direccliente_2 = "Salta capital"
x.ticketfactura.tfabrir.input.tipodoc = ASC("T")
x.ticketfactura.tfabrir.input.nrodoc = "20168993278"
x.TicketFactura.TFAbrir.input.respiva = ASC("I")
x.Execute(x.TicketFactura.TFAbrir)

x.TicketFactura.TFItem.Input.Descrip = "ACEITE PATITO LATA"
x.TicketFactura.TFItem.Input.Cantidad = 3
x.TicketFactura.TFItem.Input.Unitario = 13.50
x.Ticketfactura.TFItem.Input.TasaIVA = 2100
x.Execute(x.TicketFactura.TFItem)

x.Execute(x.TicketFactura.TFCerrar)

ENDPROC



*************************************************************************************
** ticket factura "B" a monotributista                                             **
*************************************************************************************
procedure TestTicketFacturaB

x.TicketFactura.TFAbrir.input.nomcliente_1 = "Alberto Fer"
x.TicketFactura.TFAbrir.input.direcCliente_1 = "Avda Rivadavia 1"
x.TicketFactura.TFAbrir.input.direccliente_2 = "CABA"
x.ticketfactura.tfabrir.input.tipodoc = ASC("T")
x.ticketfactura.tfabrir.input.nrodoc = "20168993278"
x.TicketFactura.TFAbrir.input.respiva = ASC("M")
x.Execute(x.TicketFactura.TFAbrir)

x.TicketFactura.TFItem.Input.Descrip = "ACEITE PATITO LATA"
x.TicketFactura.TFItem.Input.Cantidad = 3
x.TicketFactura.TFItem.Input.Unitario = 13.50
x.Ticketfactura.TFItem.Input.TasaIVA = 2100
x.Execute(x.TicketFactura.TFItem)

x.Execute(x.TicketFactura.TFCerrar)

ENDPROC




*************************************************************************************
** emite un cierre diario zeta                                                     **
*************************************************************************************
PROCEDURE TestZeta

x.Execute(x.JornadaFiscal.JORZeta)

ENDPROC
