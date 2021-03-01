// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_ConfigCola.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Configuracion
{
	public class MI_ConfigCola : MInput
	{

		[Description("Número de linea de cola a establecer."), Category("Datos")]
		public int Index { get; set; }

		[Description("Texto."), Category("Datos")]
		public string Texto { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x050A, extension, list);
				Append_N(list, Index, 3, 0, false);
				Append_RT(list, Texto, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_ConfigCola  : MOutput
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

	public class CMD_ConfigCola 
	    : CMD_CommandBase<MI_ConfigCola, MO_ConfigCola>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "ConfigCola"; }}
		public override int Opcode { get { return 0x050A; }}
		public override string Description { get { return "Configura las lineas de cola."; }}

		// Ctor
		public CMD_ConfigCola()
		{
			Input = new MI_ConfigCola();
			Output = new MO_ConfigCola();
		}
	}
}
