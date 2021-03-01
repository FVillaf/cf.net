// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_JORGetInfoImpuestosJornada.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Jornada_Fiscal
{
	public class MI_JORGetInfoImpuestosJornada : MInput
	{

		[Description("Tipo de documento para el que se requiere la información."), Category("Datos")]
		public int TipoDoc { get; set; }

		[Category("Extension")]
		[Description(
			"Indica si las ventas se acumularán desde el último cierre de cajero (bit en 1) o desde el inicio de la jornada fiscal (bit en 0)\n" +
			"false: No, true: Si")]
		public bool SoloCajero { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(SoloCajero) extension |= 0x100;
				SetOpcode(0x080B, extension, list);
				Append_N(list, TipoDoc, 3, 0, false);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_JORGetInfoImpuestosJornada  : MOutput
	{

		[Description("Total de impuestos facturados."), Category("Datos"), ReadOnly(true)]
		public decimal TotalImpuestos { get; set; }

		[Description("Tasa de IVA facturado (1/7)."), Category("Datos"), ReadOnly(true)]
		public int TasaIVA_1 { get; set; }

		[Description("Monto de IVA facturado por la tasa de iva indicada antes (1/7)."), Category("Datos"), ReadOnly(true)]
		public decimal IVA_1 { get; set; }

		[Description("Monto de ventas (neto) facturado por la tasa de iva indicada antes (1/7)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto_1 { get; set; }

		[Description("Monto de impuestos internos (fijos) facturado por la tasa de iva indicada antes (1/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntFijos_1 { get; set; }

		[Description("Monto de impuestos internos (porcentuales) facturado por la tasa de iva indicada antes (1/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntPorcen_1 { get; set; }

		[Description("Tasa de IVA facturado (2/7)."), Category("Datos"), ReadOnly(true)]
		public int TasaIVA_2 { get; set; }

		[Description("Monto de IVA facturado por la tasa de iva indicada antes (2/7)."), Category("Datos"), ReadOnly(true)]
		public decimal IVA_2 { get; set; }

		[Description("Monto de ventas (neto) facturado por la tasa de iva indicada antes (2/7)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto_2 { get; set; }

		[Description("Monto de impuestos internos (fijos) facturado por la tasa de iva indicada antes (2/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntFijos_2 { get; set; }

		[Description("Monto de impuestos internos (porcentuales) facturado por la tasa de iva indicada antes (2/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntPorcen_2 { get; set; }

		[Description("Tasa de IVA facturado (3/7)."), Category("Datos"), ReadOnly(true)]
		public int TasaIVA_3 { get; set; }

		[Description("Monto de IVA facturado por la tasa de iva indicada antes (3/7)."), Category("Datos"), ReadOnly(true)]
		public decimal IVA_3 { get; set; }

		[Description("Monto de ventas (neto) facturado por la tasa de iva indicada antes (3/7)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto_3 { get; set; }

		[Description("Monto de impuestos internos (fijos) facturado por la tasa de iva indicada antes (3/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntFijos_3 { get; set; }

		[Description("Monto de impuestos internos (porcentuales) facturado por la tasa de iva indicada antes (3/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntPorcen_3 { get; set; }

		[Description("Tasa de IVA facturado (4/7)."), Category("Datos"), ReadOnly(true)]
		public int TasaIVA_4 { get; set; }

		[Description("Monto de IVA facturado por la tasa de iva indicada antes (4/7)."), Category("Datos"), ReadOnly(true)]
		public decimal IVA_4 { get; set; }

		[Description("Monto de ventas (neto) facturado por la tasa de iva indicada antes (4/7)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto_4 { get; set; }

		[Description("Monto de impuestos internos (fijos) facturado por la tasa de iva indicada antes (4/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntFijos_4 { get; set; }

		[Description("Monto de impuestos internos (porcentuales) facturado por la tasa de iva indicada antes (4/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntPorcen_4 { get; set; }

		[Description("Tasa de IVA facturado (5/7)."), Category("Datos"), ReadOnly(true)]
		public int TasaIVA_5 { get; set; }

		[Description("Monto de IVA facturado por la tasa de iva indicada antes (5/7)."), Category("Datos"), ReadOnly(true)]
		public decimal IVA_5 { get; set; }

		[Description("Monto de ventas (neto) facturado por la tasa de iva indicada antes (5/7)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto_5 { get; set; }

		[Description("Monto de impuestos internos (fijos) facturado por la tasa de iva indicada antes (5/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntFijos_5 { get; set; }

		[Description("Monto de impuestos internos (porcentuales) facturado por la tasa de iva indicada antes (5/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntPorcen_5 { get; set; }

		[Description("Tasa de IVA facturado (6/7)."), Category("Datos"), ReadOnly(true)]
		public int TasaIVA_6 { get; set; }

		[Description("Monto de IVA facturado por la tasa de iva indicada antes (6/7)."), Category("Datos"), ReadOnly(true)]
		public decimal IVA_6 { get; set; }

		[Description("Monto de ventas (neto) facturado por la tasa de iva indicada antes (6/7)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto_6 { get; set; }

		[Description("Monto de impuestos internos (fijos) facturado por la tasa de iva indicada antes (6/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntFijos_6 { get; set; }

		[Description("Monto de impuestos internos (porcentuales) facturado por la tasa de iva indicada antes (6/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntPorcen_6 { get; set; }

		[Description("Tasa de IVA facturado (7/7)."), Category("Datos"), ReadOnly(true)]
		public int TasaIVA_7 { get; set; }

		[Description("Monto de IVA facturado por la tasa de iva indicada antes (7/7)."), Category("Datos"), ReadOnly(true)]
		public decimal IVA_7 { get; set; }

		[Description("Monto de ventas (neto) facturado por la tasa de iva indicada antes (7/7)."), Category("Datos"), ReadOnly(true)]
		public decimal Monto_7 { get; set; }

		[Description("Monto de impuestos internos (fijos) facturado por la tasa de iva indicada antes (7/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntFijos_7 { get; set; }

		[Description("Monto de impuestos internos (porcentuales) facturado por la tasa de iva indicada antes (7/7)."), Category("Datos"), ReadOnly(true)]
		public decimal MontoImpIntPorcen_7 { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					TotalImpuestos = Extract_N(data, 12, 2, false);
					TasaIVA_1 = Extract_N(data, 4, true);
					IVA_1 = Extract_N(data, 10, 2, true);
					Monto_1 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_1 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_1 = Extract_N(data, 10, 2, true);
					TasaIVA_2 = Extract_N(data, 4, true);
					IVA_2 = Extract_N(data, 10, 2, true);
					Monto_2 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_2 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_2 = Extract_N(data, 10, 2, true);
					TasaIVA_3 = Extract_N(data, 4, true);
					IVA_3 = Extract_N(data, 10, 2, true);
					Monto_3 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_3 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_3 = Extract_N(data, 10, 2, true);
					TasaIVA_4 = Extract_N(data, 4, true);
					IVA_4 = Extract_N(data, 10, 2, true);
					Monto_4 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_4 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_4 = Extract_N(data, 10, 2, true);
					TasaIVA_5 = Extract_N(data, 4, true);
					IVA_5 = Extract_N(data, 10, 2, true);
					Monto_5 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_5 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_5 = Extract_N(data, 10, 2, true);
					TasaIVA_6 = Extract_N(data, 4, true);
					IVA_6 = Extract_N(data, 10, 2, true);
					Monto_6 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_6 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_6 = Extract_N(data, 10, 2, true);
					TasaIVA_7 = Extract_N(data, 4, true);
					IVA_7 = Extract_N(data, 10, 2, true);
					Monto_7 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_7 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_7 = Extract_N(data, 10, 2, true);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_JORGetInfoImpuestosJornada 
	    : CMD_CommandBase<MI_JORGetInfoImpuestosJornada, MO_JORGetInfoImpuestosJornada>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "JORGetInfoImpuestosJornada"; }}
		public override int Opcode { get { return 0x080B; }}
		public override string Description { get { return "Devuelve información electrónica de los impuestos facturados en la jornada fiscal en curso."; }}

		// Ctor
		public CMD_JORGetInfoImpuestosJornada()
		{
			Input = new MI_JORGetInfoImpuestosJornada();
			Output = new MO_JORGetInfoImpuestosJornada();
		}
	}
}
