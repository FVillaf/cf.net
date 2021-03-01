using System;
using System.IO;
using System.IO.Ports;

using System.Drawing;

namespace FiscalProto
{
    /// <summary>
    /// Describe un controlador fiscal al que podemos conectarnos y maneja las posibles diferencias
    /// de implementación entre distintos controladores fiscales (por ejemplo, Genesis vs Kinder) o
    /// particularidades que introduzcan nuevas versiones.
    /// </summary>
    public class TargetBag
    {

#if !_CONSOLE
        /// <summary>
        /// Método fábrica usado para crear el descriptor.
        /// </summary>
        /// 
        /// <param name="code">El código de dispositivo.</param>
        /// 
        /// <returns>La descripción requerida.</returns>
        public static TargetBag FromCode(BagCode code)
        {
            switch (code)
            {
                case BagCode.Genesis:
                    return new TargetBag(code, "Moretti Genesis", Color.Crimson);

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// El color de background a usar para discriminar visualmente distintos dipsositivos.
        /// </summary>
        public Color FormColor { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="code">Código de controlador fiscal. Uno valor de <see cref="BagCode"/>.</param>
        /// <param name="nom">Nombre 'amigable' del controlador fiscal.</param>
        /// <param name="color">Color de fondo a usar en las pantallas.</param>
        private TargetBag(BagCode code, string nom, Color color)
        {
            this.Code = code;
            string fn = Code.ToString() + ".cfg";
            this.Name = nom;
            FormColor = color;
            if (File.Exists(fn))
                LoadFromFile(fn);
            else
            {
                Speed = SpeedCode.B9600;
                var ports = SerialPort.GetPortNames();
                PortName = (ports.Length > 0) ? ports[0] : "";
            }
        }

#else
        /// <summary>
        /// Constructor.
        /// </summary>
        /// 
        /// <param name="code">Código de controlador fiscal. Uno valor de <see cref="BagCode"/>.</param>
        /// <param name="nom">Nombre 'amigable' del controlador fiscal.</param>
        private TargetBag(BagCode code, string nom)
        {
            this.Code = code;
            string fn = Code.ToString() + ".cfg";
            this.Name = nom;
            if (File.Exists(fn))
                LoadFromFile(fn);
            else
            {
                Speed = SpeedCode.B9600;
                Port = Ports.COM1;
            }
        }
#endif

        /// <summary>
        /// Nombre del controlador fiscal.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Velocidad de comunicación actual (puerto serie)
        /// </summary>
        public SpeedCode Speed { get; set; }

        /// <summary>
        /// Código de controlador fiscal.
        /// </summary>
        public BagCode Code { get; private set; }

        /// <summary>
        /// Indica si se está usando Network en lugar de port serial.
        /// </summary>
        public bool UseNetwork { get; set; }

        /// <summary>
        /// Si usa network, la IP a la cual conectarse
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// El nombre del puerto.
        /// </summary>
        public string PortName
        {
            get; set;
        }

        /// <summary>
        /// Especificación completa del nombre del puerto.
        /// </summary>
        public string PortUNC
        {
            get
            {
                if (UseNetwork) return IPAddress;
                return $"{PortName}:{((int)Speed).ToString()}";
            }
        }

        /// <summary>
        /// Carga la configuración desde un archivo.
        /// </summary>
        /// 
        /// <param name="fn">PATH al archivo con la configuración.</param>
        private void LoadFromFile(string fn)
        {
            bool ok = false;
            try
            {
                string text = File.ReadAllText(fn);
                string[] parts = text.Split(':');
                if (parts.Length == 2 && parts[0].ToUpper().StartsWith("COM"))
                {
                    UseNetwork = false;
                    PortName = parts[0];
                    string[] p2 = parts[1].Split(',');
                    int speed = int.Parse(p2[0]);
                    Speed = (SpeedCode)speed;
                    ok = true;
                }
                else
                {
                    UseNetwork = true;
                    IPAddress = text.Trim();
                    ok = true;
                }
            }
            catch { }
            if (!ok)
            {
                UseNetwork = false;
                Speed = SpeedCode.B9600;
                PortName = SerialPort.GetPortNames()[0];
            }
        }

        /// <summary>
        /// Graba la configuración en un archivo.
        /// </summary>
        public void Save()
        {
            string fn = Code.ToString() + ".cfg";
            File.WriteAllText(fn, PortUNC);
        }

        /// <summary>
        /// Para consumir este objeto directamente.
        /// </summary>
        /// 
        /// <returns></returns>
        public override string ToString()
        {
            return Name + $" ({ PortUNC })";
        }
    }

    /// <summary>
    /// Código del dispositivo.
    /// </summary>
    public enum BagCode {  Genesis }

    /// <summary>
    /// Codigos de velocidad de comunicación serial.
    /// </summary>
    public enum SpeedCode
    {
        B9600 = 9600,
        B19200 = 19200,
        B38400 = 38400,
        B57600 = 57600,
        B115200 = 115200
    }
}
