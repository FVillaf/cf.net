// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TKGetInfo.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket
{
	public class MI_TKGetInfo : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0A0A, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TKGetInfo  : MOutput
	{

		[Description("Numero de ticket (o Nota de Credito) que se está emitiendo."), Category("Datos"), ReadOnly(true)]
		public int NumTicket { get; set; }

		[Description("Total bruto de la operación."), Category("Datos"), ReadOnly(true)]
		public decimal TotBruto { get; set; }

		[Description("Total pagado hasta el momento."), Category("Datos"), ReadOnly(true)]
		public decimal TotPagado { get; set; }

		[Description("Total de IVA facturado hasta el momento."), Category("Datos"), ReadOnly(true)]
		public decimal TotIva { get; set; }

		[Description("Total de Impuestos Internos facturados hasta el momento."), Category("Datos"), ReadOnly(true)]
		public decimal TotImpInt { get; set; }

		[Description("Cantidad de items (comandos 0A02) emitidos hasta el momento."), Category("Datos"), ReadOnly(true)]
		public int CantItems { get; set; }

		[Description("Cantidad maxima de items que soporta una operación."), Category("Datos"), ReadOnly(true)]
		public int CantItemsMax { get; set; }

		[Description("Cantidad de descuentos (comandos 0A03) emitidos hasta el momento."), Category("Datos"), ReadOnly(true)]
		public int CantDesc { get; set; }

		[Description("Cantidad maxima de descuentos que soporta una operación."), Category("Datos"), ReadOnly(true)]
		public int CantDescMax { get; set; }

		[Description("Cantidad de tasas de impuestos usadas hasta el momento."), Category("Datos"), ReadOnly(true)]
		public int CantTax { get; set; }

		[Description("Cantidad maxima de tasas de impuetos que soporta una operación."), Category("Datos"), ReadOnly(true)]
		public int CantTaxMax { get; set; }

		[Description("Cantidad de pagos (comandos 0A04) emitidos hasta el momento."), Category("Datos"), ReadOnly(true)]
		public int CantPag { get; set; }

		[Description("Cantidad maxima de pagos que soporta una operación."), Category("Datos"), ReadOnly(true)]
		public int CantPagMax { get; set; }

		[Description("Fase actual de la operación en curso (0=Abierto sin items, 1=Venta, 2=Descuentos/Ajustes, 3=Pagos."), Category("Datos"), ReadOnly(true)]
		public int Fase { get; set; }

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
					TotBruto = Extract_N(data, 10, 2, false);
					TotPagado = Extract_N(data, 10, 2, false);
					TotIva = Extract_N(data, 10, 2, false);
					TotImpInt = Extract_N(data, 10, 2, false);
					CantItems = Extract_N(data, 4, false);
					CantItemsMax = Extract_N(data, 4, false);
					CantDesc = Extract_N(data, 2, false);
					CantDescMax = Extract_N(data, 2, false);
					CantTax = Extract_N(data, 2, false);
					CantTaxMax = Extract_N(data, 2, false);
					CantPag = Extract_N(data, 2, false);
					CantPagMax = Extract_N(data, 2, false);
					Fase = Extract_N(data, 2, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_TKGetInfo 
	    : CMD_CommandBase<MI_TKGetInfo, MO_TKGetInfo>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TKGetInfo"; }}
		public override int Opcode { get { return 0x0A0A; }}
		public override string Description { get { return "Obtiene información de la operación en curso."; }}

		// Ctor
		public CMD_TKGetInfo()
		{
			Input = new MI_TKGetInfo();
			Output = new MO_TKGetInfo();
		}
	}
}
