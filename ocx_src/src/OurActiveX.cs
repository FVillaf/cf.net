using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace FiscalProto
{
	/// <summary>
	/// Clase prinicipal de implementación del OCX para genesis.
	/// </summary>
	[ProgId("Moretti.FiscalProto")]
	[ClassInterface(ClassInterfaceType.AutoDual), ComSourceInterfaces(typeof(ControlEvents))]
	[Guid("121C3E0E-DC6E-45dc-952B-A6617F0FAA32")]
	[ComVisible(true)]
	public partial class GenesisOCXObject : IDisposable
	{
        // Mensaje de ejmplo, para la ayuda.
        const string EJEMPLO = " Por ejemplo, 'net=192.168.1.1' o 'serial=com6' o 'serial=com6:115200'";
        string lastError;
        string remote;
        Proto proto;
        bool disposed;

        /// <summary>
        /// Expone el mensaje correspondiente al ultimo error detectado al comunicarse con el Genesis.
        /// </summary>
        /// <value>El mensaje de error, o una <b>cadena vacia</b> si no hay error.</value>
        [ComVisible(true)]
        public string LastError { get { return (lastError != null) ? lastError : ""; } }

        /// <summary>
        /// Desactiva y desconecta este modulo.
        /// </summary>
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            if (proto != null)
                proto.Dispose();
        }

        /// <summary>
        /// Helper interno que se asegura que el puerto esté bien configurado y activo.
        /// </summary>
        /// 
        /// <returns></returns>
        bool CheckPort()
        {
            if (proto == null || proto.Port == null || !proto.Port.IsOpen)
            {
                lastError = "el puerto no está abierto";
                return false;
            }

            return true;
        }

		/*public event ControlEventHandler OnClose;*/

        /// <summary>
        /// Método que debe llamarse antes de cualquier transacción con el Génesis,
        /// indicando el canal de comunicación a usar.
        /// </summary>
        /// <param name="remote"></param>
        /// <returns></returns>
        [ComVisible(true)]
        public bool Open(string remote)
        {
            if (string.IsNullOrEmpty(remote))
            {
                lastError = "Indique el port a usar." + EJEMPLO;
                return false;
            }

            Close();
            try
            {
                if (!Remote.ValidatePort(remote))
                {
                    lastError = "Port Invalido." + EJEMPLO;
                    return false;
                }

                this.remote = remote;
                VirtualPort port = Remote.CreatePort(remote);
                port.Open();
                if (!port.IsOpen)
                {
                    lastError = "No puedo abrir el port indicado";
                    return false;
                }

                proto = new Proto(port);
                return true;
            }
            catch (Exception ex) 
            {
                lastError = ex.Message;
                return false; 
            }
        }

        /// <summary>
        /// Cierra el puerto de comunicación.
        /// </summary>
        [ComVisible(true)]
        public void Close()
        {
            if(proto != null)
                proto.Dispose();
            proto = null;
        }

        /// <summary>
        /// Helper method usado para reenpaquetar la respuesta del Genesis.
        /// </summary>
        /// 
        /// <returns></returns>
        string PackAnswer()
        {
            var sb = new StringBuilder();
            foreach (var part in proto.Answer)
            {
                if (sb.Length > 0) sb.Append('|');
                sb.Append(part);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Ejecuta un comando crudo, a partir de los bytes que lo describen.
        /// </summary>
        /// 
        /// <param name="cmd">El comando a enviar. Usar el manual del protocolo de
        /// comunicación para conformar el comando.
        /// </param>
        /// 
        /// <returns>Los datos retornados por el Genesis (consultar el manual de protocolo
        /// por las respuestas apropiadas para cada comando.
        /// </returns>
        /// 
        /// <example>
        /// Por ejemplo, si se envía <b>"0005|0000"</b>, el Genesis devolverá el nro de serie.
        /// </example>
        [ComVisible(true)]
        public string ExecuteCrude(string cmd)
        {
            if (!CheckPort()) return "error";
            if (!proto.Exchange(cmd) || proto.WError)
            {
                lastError = ErrorCodes.Get(proto.ErrorCode);
                return "error";
            }

            return PackAnswer();
        }

        /// <summary>
        /// Devuelve el número de serie del impresor.
        /// </summary>
        /// 
        /// <returns>El número de serie.</returns>
        [ComVisible(true)]
        public string GetSerial()
        {
            var res = ExecuteCrude("0005|0000");
            if (res != "error")
                res = proto.Answer[1];
            return res;
        }

        /// <summary>
        /// Ejecuta un comando 
        /// </summary>
        /// <param name="pCmd"></param>
        /// <returns></returns>
        [ComVisible(true)]
        public object Execute(object pCmd)
        {
            if (pCmd == null)
            {
                lastError = "Parametro null";
                return null;
            }

            CMD_Generic cmd = (CMD_Generic)pCmd;
            var data = (cmd.InputObject as MInput).GetCommand();
            var res = proto.ExchangePacketBin(data);
            if (proto.WError)
            {
                lastError = ErrorCodes.Get(proto.ErrorCode);
                return null;
            }

            var mo = (cmd.OutputObject as MOutput);
            mo.SetFromCommand(res);
            if (mo.ErrorCodeInt != 0)
            {
                lastError = ErrorCodes.Get(mo.ErrorCodeInt);
                return false;
            }

            return mo;
        }

		///	<summary>
		///	Registra la clase en el registro.
		///	</summary>
        ///	
		///	<param name="key">La clave de registro del control</param>
		[ComRegisterFunction()]
		public static void RegisterClass ( string key )
		{
			// Strip off HKEY_CLASSES_ROOT\ from the passed key as I don't need it
			StringBuilder	sb = new StringBuilder ( key ) ;
			
			sb.Replace(@"HKEY_CLASSES_ROOT\","") ;
			// Open the CLSID\{guid} key for write access
			RegistryKey k	= Registry.ClassesRoot.OpenSubKey(sb.ToString(),true);

			// And create	the	'Control' key -	this allows	it to show up in
			// the ActiveX control container
			RegistryKey ctrl = k.CreateSubKey	( "Control"	) ;
			ctrl.Close ( ) ;

			// Next create the CodeBase entry	- needed if	not	string named and GACced.
			RegistryKey inprocServer32 = k.OpenSubKey	( "InprocServer32" , true )	;
			inprocServer32.SetValue (	"CodeBase" , Assembly.GetExecutingAssembly().CodeBase )	;
			inprocServer32.Close ( ) ;
				// Finally close the main	key
			k.Close (	) ;
			MessageBox.Show("Registered");
		}

		///	<summary>
		///	Función llamada para deregistrar el control.
		///	</summary>
		///	<param name="key">Tke registry key</param>
		[ComUnregisterFunction()]
		public static void UnregisterClass ( string	key	)
		{
			StringBuilder	sb = new StringBuilder ( key ) ;
			sb.Replace(@"HKEY_CLASSES_ROOT\","") ;

			// Open	HKCR\CLSID\{guid} for write	access
			RegistryKey	k =	Registry.ClassesRoot.OpenSubKey(sb.ToString(),true);

			// Delete the 'Control'	key, but don't throw an	exception if it	does not exist
			k.DeleteSubKey ( "Control" , false ) ;

			// Next	open up	InprocServer32
			//RegistryKey	inprocServer32 = 
			k.OpenSubKey (	"InprocServer32" , true	) ;

			// And delete the CodeBase key,	again not throwing if missing
			k.DeleteSubKey ( "CodeBase"	, false	) ;

			// Finally close the main key
			k.Close	( )	;
			MessageBox.Show("UnRegistered");
		}
    }

	/// <summary>
	/// El controlador de eventos
	/// </summary>
	public delegate void PaperFailDelegate();


	/// <summary>
	/// Eventos expuestos
	/// </summary>
	[Guid("68BD4E0D-D7BC-4cf6-BEB7-CAB950161E79")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface ControlEvents
	{
		[DispId(0x60020001)]
		void OnPaperFail();
	}
}
