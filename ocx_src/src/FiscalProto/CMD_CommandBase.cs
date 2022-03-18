using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace FiscalProto
{
    public abstract class CMD_Generic
    {
        PropertyInfo inputInfo, outputInfo;

        public abstract CmdStatus Status { get; }
        public abstract string Nombre { get; }
        public abstract string Description { get; }
        public abstract int Opcode { get; }
        public virtual bool Private {  get { return false; } }

        public object InputObject
        {
            get
            {
                if (inputInfo == null)
                {
                    var type = this.GetType();
                    inputInfo = type.GetProperty("Input");
                    outputInfo = type.GetProperty("Output");
                }
                return inputInfo.GetValue(this, null);
            }
        }

        public object OutputObject
        {
            get
            {
                if (outputInfo == null)
                {
                    var type = this.GetType();
                    inputInfo = type.GetProperty("Input");
                    outputInfo = type.GetProperty("Output");
                }
                return outputInfo.GetValue(this, null);
            }
        }

        public override string ToString()
        {
            string status = (Status == CmdStatus.Nada) ? "" :
                ((Status == CmdStatus.EnCurso) ? "[*]" : "[***]");

            return
                Opcode.ToString("X").PadLeft(4, '0') + " - " +
                Nombre +
                status;
        }
    }

    public enum CmdStatus {  Nada, EnCurso, Listo, Compatible }
}
