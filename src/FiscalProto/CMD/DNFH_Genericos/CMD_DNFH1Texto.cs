// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DNFH1Texto.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.DNFH_Genericos
{
	public class MI_DNFH1Texto : MInput
	{

		[Description("Texto no fiscal a imprimir."), Category("Datos")]
		public string Texto { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0E02, extension, list);
				Append_RT(list, Texto, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DNFH1Texto  : MOutput
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

	public class CMD_DNFH1Texto 
	    : CMD_CommandBase<MI_DNFH1Texto, MO_DNFH1Texto>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "DNFH1Texto"; }}
		public override int Opcode { get { return 0x0E02; }}
		public override string Description { get { return "Imprime una línea de texto no fiscal."; }}

		// Ctor
		public CMD_DNFH1Texto()
		{
			Input = new MI_DNFH1Texto();
			Output = new MO_DNFH1Texto();
		}
	}
}
