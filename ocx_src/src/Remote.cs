using System;
using System.IO.Ports;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace FiscalProto
{
    static class Remote
    {
        public static bool ValidatePort(string remote)
        {
            var parts = remote.Split('=');
            if (parts.Length != 2) return false;
            switch(parts[0].Trim().ToLower())
            {
                case "serial":
                    int speed;
                    var parts2 = parts[1].Split(':');
                    if (parts2.Length != 2)
                        return false;
                    if (!int.TryParse(parts2[1], out speed))
                        return false;
                    break;

                case "net":
                    IPAddress ip;
                    if (!IPAddress.TryParse(parts[1], out ip))
                        return false;
                    break;

                default:
                    return false;
            }
            return true;
        }

        public static VirtualPort CreatePort(string remote)
        {
            var parts = remote.Split('=');
            if (parts.Length == 2)
            {
                switch (parts[0].Trim().ToLower())
                {
                    case "serial":
                        int speed;
                        var parts2 = parts[1].Split(':');
                        if (
                            parts2.Length == 2 &&
                            int.TryParse(parts2[1], out speed))
                        {
                            return new RSerialPort(parts2[0], speed, Parity.None, 8, StopBits.One);
                        }
                        break;

                    case "net":
                        IPAddress ip;
                        if (IPAddress.TryParse(parts[1], out ip))
                            return new RNetworkPort(ip);
                        break;
                }
            }

            throw new ArgumentException("'Remoto' Invalido");
        }
    }
}
