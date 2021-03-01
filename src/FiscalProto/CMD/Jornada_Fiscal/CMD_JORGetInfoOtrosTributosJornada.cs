// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_JORGetInfoOtrosTributosJornada.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Jornada_Fiscal
{
	public class MI_JORGetInfoOtrosTributosJornada : MInput
	{

		[Description("Tipo de documento para el que se requiere la información."), Category("Datos")]
		public int TipoDoc { get; set; }

		[Description("Indica el número de otros tributos a reportar."), Category("Datos")]
		public int NumOtrosTrib { get; set; }

		[Category("Extension")]
		[Description(
			"Indica nsi la información debe o no ser discriminada por tasa de impuestos\n" +
			"false: No discriminar, true: Si discriminar")]
		public bool Discriminar { get; set; }

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
				if(Discriminar) extension |= 0x01;
				if(SoloCajero) extension |= 0x100;
				SetOpcode(0x080C, extension, list);
				Append_N(list, TipoDoc, 3, 0, false);
				Append_N(list, NumOtrosTrib, 2, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_JORGetInfoOtrosTributosJornada  : MOutput
	{

		[Description("Total de 'Otros Tributos'."), Category("Datos"), ReadOnly(true)]
		public decimal Total { get; set; }

		[Description("Total de 'Percepción Ingresos Brutos' facturado (código 07)."), Category("Datos"), ReadOnly(true)]
		public decimal TotPercepIngBrutos { get; set; }

		[Description("Total de 'Percepción IVA' facturado (código 06)."), Category("Datos"), ReadOnly(true)]
		public decimal TotPercepIVA { get; set; }

		[Description("Total de 'Otras Percepciones' facturadas (código 09)."), Category("Datos"), ReadOnly(true)]
		public decimal TotPercepOtras { get; set; }

		[Description("Cantidad de 'Otros Tributos' encontrados."), Category("Datos"), ReadOnly(true)]
		public int CantOtrosTributos { get; set; }

		[Description("Descripción del 'Otro Tributo' #n."), Category("Datos"), ReadOnly(true)]
		public string DescOtroTrbuto { get; set; }

		[Description("Monto facturado al 'Otro Tributo' #n."), Category("Datos"), ReadOnly(true)]
		public decimal Monto { get; set; }

		[Description("Tasa asociada al 'Otro Tributo' #n."), Category("Datos"), ReadOnly(true)]
		public int Tasa { get; set; }

		[Description("Código asociado al 'Otro Tributo' #n."), Category("Datos"), ReadOnly(true)]
		public int Codigo { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Total = Extract_N(data, 10, 2, false);
					TotPercepIngBrutos = Extract_N(data, 10, 2, false);
					TotPercepIVA = Extract_N(data, 10, 2, false);
					TotPercepOtras = Extract_N(data, 10, 2, false);
					CantOtrosTributos = Extract_N(data, 2, false);
					DescOtroTrbuto = Extract_RT(data, -1, true);
					Monto = Extract_N(data, 10, 2, true);
					Tasa = Extract_N(data, 4, true);
					Codigo = Extract_N(data, 2, true);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_JORGetInfoOtrosTributosJornada 
	    : CMD_CommandBase<MI_JORGetInfoOtrosTributosJornada, MO_JORGetInfoOtrosTributosJornada>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "JORGetInfoOtrosTributosJornada"; }}
		public override int Opcode { get { return 0x080C; }}
		public override string Description { get { return "Devuelve información electrónica de los 'Otros Tributos' facturados en la jornada fiscal en curso."; }}

		// Ctor
		public CMD_JORGetInfoOtrosTributosJornada()
		{
			Input = new MI_JORGetInfoOtrosTributosJornada();
			Output = new MO_JORGetInfoOtrosTributosJornada();
		}
	}
}
