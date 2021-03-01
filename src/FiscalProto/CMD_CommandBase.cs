using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiscalProto
{
    /// <summary>
    /// Clase abstracta usada para representar genéricamente cualquier comando que se pueda enviar
    /// al controlador fiscal.
    /// </summary>
    /// 
    /// <typeparam name="T1">El tipo concreto del bloque de entrada</typeparam>
    /// <typeparam name="T2">El tipo concreto del bloque de salida.</typeparam>
    public abstract class CMD_CommandBase<T1, T2>
        : CMD_Generic
        where T1: MInput 
        where T2 : MOutput
    {
        /// <summary>
        /// Bloque con los datos de entrada que se enviarán al controlador fiscal.
        /// </summary>
        public T1 Input { get; protected set; }

        /// <summary>
        /// Bloque con los datos de salida que se recibieron del controlador fiscal.
        /// </summary>
        public T2 Output { get; protected set; }

        /// <summary>
        /// Representa genéricamente a un comando.
        /// </summary>
        /// 
        /// <returns></returns>
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

    /// <summary>
    /// Clase base para todos los comandos.
    /// </summary>
    public abstract class CMD_Generic
    {
        /// <summary>
        /// Propiedades para aplicar reflexión a los bloques de entrada y salida y adaptarlos
        /// según su tipo.
        /// </summary>
        PropertyInfo inputInfo, outputInfo;

        /// <summary>
        /// El estado del comando. Uno de los posibles valores de <see cref="CmdStatus"/>.
        /// </summary>
        public abstract CmdStatus Status { get; }

        /// <summary>
        /// El nombre corto del comando.
        /// </summary>
        public abstract string Nombre { get; }

        /// <summary>
        /// Una descripción del comando.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// El opcode que le corresponde.
        /// </summary>
        public abstract int Opcode { get; }

        /// <summary>
        /// Indica si el comando se debe exponer o es solo para consumo de las utilidades
        /// de Moretti.
        /// </summary>
        public virtual bool Private {  get { return false; } }

        /// <summary>
        /// El bloque de entrada, como un objeto genérico.
        /// </summary>
        /// 
        /// <remarks>
        /// Extrae mediante reflexión la referencia al objeto que usa el comando.
        /// </remarks>
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

        /// <summary>
        /// el bloque de salida, como un objeto genérico.
        /// </summary>
        /// 
        /// <remarks>
        /// Extrae mediante flexión la referencia al objeto que usa el comando.</remarks>
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
    }

    /// <summary>
    /// Posibles estados del comando.
    /// </summary>
    public enum CmdStatus 
    {  
        // El comando no está implementado en el CF
        Nada, 
        
        // El comando está en curso de implementación en el CF
        EnCurso, 
        
        // El comando está listo para ser usado.
        Listo, 
        Compatible 
    }
}
