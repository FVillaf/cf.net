// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_JORDupliEnumera.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Jornada_Fiscal
{
	public class MI_JORDupliEnumera : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x08F4, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_JORDupliEnumera  : MOutput
	{

		[Description("Data solicitada."), Category("Datos"), ReadOnly(true)]
		public string Data { get; set; }

		[Description("Indica si hay o no mas datos a descargar."), Category("Datos"), ReadOnly(true)]
		public bool Continua { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Data = Extract_P(data, 4000, false);
					Continua = Extract_Y(data, 1, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_JORDupliEnumera 
	    : CMD_CommandBase<MI_JORDupliEnumera, MO_JORDupliEnumera>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "JORDupliEnumera"; }}
		public override int Opcode { get { return 0x08F4; }}
		public override string Description { get { return "Continúa la descarga de un documento cuya copia se solicitó con anterioridad."; }}

		// Ctor
		public CMD_JORDupliEnumera()
		{
			Input = new MI_JORDupliEnumera();
			Output = new MO_JORDupliEnumera();
		}
	}
}
