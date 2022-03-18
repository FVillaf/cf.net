using System;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace FiscalProto
{
    public abstract class MOutput
    {
        const byte FLD = 0x1C;
        const byte ESC = 0x1B;
      
        int printerStatus, fiscalStatus, errorCode;

        public abstract void SetFromCommand(byte[] data);

        [Category("General")]
        [Description("El estado mecanico de la impresora. Pulsar el boton 'Printer Stat' para ver detalles.")]
        public string PrinterStatus
        {
            get { return printerStatus.ToString("X").PadLeft(4, '0'); }
        }

        [Category("General")]
        [Description("El estado fiscal de la impresora. Pulsar el boton 'Fiscal Stat' para ver detalles.")]
        public string FiscalStatus
        {
            get { return fiscalStatus.ToString("X").PadLeft(4, '0'); }
        }

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

        [Browsable(false)]
        public int ErrorCodeInt
        {
            get { return errorCode; }
        }

        [Browsable(false)]
        [Description("Error generado al desensamblar el comando"), ReadOnly(true)]
        public string Error { get; protected set; }

        private void SkipFieldSep(BinReader cmd)
        {
            if (cmd.ReadNext() != FLD)
                throw new ArgumentException("Esperaba separador FLD");
        }

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

        protected string Extract_A(BinReader cmd, int length, bool optional)
        {
            return Extract_RT(cmd, length, optional);
        }

        protected string Extract_P(BinReader cmd, int length, bool optional)
        {
            SkipFieldSep(cmd);

            var sb = new StringBuilder();
            while (!cmd.EOC)
            {
                if (cmd.PeekNext() == ESC)
                    cmd.ReadNext();     // Skip ESC char
                else
                {
                    if (cmd.PeekNext() == FLD)
                        break;
                }

                sb.Append((char)cmd.ReadNext());
            }
            return sb.ToString();
        }

        protected string Extract_RT(BinReader cmd, int length, bool optional)
        {
            SkipFieldSep(cmd);

            var sb = new StringBuilder();
            while (!cmd.EOC && cmd.PeekNext() != FLD)
                sb.Append((char)cmd.ReadNext());
            return sb.ToString();
        }

        protected string Extract_L(BinReader cmd, int length, bool optional) { return Extract_RT(cmd, length, optional); }

        protected int Extract_N(BinReader cmd, int length, bool optional)
        {
            string text = Extract_RT(cmd, length, optional);
            if (text.Length == 0)
                return 0;

            return int.Parse(text);
        }

        protected decimal Extract_N(BinReader cmd, int length, int decim, bool optional)
        {
            string text = Extract_RT(cmd, length, optional);
            if (text.Length == 0)
                return 0;

            decimal num = decimal.Parse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            while (decim-- > 0)
                num /= 10;
            return num;

        }

        protected int Extract_D(BinReader cmd, int length, bool optional)
        {
            string text = Extract_RT(cmd, length, optional);
            if (text.Length == 0)
                return 0;

            return int.Parse(text);
        }

        protected int Extract_T(BinReader cmd, int length, bool optional)
        {
            return Extract_D(cmd, length, optional);
        }

        protected byte[] Extract_B(BinReader cmd, int length, bool optional)
        {
            var result = new List<byte>();
            while (!cmd.EOC && cmd.PeekNext() != FLD)
                result.Add(cmd.ReadNext());
            return result.ToArray();
        }

        protected bool Extract_Y(BinReader cmd, int length, bool optional)
        {
            SkipFieldSep(cmd);
            if (!cmd.EOC && cmd.PeekNext() != FLD)
            {
                char ch = (char)cmd.ReadNext();
                return (ch != 'n' && ch != 'N');
            }
            return false;
        }

        protected string Extract_Reservado(BinReader cmd, int length, bool optional)
        {
            SkipFieldSep(cmd);
            return string.Empty;
        }
    }

    public class BinReader
    {
        byte[] buff;

        public int Ptr { get; set; }

        public bool EOC {  get { return buff.Length <= Ptr; } }

        public byte PeekNext()
        {
            return PeekNext(0);
        }

        public byte PeekNext(int offset)
        {
            if (Ptr + offset >= buff.Length)
                return 0;

            return buff[Ptr + offset];
        }

        public byte ReadNext() 
        {
            return ReadNext(true);
        }

        public byte ReadNext(bool adv)
        {
            byte result = buff[Ptr];
            if (adv) Ptr++;
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
