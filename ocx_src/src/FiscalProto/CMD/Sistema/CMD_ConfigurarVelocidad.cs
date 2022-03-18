// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_ConfigurarVelocidad.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_ConfigurarVelocidad_ClassInterface
	{
		CV_Velocidad Velocidad { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_ConfigurarVelocidad 
		: MInput
		, IMI_ConfigurarVelocidad_ClassInterface
	{
		public CV_Velocidad Velocidad { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				extension |= (((int) Velocidad) & 0x0007);
				SetOpcode(0x000A, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_ConfigurarVelocidad_ClassInterface
	{

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_ConfigurarVelocidad  
		: MOutput
		, IMO_ConfigurarVelocidad_ClassInterface
	{

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_ConfigurarVelocidad_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_ConfigurarVelocidad_ClassInterface Input { get; }
		IMO_ConfigurarVelocidad_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_ConfigurarVelocidad 
	    : CMD_Generic
		, ICMD_ConfigurarVelocidad_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "ConfigurarVelocidad"; }}
		public override int Opcode { get { return 0x000A; }}
		public override string Description { get { return "Configura la velocidad de comunicación del puerto de usuario (host port). IMPORTANTE: Debe apagar y encender el impresor despues de enviar este comando, para que el cambio se haga efectivo."; }}

		public IMI_ConfigurarVelocidad_ClassInterface Input { get; protected set; }
		public IMO_ConfigurarVelocidad_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_ConfigurarVelocidad();
			Output = new MO_ConfigurarVelocidad();
		}

		// Ctor
		public CMD_ConfigurarVelocidad()
		{
			SetDefaults();
		}
	}

	public enum CV_Velocidad 
	{
		B38400 = 0,	// 000
		B19200 = 1,	// 001
		B9600 = 2,	// 010
		B57600 = 3,	// 011
		B115200 = 4,	// 100
	};

}
