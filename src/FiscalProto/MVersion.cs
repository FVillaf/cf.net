using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiscalProto
{
    /// <summary>
    /// Versión de las utilidades
    /// </summary>
    public static class MVersion
    {
        static int 
            major = 1, 
            minor = 0, 
            compilation = 103;

        public static int Major => major;

        public static int Minor => minor;

        public static int Compilation => compilation;

        public static string Name => $" (Versión {major}.{minor}.{compilation})";
    }
}
