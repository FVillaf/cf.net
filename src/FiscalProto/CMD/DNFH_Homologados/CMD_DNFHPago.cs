// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DNFHPago.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.DNFH_Homologados
{
	public class MI_DNFHPago : MInput
	{

		[Description("Descripcion extra #1."), Category("Datos")]
		public string PagoDescExtra1 { get; set; }

		[Description("Descripcion extra #2."), Category("Datos")]
		public string PagoDescExtra2 { get; set; }

		[Description("Cantidad de cuotas. No enviar este campo o enviarlo en 0 si no se usa."), Category("Datos")]
		public int Cuotas { get; set; }

		[Description("Detalle de otra forma de pago."), Category("Datos")]
		public string DetaAdic { get; set; }

		[Description("Detalle de los cupones."), Category("Datos")]
		public string DetaCupon { get; set; }

		[Description("Codigo de la forma de pago."), Category("Datos")]
		public CodMedioDePago Codigo { get; set; }

		[Description("Monto pagado."), Category("Datos")]
		public decimal Monto { get; set; }

		[Category("Extension")]
		[Description(
			"Indica como afecta el pago que se esta enviando, a la operacion en curso\n" +
			"false: Pago, true: Anula Pago")]
		public bool Tipo { get; set; }

		[Category("Extension")]
		[Description(
			"Indica si lo registrado en este medio de pago debe incluirse en los arqueos de fin de jornada\n" +
			"false: No, true: Si")]
		public bool Arqueo { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Tipo) extension |= 0x01;
				if(Arqueo) extension |= 0x02;
				SetOpcode(0x1005, extension, list);
				Append_RT(list, PagoDescExtra1, -1, 0, true);
				Append_RT(list, PagoDescExtra2, -1, 0, true);
				Append_N(list, Cuotas, 3, 0, true);
				Append_RT(list, DetaAdic, -1, 0, true);
				Append_RT(list, DetaCupon, -1, 0, true);
				Append_N(list, (int)Codigo, 2, 0, false);
				Append_N(list, Monto, 10, 2, false);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DNFHPago  : MOutput
	{

		[Description("Si corresponde, monto que todavia esta pendiente de pago."), Category("Datos"), ReadOnly(true)]
		public decimal Pendiente { get; set; }

		[Description("Si corresponde, monto del vuelto a entregar al cliente."), Category("Datos"), ReadOnly(true)]
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
					Pendiente = Extract_N(data, 10, 2, false);
					Vuelto = Extract_N(data, 10, 2, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_DNFHPago 
	    : CMD_CommandBase<MI_DNFHPago, MO_DNFHPago>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "DNFHPago"; }}
		public override int Opcode { get { return 0x1005; }}
		public override string Description { get { return "Aplica un pago (o cancelacion) al documento en curso."; }}

		// Ctor
		public CMD_DNFHPago()
		{
			Input = new MI_DNFHPago();
			Output = new MO_DNFHPago();
		}
	}
}
