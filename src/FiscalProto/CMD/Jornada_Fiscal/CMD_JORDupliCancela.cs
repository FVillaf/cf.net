// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_JORDupliCancela.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Jornada_Fiscal
{
	public class MI_JORDupliCancela : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x08F6, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_JORDupliCancela  : MOutput
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

	public class CMD_JORDupliCancela 
	    : CMD_CommandBase<MI_JORDupliCancela, MO_JORDupliCancela>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "JORDupliCancela"; }}
		public override int Opcode { get { return 0x08F6; }}
		public override string Description { get { return "Cancela la descarga del duplicado solicitado."; }}

		// Ctor
		public CMD_JORDupliCancela()
		{
			Input = new MI_JORDupliCancela();
			Output = new MO_JORDupliCancela();
		}
	}
}
