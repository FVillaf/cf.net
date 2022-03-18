using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace FiscalProto
{
    public abstract class MInput
    {
        public const byte FLD = 0x1c;

        static int reqCounter = 0x81;

        public abstract byte[] GetCommand();

        [Browsable(false)]
        [Category("Error")]
        [Description("Error generado al ensamblar el comando"), ReadOnly(true)]
        public string Error { get; protected set; }

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

        protected void Append_B(List<byte> cmd, byte[] data, int digits, int decim, bool optional)
        {
            if (data == null || (digits > 0 && data.Length != digits) || decim < 0)
                throw new ArgumentException("Largo incorrecto de datos B (Binarios)");

            cmd.Add(FLD);
            cmd.AddRange(data);
        }

        protected void Append_N(List<byte> cmd, int num, int digits, int decim, bool optional)
        {
            if (num < 0 || decim != 0)
                throw new ArgumentException("El 'num' a enviar no puede ser negativo");

            cmd.Add(FLD);
            if (num == 0 && optional) return;
            string tmp = num.ToString();
            if (digits < tmp.Length)
                throw new Exception(string.Format("Append_N overflow. No se soportan mas de {0} digitos", digits));
            foreach (char ch in tmp)
                cmd.Add((byte)ch);
        }

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

            string tmp = num.ToString("F0");
            foreach (char ch in tmp)
                cmd.Add((byte)ch);
        }

        protected void Append_D(List<byte> cmd, int date, int digits, int decim, bool optional)
        {
            Append_N(cmd, date, 6, 0, optional);
        }

        protected void Append_T(List<byte> cmd, int time, int digits, int decim, bool optional)
        {
            Append_N(cmd, time, 6, 0, optional);
        }

        protected void Append_RT(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
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

        protected void Append_A(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
            Append_RT(cmd, text, digits, 0, optional);
        }

        protected void Append_P(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
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

        protected void Append_L(List<byte> cmd, string text, int digits, int decim, bool optional)
        {
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
