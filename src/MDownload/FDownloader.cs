using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FiscalProto;
using FiscalProto.Descarga;

namespace MDownload
{
    class FDownloader
    {
        int cmd = 0;
        bool decrypt = false;
        string carpeta = null;
        TargetBag bag;

        CMD_DNL_RangoFechas startF = new CMD_DNL_RangoFechas();
        CMD_DNL_RangoZetas startZ = new CMD_DNL_RangoZetas();
        CMD_DNL_Enumera gloop = new CMD_DNL_Enumera();
        CMD_DNL_Final end = new CMD_DNL_Final();
        CMD_DNL_Cancela canc = new CMD_DNL_Cancela();

        public FDownloader(int cmd, int conf, bool valid, bool borrar, string carpeta, bool decrypt)
        {
            _ = valid;
            _ = borrar;
            
            this.cmd = cmd;
            this.carpeta = carpeta;
            this.decrypt = decrypt;

            bag = TargetBag.FromCode(BagCode.Genesis);
        }

        private void DecryptPEMFile(string fn)
        {
            var result = new List<byte>();
            var lines = File.ReadAllLines(fn);
            for (int i = 1; i < lines.Length - 1; i++)
            {
                var lsrc = Convert.FromBase64String(lines[i]);
                result.AddRange(lsrc);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var b in result)
            {
                if (b >= 0x20 && b < 0xff)
                    sb.Append(((char)b).ToString());
            }

            string s = sb.ToString();
            s = s.Substring(s.IndexOf("<?xml "));
            int next = s.IndexOf('<', 1);
            string k = "";
            while (next++ < s.Length)
            {
                char ch = s[next];
                if (char.IsWhiteSpace(ch))
                    break;
                k += ch.ToString();
            }
            k = "</" + k + ">";
            next = s.IndexOf(k);
            s = s.Substring(0, next + k.Length);

            fn = Path.ChangeExtension(fn, "xml");
            File.WriteAllText(fn, s);
        }

        private string ShortPath(string fn)
        {
            if (fn.Length > 70)
                fn = fn.Substring(0, 10) + "..." + fn.Substring(fn.Length - 10);
            return fn;
        }

        public void DoStatus()
        {
            using (var proto = new ProtoHandler(bag))
            {
                var cmd = new CMD_DNL_Info();
                var mo = proto.ExchangePacket(cmd);
                if (mo == null) return;

                Console.WriteLine("<donwload>");
                Console.WriteLine($"  <ctd>{cmd.Output.DescargaCTDDesde}</ctd>");
                Console.WriteLine($"  <resu>{cmd.Output.DescargaRESUDesde}</resu>");
                Console.WriteLine($"  <dupliA>{cmd.Output.DescargaDUPLIDesde}</dupliA>");
                Console.WriteLine($"  <listo>{cmd.Output.ListasHasta}</listo>");
                Console.WriteLine($"  <borrado>{cmd.Output.BorradasHasta}</borrado>");
                Console.WriteLine("</donwload>");
            }
        }

        private void CommonDoWork(ProtoHandler proto, CMD_Generic start, Func<CMD_Generic, string> getFileName)
        {
            StringBuilder data = new StringBuilder();
            try
            {
                // Lanza la descarga.
                var mo = proto.ExchangePacket(start);
                if (mo == null || mo.ErrorCodeInt != 0)
                {
                    string msg =
                        (start as CMD_DNL_RangoFechas)?.Output.ErrorCode ??
                        (start as CMD_DNL_RangoZetas)?.Output.ErrorCode ??
                        "<Error Desconocido>";

                    Console.WriteLine($"ERROR: '{msg}'");
                    return;
                }

                // Lazo de descarga
                int prog = 0;
                Console.Write("Descargando: ");
                while (true)
                {
                    mo = proto.ExchangePacket(gloop);
                    if (mo.ErrorCodeInt != 0)
                        throw new Exception(mo.Error);
                    data.Append(gloop.Output.Data);
                    if (!gloop.Output.Continua)
                        break;

                    if (prog++ == 5)
                    {
                        Console.Write("\b\b\b\b\b     \b\b\b\b\b");
                        prog = 0;
                    }
                    else
                        Console.Write(".");
                }

                // Finaliza
                proto.ExchangePacket(end);
                var fn = Path.Combine(carpeta, getFileName(start));
                Console.Write($"\r                         \r==> '{ShortPath(fn)}'\n");
                File.WriteAllText(fn, data.ToString());
                if (decrypt)
                {
                    var xml = Path.ChangeExtension(fn, "xml");
                    Console.WriteLine($" Decrypt  '{ShortPath(fn)}' => '{ShortPath(xml)}");
                    DecryptPEMFile(fn);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - '{ex.Message}'");
                try
                {
                    proto.ExchangePacket(canc);
                }
                catch { }
            }
        }
    
        public void DoWorkByFecha(int fechaDesde, int fechaHasta)
        {
            using (var proto = new ProtoHandler(bag))
            {
                startF.Input.Selector = (cmd == 1) ?
                    DNLRF_Selector.CTD :
                    (cmd == 2) ? DNLRF_Selector.DUPLI_A : DNLRF_Selector.RESUMEN;

                startF.Input.FechaDesde = fechaDesde;
                startF.Input.FechaHasta = (fechaHasta == 0)? fechaDesde: fechaHasta;
                startF.Input.Zipeado = true;
                CommonDoWork(proto, startF, c => ((CMD_DNL_RangoFechas)c).Output.NomArchivo);
            }
        }

        public void DoWorkByZeta(int zetaDesde, int zetaHasta)
        {
            using (var proto = new ProtoHandler(bag))
            {
                startZ.Input.Selector = (cmd == 1) ?
                    DNLRZ_Selector.CTD :
                    (cmd == 2) ? DNLRZ_Selector.DUPLI_A : DNLRZ_Selector.RESUMEN;

                startZ.Input.ZetaDesde = zetaDesde;
                startZ.Input.ZetaHasta = (zetaHasta == 0)? zetaDesde: zetaHasta;
                startZ.Input.Zipeado = true;
                CommonDoWork(proto, startZ, c => ((CMD_DNL_RangoZetas)c).Output.NomArchivo);
            }
        }
    }
}
