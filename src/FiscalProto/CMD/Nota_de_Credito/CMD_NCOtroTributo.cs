// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_NCOtroTributo.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Nota_de_Credito
{
	public class MI_NCOtroTributo : MInput
	{

		[Description("Descripción del tributo que se agrega a la operación en curso."), Category("Datos")]
		public string Descrip { get; set; }

		[Description("Monto neto del tributo."), Category("Datos")]
		public decimal Monto { get; set; }

		[Description("Tasa de IVA asociada."), Category("Datos")]
		public int TasaIVA { get; set; }

		[Category("Extension")]
		[Description(
			"Indica si la información de éste comando es para agregar un nuevo tributo, o para anular un tributo agregado con anterioridad\n" +
			"false: Agregar, true: Anular")]
		public bool Accion { get; set; }

		[Category("Extension")]
		[Description(
			"Devuelve el subtotal de la operación en la respuesta de éste comando\n" +
			"false: No, true: Si")]
		public bool RetSubtot { get; set; }

		[Description("Indica el tipo de tributo a agregar a la operación en curso"), Category("Extension")]
		public NCOT_Tipo Tipo { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Accion) extension |= 0x01;
				if(RetSubtot) extension |= 0x400;
				extension |= ((((int) Tipo) & 0x000F) << 6);
				SetOpcode(0x0D20, extension, list);
				Append_RT(list, Descrip, -1, 0, false);
				Append_N(list, Monto, 10, 2, false);
				Append_N(list, TasaIVA, 4, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_NCOtroTributo  : MOutput
	{

		[Description("Subtotal parcial (bruto) de la operación en curso."), Category("Datos"), ReadOnly(true)]
		public decimal Subtotal { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Subtotal = Extract_N(data, 10, 2, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_NCOtroTributo 
	    : CMD_CommandBase<MI_NCOtroTributo, MO_NCOtroTributo>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "NCOtroTributo"; }}
		public override int Opcode { get { return 0x0D20; }}
		public override string Description { get { return "Aplica un tributo adicional la operación en curso."; }}

		// Ctor
		public CMD_NCOtroTributo()
		{
			Input = new MI_NCOtroTributo();
			Output = new MO_NCOtroTributo();
		}
	}

	public enum NCOT_Tipo 
	{
		PercepIIBB = 0,	// 0000
		PercepMUNI = 1,	// 0001
		ImpNacional = 2,	// 0010
		ImpProvincial = 3,	// 0011
		PercepIVA = 4,	// 0100
		ImpMunicipal = 5,	// 0101
		ImpInterno = 6,	// 0110
		ImpIngBrutos = 7,	// 0111
		PercepOTRAS = 8,	// 1000
		Otros = 9,	// 1001
	};
}
