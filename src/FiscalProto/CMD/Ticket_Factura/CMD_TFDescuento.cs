// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TFDescuento.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket_Factura
{
	public class MI_TFDescuento : MInput
	{

		[Description("Linea 1 con la descripcion del descuento o recargo."), Category("Datos")]
		public string Descrip { get; set; }

		[Description("Monto del descuento o recargo."), Category("Datos")]
		public decimal Monto { get; set; }

		[Description("Se ignora el contenido de este campo."), Category("Datos")]
		public int No_usar_1 { get; set; }

		[Description("Codigo interno al que se asignará el descuento o recargo."), Category("Datos")]
		public string CodigoInt { get; set; }

		[Description("Se ignora el contenido de este campo."), Category("Datos")]
		public CodTKItemCondIva No_usar_2 { get; set; }

		[Description("Linea 2 con la descripcion del descuento o recargo."), Category("Datos")]
		public string Descrip2 { get; set; }

		[Description("Linea 3 con la descripcion del descuento o recargo."), Category("Datos")]
		public string Descrip3 { get; set; }

		[Description("Indica el tipo de descuento a efectuar o aplicar al documento"), Category("Extension")]
		public TFD_Tipo Tipo { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				extension |= (((int) Tipo) & 0x0003);
				SetOpcode(0x0B04, extension, list);
				Append_RT(list, Descrip, -1, 0, false);
				Append_N(list, Monto, 10, 2, false);
				Append_N(list, No_usar_1, 4, 0, true);
				Append_A(list, CodigoInt, 50, 0, true);
				Append_N(list, (int)No_usar_2, 1, 0, true);
				Append_RT(list, Descrip2, -1, 0, true);
				Append_RT(list, Descrip3, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TFDescuento  : MOutput
	{

		[Description("Subtotal parcial del ticket (bruto)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Monto = Extract_N(data, 12, 4, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_TFDescuento 
	    : CMD_CommandBase<MI_TFDescuento, MO_TFDescuento>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TFDescuento"; }}
		public override int Opcode { get { return 0x0B04; }}
		public override string Description { get { return "Descuentos o ajustes globales al total de la operacion."; }}

		// Ctor
		public CMD_TFDescuento()
		{
			Input = new MI_TFDescuento();
			Output = new MO_TFDescuento();
		}
	}

	public enum TFD_Tipo 
	{
		Descuento = 0,	// 00
		Recargo = 1,	// 01
	};
}
