// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TFGetPago.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket_Factura
{
	public class MI_TFGetPago : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0B0C, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TFGetPago  : MOutput
	{

		[Description("Monto total pagado hasta el momento."), Category("Datos"), ReadOnly(true)]
		public decimal TotalPagado { get; set; }

		[Description("Total de la operación, que se debe pagar."), Category("Datos"), ReadOnly(true)]
		public decimal TotalTicket { get; set; }

		[Description("Si corresponde, vuelto a emitir."), Category("Datos"), ReadOnly(true)]
		public decimal Vuelto { get; set; }

		[Description("Cantidad de items de pagos recibidos."), Category("Datos"), ReadOnly(true)]
		public int CantPagos { get; set; }

		[Description("Cantidad Maxima de items de pagos recibidos."), Category("Datos"), ReadOnly(true)]
		public int CantPagosMax { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					TotalPagado = Extract_N(data, 10, 2, false);
					TotalTicket = Extract_N(data, 10, 2, false);
					Vuelto = Extract_N(data, 10, 2, false);
					CantPagos = Extract_N(data, 1, false);
					CantPagosMax = Extract_N(data, 1, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_TFGetPago 
	    : CMD_CommandBase<MI_TFGetPago, MO_TFGetPago>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TFGetPago"; }}
		public override int Opcode { get { return 0x0B0C; }}
		public override string Description { get { return "Obtiene información de pagos de la operación en curso."; }}

		// Ctor
		public CMD_TFGetPago()
		{
			Input = new MI_TFGetPago();
			Output = new MO_TFGetPago();
		}
	}
}
