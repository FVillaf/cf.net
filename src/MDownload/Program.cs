using System;
using System.IO;
using FiscalProto;

namespace MDownload
{
    class Program
    {
        static void ShowLogo()
        {
            Console.WriteLine("MFD" + MVersion.Name + " - Moretti File Downloader");
            Console.WriteLine("Copyright (c) 2018 - Andres Moretti e Hijos S.A.\n");
        }

        static void ShowInstruc()
        {
            Console.WriteLine("Uso:");
            Console.WriteLine(" mfd ctd|dupli|resu|info [-d=carpeta] [-c=conf] [-y] -zdesde=num [-zhasta=num]");
            Console.WriteLine("  --- ó ---");
            Console.WriteLine(" mfd ctd|dupli|resu|info [-d=carpeta] [-c=conf] [-y] -fdesde=ddmmaa [-fhasta=ddmmaa]");
            Console.WriteLine("");
            Console.WriteLine("\n\nParámetros:\n");
            Console.WriteLine("ctd|dupli|resu: Tipo de archivo a descargar:");
            Console.WriteLine("    ctd:     Cinta testigo digital.");
            Console.WriteLine("    dupli:   Duplicado de comprobantes tipo 'A' emitidos");
            Console.WriteLine("    resu:    Resúmen de ventas");
            Console.WriteLine("    info:    Muestra detalles del estado de descarga de los archivos");
            Console.WriteLine("-d=carpeta:  Opcional. Nombre de la carpeta donde se colocará la descarga.");
            Console.WriteLine("-c=conf:     Opcional. Configuración a usar. Solo se permite 'moretti' o 'epson'");
            Console.WriteLine("-y:          Intenta desencriptar el archivo PEM");
            Console.WriteLine("-zdesde:     Primer número de ZETA a descargar");
            Console.WriteLine("-zhasta:     Ultimo número de ZETA a descargar. Si no lo indica, usa -zdesde");
            Console.WriteLine("-fdesde=fecha: Fecha de la primer jornada a descargar. Usar DDMMAA");
            Console.WriteLine("-fhasta=fecha: Fecha de la última jornada a descargar. Si no se indica, usa -fdesde");
            Console.WriteLine("-force:      Omite validaciones de fechas. Util al hacer una baja fiscal.");
            Console.WriteLine("");
            Console.WriteLine("También puede invocar a 'mfd' sin parámetros, para que se abra la ventana de descarga.");
            Console.WriteLine("-fdesde/-fhasta son mutuamente exclusivas con -zdesde/-zhasta");

        }

        static bool ValidFecha(int fecha)
        {
            return (fecha > 0);
        }

        [STAThread]
        static void Main(string[] args)
        {
            int cmd = 0;
            int conf = 0;
            bool valid = true;
            bool borrar = false;
            string carpeta = null;
            bool decrypt = false;
            int fechaDesde = 0, fechaHasta = 0;
            int zetaDesde = 0, zetaHasta = 0;

            ShowLogo();
            if (args.Length == 1 && (args[0] == "-?" || args[0] == "/?"))
            {
                ShowInstruc();
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i].Trim().ToLower();
                if (arg == "ctd" || arg == "dupli" || arg == "resu" || arg == "info")
                {
                    if (cmd != 0)
                    {
                        Console.WriteLine("ERROR - Solo se permite uno de 'ctd' o 'dupli' o 'resu' o 'info'");
                        valid = false;
                        break;
                    }
                    cmd = (arg == "ctd") ? 1 : ((arg == "dupli") ? 2 : ((arg == "info")? 4: 3));
                }
                else if (arg == "-y")
                {
                    if (decrypt)
                    {
                        Console.WriteLine("ERROR - '-y' solo se puede indicar una vez");
                        valid = false;
                        break;
                    }
                    decrypt = true;
                }
                else if (arg.StartsWith("-c="))
                {
                    if (conf != 0)
                    {
                        Console.WriteLine("ERROR - La opción '-c=...' solo se permite una vez");
                        valid = false;
                        break;
                    }

                    if (arg == "-c=moretti")
                        conf = 1;
                    else if (arg == "-c=epson")
                        conf = 2;
                    else
                    {
                        Console.WriteLine("ERROR - '-c=...' invalido. Solo se acepta '-c=moretti' o '-c=epson'");
                        valid = false;
                        break;
                    }

                }
                else if (arg.StartsWith("-d="))
                {
                    if (carpeta != null)
                    {
                        Console.WriteLine("ERROR - '-d=...' solo se puede indicar una sola vez");
                        valid = false;
                        break;
                    }

                    carpeta = arg.Substring(3);
                    if (!Directory.Exists(carpeta))
                    {
                        Console.WriteLine($"ERROR - La carpeta '{carpeta}' no existe");
                        valid = false;
                        break;
                    }
                }
                else if (arg.StartsWith("-fdesde="))
                {
                    if (fechaDesde > 0)
                    {
                        Console.WriteLine("ERROR - '-fdesde' solo se puede indicar una vez");
                        valid = false;
                        break;
                    }
                    else if(zetaDesde > 0)
                    {
                        Console.WriteLine("ERROR - No puede usar '-fdesde' si ya us '-zdesde");
                        valid = false;
                        break;
                    }
                    if (!int.TryParse(arg.Substring(8), out fechaDesde) || !ValidFecha(fechaDesde))
                    {
                        Console.WriteLine("ERROR - El valor para '-fdesde=' no es valido. Use DDMMAA");
                        valid = false;
                        break;
                    }
                }
                else if (arg.StartsWith("-fhasta="))
                {
                    if (fechaHasta > 0)
                    {
                        Console.WriteLine("ERROR - '-fhasta' solo se puede indicar una vez");
                        valid = false;
                        break;
                    }
                    else if(fechaDesde <= 0)
                    {
                        Console.WriteLine("ERROR - No puede usar '-fhasta' si todavía no use '-fdesde'");
                        valid = false;
                        break;
                    }
                    else if (zetaDesde > 0)
                    {
                        Console.WriteLine("ERROR - No puede usar '-fhasta' si ya uso '-zdesde");
                        valid = false;
                        break;
                    }
                    if (!int.TryParse(arg.Substring(8), out fechaHasta) || !ValidFecha(fechaHasta))
                    {
                        Console.WriteLine("ERROR - El valor para '-fhasta=' no es valido. Use DDMMAA");
                        valid = false;
                        break;
                    }
                }
                else if (arg.StartsWith("-zdesde="))
                {
                    if (zetaDesde > 0)
                    {
                        Console.WriteLine("ERROR - '-zdesde' solo se puede indicar una vez");
                        valid = false;
                        break;
                    }
                    else if (fechaDesde > 0)
                    {
                        Console.WriteLine("ERROR - No puede usar '-zdesde' si ya usó '-fdesde");
                        valid = false;
                        break;
                    }
                    if (!int.TryParse(arg.Substring(8), out zetaDesde))
                    {
                        Console.WriteLine("ERROR - El valor para '-zdesde=' no es valido.");
                        valid = false;
                        break;
                    }
                }
                else if (arg.StartsWith("-zhasta="))
                {
                    if (zetaHasta > 0)
                    {
                        Console.WriteLine("ERROR - '-zhasta' solo se puede indicar una vez");
                        valid = false;
                        break;
                    }
                    else if (zetaDesde <= 0)
                    {
                        Console.WriteLine("ERROR - No puede usar '-zhasta' si todavía no use '-zdesde'");
                        valid = false;
                        break;
                    }
                    else if (fechaDesde > 0)
                    {
                        Console.WriteLine("ERROR - No puede usar '-zhasta' si ya uso '-fdesde");
                        valid = false;
                        break;
                    }
                    if (!int.TryParse(arg.Substring(8), out zetaHasta))
                    {
                        Console.WriteLine("ERROR - El valor para '-zhasta=' no es valido.");
                        valid = false;
                        break;
                    }
                }
            }

            if (cmd == 0)
            {
                Console.WriteLine("ERROR - Debe indicar uno de 'ctd' o 'dupli' o 'resu' o 'info'");
                valid = false;
            }

            if (fechaDesde == 0 && zetaDesde == 0 && cmd < 4)
            {
                Console.WriteLine("ERROR - Debe indicar la opción '-fdesde=...' o sino '-zdesde=...'");
                valid = false;
            }

            if (!valid)
            {
                Console.WriteLine();
                ShowInstruc();
            }
            else
            {
                if (conf == 0) conf = 1;
                if (carpeta == null) carpeta = ".\\";

                var fd = new FDownloader(cmd, conf, valid, borrar, carpeta, decrypt);
                if (cmd == 4)
                    fd.DoStatus();
                else if (zetaDesde > 0)
                    fd.DoWorkByZeta(zetaDesde, zetaHasta);
                else
                    fd.DoWorkByFecha(fechaDesde, fechaHasta);
            }

#if DEBUG
            Console.WriteLine("Listo. Pulsar una tecla para salir...");
            Console.ReadKey();
#endif
        }
    }
}
