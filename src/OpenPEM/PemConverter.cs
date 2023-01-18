using System;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Pkcs;

namespace OpenPEM
{
    /// <summary>
    /// Helper class para convertir los archivos fiscales.
    /// </summary>
    static class PemConverter
    {
        /// <summary>
        /// Version del conversor
        /// </summary>
        public static string Version => "1.0";

        /// <summary>
        /// Tool que convierte texto PEM en puros bytes
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        static byte[] Pem2Bin(string pem)
        {
            string base64Str;

            pem = pem.Replace("\r", "").Replace("\n", "");
            if (pem.StartsWith("-----BEGIN "))
            {
                int epos = pem.IndexOf("-----", 11);
                if (epos >= 0 && epos < 30)
                {
                    var kind = pem.Substring(11, epos - 11);
                    var etag = $"-----END { kind }-----";
                    var tagpos = pem.IndexOf(etag);
                    if (tagpos < 0)
                        throw new Exception($"No encuentro '{etag}' en el archivo");
                    base64Str = pem.Substring(epos + 5, tagpos - epos - 5);
                }
                else
                    base64Str = pem;
            }
            else
                base64Str = pem;

            base64Str = base64Str.Replace(" ", "");
            return Convert.FromBase64String(base64Str);
        }

        /// <summary>
        /// API que convierte el archivo fiscal PEM en XML
        /// </summary>
        /// <param name="inFn">Path completo al archivo PEM a convertir</param>
        /// <param name="outFn">Path completo donde se guardara el XML convertido</param>
        public static void Convert2Xml(string inFn, string outFn, bool raw = false)
        {
            if (!File.Exists(inFn))
                throw new Exception("El archivo PEM indicado no existe");

            var bytes = Pem2Bin(File.ReadAllText(inFn));
            var cms = new SignedCms();
            cms.Decode(bytes);
            if (cms.ContentInfo.ContentType.FriendlyName != "Datos PKCS 7")
                throw new Exception("El archivo no contiene info PKCS7");
            
            var xml = Encoding.UTF8.GetString(cms.ContentInfo.Content);
            if (!raw)
            {
                try { xml = PrettyXML(xml); } catch { }
            }

            File.WriteAllText(outFn, xml);
        }

        /// <summary>
        /// API que ayuda a derivar el nombre del archivo 'target' dado el source PEM.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string DeriveTarget(string source)
        {
            string nFilename = source;
            var parts = Path.GetFileNameWithoutExtension(nFilename).Split('.');
            if (parts.Length >= 4)
            {
                nFilename = Path.Combine(
                    Path.GetDirectoryName(nFilename),
                    $"{parts[0]}.{parts[1]}.{parts[2]}.{parts[3]}");
            }

            return Path.ChangeExtension(nFilename, "xml");
        }

        /// <summary>
        /// Convierte un XML raw (sin formato) en un XML formateado
        /// </summary>
        /// 
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string PrettyXML(string xml)
        {
            var sb = new StringBuilder();
            var elem = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var w = XmlWriter.Create(sb, settings))
                elem.Save(w);

            return
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                Environment.NewLine +
                sb.ToString();
        }

    }
}
