// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_AbrirGaveta.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_AbrirGaveta_ClassInterface
	{
		bool Gaveta { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_AbrirGaveta 
		: MInput
		, IMI_AbrirGaveta_ClassInterface
	{
		public bool Gaveta { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Gaveta) extension |= 0x01;
				SetOpcode(0x0707, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_AbrirGaveta_ClassInterface
	{

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_AbrirGaveta  
		: MOutput
		, IMO_AbrirGaveta_ClassInterface
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


	public interface ICMD_AbrirGaveta_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_AbrirGaveta_ClassInterface Input { get; }
		IMO_AbrirGaveta_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_AbrirGaveta 
	    : CMD_Generic
		, ICMD_AbrirGaveta_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "AbrirGaveta"; }}
		public override int Opcode { get { return 0x0707; }}
		public override string Description { get { return "Abre uno de los posibles cajones de dinero."; }}

		public IMI_AbrirGaveta_ClassInterface Input { get; protected set; }
		public IMO_AbrirGaveta_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_AbrirGaveta();
			Output = new MO_AbrirGaveta();
		}

		// Ctor
		public CMD_AbrirGaveta()
		{
			SetDefaults();
		}
	}

}
