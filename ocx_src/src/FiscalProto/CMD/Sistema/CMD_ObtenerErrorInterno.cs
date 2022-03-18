// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_ObtenerErrorInterno.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_ObtenerErrorInterno_ClassInterface
	{
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_ObtenerErrorInterno 
		: MInput
		, IMI_ObtenerErrorInterno_ClassInterface
	{
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0004, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_ObtenerErrorInterno_ClassInterface
	{

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_ObtenerErrorInterno  
		: MOutput
		, IMO_ObtenerErrorInterno_ClassInterface
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


	public interface ICMD_ObtenerErrorInterno_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_ObtenerErrorInterno_ClassInterface Input { get; }
		IMO_ObtenerErrorInterno_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_ObtenerErrorInterno 
	    : CMD_Generic
		, ICMD_ObtenerErrorInterno_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.Compatible; }}
		public override string Nombre { get { return "ObtenerErrorInterno"; }}
		public override int Opcode { get { return 0x0004; }}
		public override string Description { get { return "Obtiene el código de error correspondiente, si se produjo un error en operaciones internas."; }}

		public IMI_ObtenerErrorInterno_ClassInterface Input { get; protected set; }
		public IMO_ObtenerErrorInterno_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_ObtenerErrorInterno();
			Output = new MO_ObtenerErrorInterno();
		}

		// Ctor
		public CMD_ObtenerErrorInterno()
		{
			SetDefaults();
		}
	}

}
