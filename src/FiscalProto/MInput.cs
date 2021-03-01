using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiscalProto
{
    /// <summary>
    /// Clase abstracta que define la interface basica de los bloques de entrada de los comandos.
    /// </summary>
    /// 
    /// <remarks>
    /// Estos bloques se usan para enviar argumentos y parámetros al controlador fiscal, como parte
    /// de un comando.
    /// </remarks>
    public abstract class MInput
    {
        /// <summary>
        /// Separador de campo.
        /// </summary>
        public const byte FLD = 0x1c;

        /// <summary>
        /// Sequenciador de bloques.
        /// </summary>
        static int reqCounter = 0x81;

        /// <summary>
        /// Devuelve el binario del comando.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetCommand();

        /// <summary>
        /// Error generado al ensamblar el comando, que impide generar los bytes correctamente.
        /// </summary>
        [Browsable(false)]
        [Category("Error")]
        [Description("Error generado al ensamblar el comando"), ReadOnly(true)]
        public string Error { get; protected set; }

        /// <summary>
        /// Setea el código de comando y la extensión.
        /// </summary>
        /// <param name="cmdnum">El código de comando.</param>
        /// <param name="extension">La extensión</param>
        /// <param name="result">La lista donde se agregarán los bytes que se generen.</param>
        protected void SetOpcode(int cmdnum, int extension, List<byte> result)
        {
            result.Add((byte)0xa9/*reqCounter++*/);
            if (reqCounter > 0xff) reqCounter = 0x81;

            // Agrega el comando (big endian)
            result.Add((byte)(cmdnum / 0x100));
            result.Add((byte)cmdnum);

            // Agrega la extension (big endian)
            result.Add(FLD);
            result.Add((byte)(extension / 0x100));
            result.Add((byte)extension);
        }

        /// <summary>
        /// Agrega un campo binario.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_B(List<byte> cmd, byte[] data, int digits, int decim, bool optional)
        {
            _ = optional;

            if (data == null || (digits > 0 && data.Length != digits) || decim < 0)
                throw new ArgumentException("Largo incorrecto de datos B (Binarios)");

            cmd.Add(FLD);
            cmd.AddRange(data);
        }

        /// <summary>
        /// Agrega un campo numérico a partir de un entero.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="num"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_N(List<byte> cmd, int num, int digits, int decim, bool optional)
        {
            if (num < 0 || decim != 0)
                throw new ArgumentException("El 'num' a enviar no puede ser negativo");

            cmd.Add(FLD);
            if (num == 0 && optional) return;
            string tmp = num.ToString();
            if (digits < tmp.Length)
                throw new Exception($"Append_N overflow. No se soportan mas de {digits} digitos");
            foreach (char ch in tmp)
                cmd.Add((byte)ch);
        }

        /// <summary>
        /// Agrega un campo numérico a partir de un decimal.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="num"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_N(List<byte> cmd, decimal num, int digits, int decim, bool optional)
        {
            if (num < 0)
                throw new ArgumentException("El 'num' a enviar no puede ser negativo");

            cmd.Add(FLD);
            if (num == 0 && optional) return;
            if (digits > 0)
            {
                while (decim-- > 0)
                    num *= 10;
            }

            string tmp = ((int)num).ToString();
            foreach (char ch in tmp)
                cmd.Add((byte)ch);
        }

        /// <summary>
        /// Agrega una fecha.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="date"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_D(List<byte> cmd, int date, int digits, int decim, bool optional)
        {
            _ = optional;
            _ = digits;
            _ = decim;
            Append_N(cmd, date, 6, 0, optional);
        }

        /// <summary>
        /// Agrega una hora.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="time"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_T(List<byte> cmd, int time, int digits, int decim, bool optional)
        {
            _ = digits;
            _ = decim;
            Append_N(cmd, time, 6, 0, optional);
        }

        /// <summary>
        /// Agrega un rich text
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="text"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_RT(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
            _ = digits;
            _ = decim;
            cmd.Add(FLD);
            if (optional && (text == null || text.Length == 0))
                return;

            if (text == null)
                throw new ArgumentException("Debe indicar 'Texto'");

            if (digits > 0)
            {
                if (digits > text.Length)
                    text = text.PadRight(digits - text.Length, ' ');
                else if (digits < text.Length)
                    text = text.Substring(0, digits);
            }

            text = text.Trim();
            for(int i=0; i<text.Length; i++)
            {
                char ch = text[i];
                if (ch >= ' ' && ((int)ch) < 0xff)
                    cmd.Add((byte)ch);
            }
        }

        /// <summary>
        /// Agrega un campo ascii sin formato.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="text"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_A(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
            _ = digits;
            _ = decim;

            Append_RT(cmd, text, digits, 0, optional);
        }

        /// <summary>
        /// Agrega un campo ascii con formato.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="text"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_P(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
            _ = decim;

            cmd.Add(FLD);
            if (optional && (text == null || text.Length == 0))
                return;

            if (text == null)
                throw new ArgumentException("Debe indicar 'Texto'");

            if (digits > 0)
            {
                //if (digits > text.Length)
                //    text = text.PadRight(digits - text.Length, ' ');
                //else 
                if (digits < text.Length)
                    text = text.Substring(0, digits);
            }
            foreach (var ch in text)
            {
                if (ch == FLD)
                    cmd.Add((byte)FLD);
                cmd.Add((byte)ch);
            }
        }

        /// <summary>
        /// Agrega un campo de texto limitado.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="text"></param>
        /// <param name="digits"></param>
        /// <param name="decim"></param>
        /// <param name="optional"></param>
        protected void Append_L(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
            _ = decim;

            cmd.Add(FLD);
            if (optional && (text == null  || text.Length == 0))
                return;

            if (text == null)
                throw new ArgumentException("Debe indicar 'Texto'");

            if (digits > 0)
            {
                if (digits > text.Length)
                    text = text.PadRight(digits - text.Length, ' ');
                else if (digits < text.Length)
                    text = text.Substring(0, digits);
            }
            foreach (var ch in text)
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
                    cmd.Add((byte)ch);
            }
        }
    }
}
