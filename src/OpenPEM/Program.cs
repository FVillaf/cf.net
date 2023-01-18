using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenPEM
{
    /// <summary>
    /// Clase con el punto de entrada de la aplicación
    /// </summary>
    static class Program
    {
        #region Manejo de la consola
        const Int32 SW_HIDE = 0;
        const Int32 SW_SHOW = 5;

        // API de windows para obtener el handle de la consola.
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        // Api the windows para manejar el estado de visibilidad de una ventana.
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] Int32 nCmdShow);

        /// <summary>
        /// Muestra/oculta la ventana de consola.
        /// </summary>
        /// 
        /// <param name="show"><b>true</b> para mostrar la consola, <b>false</b> para ocultarla</param>
        public static void HandleConsoleWindow(bool show)
        {
            IntPtr hWndConsole = GetConsoleWindow();
            ShowWindow(hWndConsole, show? SW_SHOW : SW_HIDE);
        }

        #endregion

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // Si viene con argumentos, entonces ejecuta directamente la conversión sin pasar por la GUI
                Console.WriteLine($"OpenPEM Versión { PemConverter.Version }");
                Console.WriteLine("Copyright (c) 2021 - federvillaf@hotmail.com\n");
                Console.Out.Flush();
                if (args.Length > 2)
                    Console.WriteLine("Usar:\nOPENPEM <archivoPEM> [<archivoXML>]");
                else
                {
                    try
                    {
                        string source = args[0];
                        string target = (args.Length > 1) ?
                            args[1] :
                            PemConverter.DeriveTarget(source);
                        PemConverter.Convert2Xml(source, target);
                        Console.WriteLine($"Listo. Archivo '{ target }' generado correctamente");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: '{ ex.Message }'");
                    }
                }
            }
            else
            {
                // Usa la GUI para realizar la conversión
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
