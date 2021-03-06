// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_MensajeSistema.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.DNFH_Genericos
{
	public class MI_MensajeSistema : MInput
	{

		[Description("Texto opcional para la línea 1."), Category("Datos")]
		public string Linea1 { get; set; }

		[Description("Texto opcional para la línea 2."), Category("Datos")]
		public string Linea2 { get; set; }

		[Description("Texto opcional para la línea 3."), Category("Datos")]
		public string Linea3 { get; set; }

		[Description("Texto opcional para la línea 4."), Category("Datos")]
		public string Linea4 { get; set; }

		[Description("Texto opcional para la línea 5."), Category("Datos")]
		public string Linea5 { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0E50, extension, list);
				Append_RT(list, Linea1, -1, 0, true);
				Append_RT(list, Linea2, -1, 0, true);
				Append_RT(list, Linea3, -1, 0, true);
				Append_RT(list, Linea4, -1, 0, true);
				Append_RT(list, Linea5, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_MensajeSistema  : MOutput
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

	public class CMD_MensajeSistema 
	    : CMD_CommandBase<MI_MensajeSistema, MO_MensajeSistema>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "MensajeSistema"; }}
		public override int Opcode { get { return 0x0E50; }}
		public override string Description { get { return "Imprime un mensaje del sistema."; }}

		// Ctor
		public CMD_MensajeSistema()
		{
			Input = new MI_MensajeSistema();
			Output = new MO_MensajeSistema();
		}
	}
}
