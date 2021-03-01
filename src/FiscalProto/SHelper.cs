using System;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace FiscalProto
{
    /// <summary>
    /// Helpers estaticos.
    /// </summary>
    public static class SHelper
    {
        /// <summary>
        /// Formatea un XML de forma que sea fácil de leer
        /// </summary>
        /// 
        /// <param name="xml">El texto en XML a formatear</param>
        /// <returns>El XML formateado.</returns>
        public static string PrettyXML(string xml)
        {
            var sb = new StringBuilder();
            var elem = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var w = XmlWriter.Create(sb, settings))
            {
                elem.Save(w);
            }

            return
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                Environment.NewLine +
                sb.ToString();
        }

        /// <summary>
        /// Devuelve el equivalente en hexadecimal de un comando.
        /// </summary>
        /// 
        /// <param name="cmd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ToHexaCommand(this CMD_Generic cmd, out string msg)
        {
            var mi = (MInput)cmd.InputObject;
            return mi.ToHexaCommand(out msg);
        }

        /// <summary>
        /// Devuelve el equivalente hexadecimal de un bloque de entrada
        /// </summary>
        /// 
        /// <param name="mi"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ToHexaCommand(this MInput mi, out string msg)
        {
            var cmdBin = mi.GetCommand();
            msg = null;

            if (mi.Error.Length != 0)
            {
                msg = "ERROR: " + mi.Error;
                return false;
            }

            var sb = new StringBuilder();
            sb.Clear();
            sb.Append($"{ (cmdBin[1] * 0x100 + cmdBin[2]).ToString("X").PadLeft(4, '0') }");
            sb.Append('|');
            sb.Append($"{ (cmdBin[4] * 0x100 + cmdBin[5]).ToString("X").PadLeft(4, '0') }");
            for (int i = 6; i < cmdBin.Length; i++)
            {
                byte b = cmdBin[i];
                if (b == MInput.FLD)
                    sb.Append('|');
                else
                    sb.Append((char)b);
            }

            msg = sb.ToString();
            return true;
        }
    }
}
