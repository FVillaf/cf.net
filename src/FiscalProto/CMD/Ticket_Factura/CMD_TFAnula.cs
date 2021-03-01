// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TFAnula.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket_Factura
{
	public class MI_TFAnula : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0B07, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TFAnula  : MOutput
	{

		[Description("Numero del ticket fiscal que se anulo."), Category("Datos"), ReadOnly(true)]
		public int NumTicket { get; set; }

		[Description("Indica el tipo de nota de crédito que se canceló (A/B/C/M)."), Category("Datos"), ReadOnly(true)]
		public string TipoTicket { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					NumTicket = Extract_N(data, 8, false);
					TipoTicket = Extract_L(data, 1, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_TFAnula 
	    : CMD_CommandBase<MI_TFAnula, MO_TFAnula>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TFAnula"; }}
		public override int Opcode { get { return 0x0B07; }}
		public override string Description { get { return "Anula el ticket factura o nota de débito en curso."; }}

		// Ctor
		public CMD_TFAnula()
		{
			Input = new MI_TFAnula();
			Output = new MO_TFAnula();
		}
	}
}
