// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TFGetUltimoCierre.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket_Factura
{
	public class MI_TFGetUltimoCierre : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0B10, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TFGetUltimoCierre  : MOutput
	{

		[Description("Numero del ticket fiscal emitido."), Category("Datos"), ReadOnly(true)]
		public int NumTicket { get; set; }

		[Description("Tipo de operación (A/B/C/M) emitida."), Category("Datos"), ReadOnly(true)]
		public string Tipo { get; set; }

		[Description("Monto total de la operacion (bruto)."), Category("Datos"), ReadOnly(true)]
		public decimal Total { get; set; }

		[Description("Monto total de IVA facturado (o descontado, si es Nota de Credito)."), Category("Datos"), ReadOnly(true)]
		public decimal Iva { get; set; }

		[Description("Vuelto final."), Category("Datos"), ReadOnly(true)]
		public decimal Vuelto { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					NumTicket = Extract_N(data, 8, true);
					Tipo = Extract_L(data, 1, true);
					Total = Extract_N(data, 10, 2, true);
					Iva = Extract_N(data, 10, 2, true);
					Vuelto = Extract_N(data, 10, 2, true);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_TFGetUltimoCierre 
	    : CMD_CommandBase<MI_TFGetUltimoCierre, MO_TFGetUltimoCierre>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TFGetUltimoCierre"; }}
		public override int Opcode { get { return 0x0B10; }}
		public override string Description { get { return "Obtiene información sobre la respuesta enviada por el comando de cierre del último ticket fiscal o nota de débito cerrado exitosamente."; }}

		// Ctor
		public CMD_TFGetUltimoCierre()
		{
			Input = new MI_TFGetUltimoCierre();
			Output = new MO_TFGetUltimoCierre();
		}
	}
}
