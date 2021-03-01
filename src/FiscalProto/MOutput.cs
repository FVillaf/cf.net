using System;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiscalProto
{
    /// <summary>
    /// Clase abstracte que describe la interfaz básica de un bloque de salida.
    /// </summary>
    /// 
    /// <remarks>
    /// Los bloques de salida son aquellos por los cuales el controlador fiscal devuelve
    /// información al host.
    /// </remarks>
    public abstract class MOutput
    {
        /// <summary>
        /// Separador de campos.
        /// </summary>
        const byte FLD = 0x1C;

        /// <summary>
        /// Todos los comandos tienen este campo.
        /// </summary>
        int printerStatus, fiscalStatus, errorCode;

        /// <summary>
        /// Setea el objeto de salida a partir de los datos en binario recibidos desde el controlador
        /// fiscal.
        /// </summary>
        /// 
        /// <param name="data">Los datos en binario</param>
        public abstract void SetFromCommand(byte[] data);

        /// <summary>
        /// El estado mecánico de la impresora.
        /// </summary>
        [Category("General")]
        [Description("El estado mecanico de la impresora. Pulsar el boton 'Printer Stat' para ver detalles.")]
        public string PrinterStatus
        {
            get { return printerStatus.ToString("X").PadLeft(4, '0'); }
        }

        /// <summary>
        /// El estado fiscal de la impresora.
        /// </summary>
        [Category("General")]
        [Description("El estado fiscal de la impresora. Pulsar el boton 'Fiscal Stat' para ver detalles.")]
        public string FiscalStatus
        {
            get { return fiscalStatus.ToString("X").PadLeft(4, '0'); }
        }

        /// <summary>
        /// Código de error devuelto por el comando, en 4 digitos hexadecimales.
        /// </summary>
        [Category("General")]
        [Description("El codigo de error devuelto por la impresora. 0000 indica SIN ERROR.")]
        public string ErrorCode
        {
            get
            {
                return
                    errorCode.ToString("X").PadLeft(4, '0') +
                    "-" +
                    ErrorCodes.Get(errorCode);
            }
        }

        /// <summary>
        /// Código de error del comando. 0=Sin error.
        /// </summary>
        [Browsable(false)]
        public int ErrorCodeInt
        {
            get { return errorCode; }
        }

        /// <summary>
        /// Mensaje de error descriptivo.
        /// </summary>
        [Browsable(false)]
        [Description("Error generado al desensamblar el comando"), ReadOnly(true)]
        public string Error { get; protected set; }

        /// <summary>
        /// Saltea un 'posible' separador de campo.
        /// </summary>
        /// 
        /// <param name="cmd">el lector del comando</param>
        private void SkipFieldSep(BinReader cmd)
        {
            if (cmd.ReadNext() != FLD)
                throw new ArgumentException("Esperaba separador FLD");
        }

        /// <summary>
        /// Parse las partes comunes de todas las respuestas a comandos.
        /// </summary>
        /// 
        /// <param name="cmd">El lector de comandos.</param>
        protected void ParseAnswerHeader(BinReader cmd)
        {
            cmd.Ptr = 1;
            printerStatus = cmd.ReadNext() * 0x100 + cmd.ReadNext();

            SkipFieldSep(cmd);
            fiscalStatus = cmd.ReadNext() * 0x100 + cmd.ReadNext();

            SkipFieldSep(cmd);  // Reserved 1

            SkipFieldSep(cmd);
            errorCode = cmd.ReadNext() * 0x100 + cmd.ReadNext();

            if (errorCode != 0)
                this.Error = "Codigo: 0x" + errorCode.ToString("X").PadLeft(4, '0');

            SkipFieldSep(cmd);  // Reserved 1
        }

        /// <summary>
        /// Extrae un campo ASCII
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected string Extract_A(BinReader cmd, int length, bool optional)
        {
            return Extract_RT(cmd, length, optional);
        }

        /// <summary>
        /// Extrae un campo ascii con formato.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected string Extract_P(BinReader cmd, int length, bool optional)
        {
            _ = length;
            _ = optional;

            SkipFieldSep(cmd);

            var sb = new StringBuilder();
            while (!cmd.EOC)
            {
                if (cmd.PeekNext() == FLD)
                {
                    if (cmd.PeekNext(1) != FLD)
                        break;

                    cmd.ReadNext();     // Read the escaped FLD
                }

                sb.Append((char)cmd.ReadNext());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Extrae un campo de texto enriquecido.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected string Extract_RT(BinReader cmd, int length, bool optional)
        {
            _ = length;
            _ = optional;

            SkipFieldSep(cmd);

            var sb = new StringBuilder();
            while (!cmd.EOC && cmd.PeekNext() != FLD)
                sb.Append((char)cmd.ReadNext());
            return sb.ToString();
        }

        /// <summary>
        /// Extrae un campo booleano.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected string Extract_L(BinReader cmd, int length, bool optional)
        {
            _ = cmd;
            _ = length;
            _ = optional;

            return null;
        }

        /// <summary>
        /// Extrae un campo numérico como un valor entero.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected int Extract_N(BinReader cmd, int length, bool optional)
        {
            string text = Extract_RT(cmd, length, optional);
            return int.Parse(text);
        }

        /// <summary>
        /// Extrae un campo numérico como un valor con posibles decimales.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected decimal Extract_N(BinReader cmd, int length, int decim, bool optional)
        {
            _ = decim;

            string text = Extract_RT(cmd, length, optional);
            return decimal.Parse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

        }

        /// <summary>
        /// Extrae un campo FECHA
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected int Extract_D(BinReader cmd, int length, bool optional)
        {
            string text = Extract_RT(cmd, length, optional);
            return int.Parse(text);
        }

        /// <summary>
        /// Extrae un campo HORA
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected int Extract_T(BinReader cmd, int length, bool optional)
        {
            return Extract_D(cmd, length, optional);
        }

        /// <summary>
        /// Extrae un campo Binario.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected byte[] Extract_B(BinReader cmd, int length, bool optional)
        {
            _ = length;
            _ = optional;

            var result = new List<byte>();
            while (!cmd.EOC && cmd.PeekNext() != FLD)
                result.Add(cmd.ReadNext());
            return result.ToArray();
        }

        /// <summary>
        /// Extrae un campo si/no.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected bool Extract_Y(BinReader cmd, int length, bool optional)
        {
            _ = length;
            _ = optional;

            SkipFieldSep(cmd);
            if (!cmd.EOC && cmd.PeekNext() != FLD)
            {
                char ch = (char)cmd.ReadNext();
                return (ch != 'n' && ch != 'N');
            }
            return false;
        }

        /// <summary>
        /// Extrae un campo reservado.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="length"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        protected string Extract_Reservado(BinReader cmd, int length, bool optional)
        {
            _ = length;
            _ = optional;

            SkipFieldSep(cmd);
            return string.Empty;
        }
    }

    /// <summary>
    /// Helper class que permite leer una respuesta binaria.
    /// </summary>
    public class BinReader
    {
        byte[] buff;

        public int Ptr { get; set; }

        public bool EOC {  get { return buff.Length <= Ptr; } }

        public byte PeekNext(int offset = 0)
        {
            if (Ptr + offset >= buff.Length)
                return 0;

            return buff[Ptr + offset];
        }

        public byte ReadNext(int ptr = -1)
        {
            if (ptr >= 0) Ptr = ptr;
            byte result = buff[Ptr];
            Ptr += 1;
            return result;
        }

        public byte this[int index]
        {
            get { return buff[Ptr + index]; }
        }

        public BinReader(byte[] buff)
        {
            this.buff = buff;
        }
    }
}
