using System;
using System.Net;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiscalProto
{
    public class BatchArgs
    {
        string comPort = null;
        int comSpeed = 115200, comBits = 8;
        Parity comParity = Parity.None;
        StopBits comStopBits = StopBits.One;
        bool comSet = false;

        public string ErrorMessage { get; private set; } = null;

        public string ComPortName {  get { return comPort; } }
        public int ComSpeed { get { return comSpeed; } }
        public Parity ComParity {  get { return comParity; } }
        public int ComNumBits { get { return comBits; } }
        public StopBits CompStopBits {  get { return comStopBits; } }
        public bool UseSocket { get; set; }
        public IPAddress IP { get; set; }

        string ParityToString(Parity parity)
        {
            switch (parity)
            {
                case Parity.Even: return "e";
                case Parity.Mark: return "m";
                case Parity.Odd: return "o";
                case Parity.Space: return "s";
                default:
                    return "n";
            }
        }

        string StopBitsToString(StopBits stopBits)
        {
            switch (stopBits)
            {
                case StopBits.One: return "1";
                case StopBits.OnePointFive: return "1.5";
                case StopBits.Two: return "2";
                default:
                    return "n";
            }
        }

        public string CommPort
        {
            get { return $"{comPort}:{comSpeed},{ParityToString(comParity)},{comBits},{StopBitsToString(comStopBits)}"; }
            set
            {
                if (comSet)
                {
                    AddError("El argumento '-p' fue usado varias veces");
                    return;
                }
                comSet = true;

                value = value.Trim().ToLower();
                if (!value.StartsWith("com") || (value[4] != ':' && value[5] != ':'))
                    AddError("Falta (o es inválido) el nombre del port (com1:, com21:, etc...)");
                else
                {
                    var idx = value.IndexOf(':');
                    comPort = value.Substring(0, idx);
                    value = value.Substring(idx + 1);

                    idx = value.IndexOf(',');
                    if (idx < 0 || !int.TryParse(value.Substring(0, idx), out comSpeed))
                        AddError("Falta la velocidad");
                    else
                    {
                        if (comSpeed != 9600 && comSpeed != 19200 && comSpeed != 38400 && comSpeed != 57600 && comSpeed != 115200)
                            AddError("Velocidad invalida. Use 9600 o 19200 o 38400 o 57600 o 115200");
                        else
                        {
                            value = value.Substring(idx + 1);
                            idx = value.IndexOf(',');
                            if (idx != 1)
                                AddError("No indico la paridad a usar");
                            else
                            {
                                switch (value[0])
                                {
                                    case 'n': comParity = Parity.None; break;
                                    case 'e': comParity = Parity.Even; break;
                                    case 'o': comParity = Parity.Odd; break;
                                    case 'm': comParity = Parity.Mark; break;
                                    case 's': comParity = Parity.Space; break;
                                    default:
                                        AddError("Paridad invalida. Use 'n', 'e', 'o', 'm' o 's'");
                                        return;
                                }

                                value = value.Substring(idx + 1);
                                idx = value.IndexOf(',');
                                if (idx != 1 || !int.TryParse(value.Substring(0, idx), out comBits))
                                    AddError("No indico (o es inválido) el numero de bits a usar");
                                else
                                {
                                    switch (comBits)
                                    {
                                        case 7:
                                        case 8:
                                            break;

                                        default:
                                            AddError("Nro de bits inválido. Use 7 u 8");
                                            return;
                                    }
                                    value = value.Substring(idx + 1);
                                    switch (value)
                                    {
                                        case "1": comStopBits = StopBits.One; break;
                                        case "1.5":
                                        case "1,5": comStopBits = StopBits.OnePointFive; break;
                                        case "2": comStopBits = StopBits.Two; break;
                                        case "n": comStopBits = StopBits.None; break;
                                        default:
                                            AddError("Numero de bits de parada invalidos. Use 1, 1.5, 2, o n");
                                            return;

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AddError(string msg)
        {
            if (ErrorMessage == null)
                ErrorMessage = msg;
            else
                ErrorMessage += "\n" + msg;
        }

        string batchFile = null;

        public string BatchFile
        {
            get { return batchFile; }
            set
            {
                if (batchFile != null)
                    AddError("Se indicó mas de 1 batch a ejecutar");
                else
                {
                    value = Environment.ExpandEnvironmentVariables(value);
                    if (!File.Exists(value))
                        AddError($"El archivo '{ value }' no existe");
                    else
                        batchFile = value;
                }
            }
        }

        int repeats = 1;

        public string Repeats
        {
            get { return repeats.ToString(); }
            set
            {
                if (!int.TryParse(value, out repeats))
                    AddError("El valor de repeticiones indicado no es valido");
                if (repeats <= 0)
                    repeats = 1;
            }
        }

        bool verbose = false;

        public bool Verbose
        {
            get { return verbose; }
            set
            {
                if (verbose)
                    AddError("Ya se activo la opción '-v'");
                else
                    verbose = true;
            }
        }

        public int RepeatsInt { get { return repeats; } }

        bool writeLogs = false;

        public bool WriteLog
        {
            get { return writeLogs; }
            set
            {
                if (writeLogs)
                    AddError("La opción '-l' se indicó varias veces");
                else
                    writeLogs = value;
            }
        }

        public void FinalValidation()
        {
            if (ErrorMessage != null) return;
            if (batchFile == null)
                AddError("Falta indicar el archivo batch a ejecutar");

            if (UseSocket)
            {
                if (IP == null)
                    AddError("LA dirección IP no es válida");
            }
            else
            {
                if (comPort == null)
                    AddError("Falta indicar el puerto COM a usar");
            }
        }
    }
}
