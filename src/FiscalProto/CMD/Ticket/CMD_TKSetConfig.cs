// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TKSetConfig.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket
{
	public class MI_TKSetConfig : MInput
	{

		[Description("Descripción a usar cuando se necesite el 'Pago Automatico'."), Category("Datos")]
		public string DescPagoAutom { get; set; }

		[Description("Codigo a usar cuando se necesite el 'Pago Automatico'."), Category("Datos")]
		public CodMedioDePago CodPagoAutom { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime información sobe el total pagado y el vuelto\n" +
			"false: No, true: Si (default)")]
		public bool PrintInfoPago { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime lineas en blanco separando el encabezado y cola del cuerpo del ticket\n" +
			"false: No, true: Si (default)")]
		public bool AddBlank { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime la leyenda 'A CONSUMIDOR FINAL'\n" +
			"false: No, true: Si (default)")]
		public bool PrintACF { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime lineas de domicilio comercial\n" +
			"false: No, true: Si (default)")]
		public bool PrintDOM { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime lineas con el nombre de fantasía del comercio\n" +
			"false: No (default), true: Si")]
		public bool PrintFANT { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime la linea de 'Ingresos Brutos' en el encabezamiento\n" +
			"false: No (default), true: Si")]
		public bool PrintIB { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime las lineas de logo\n" +
			"false: No, true: Si (default)")]
		public bool PrintHeader { get; set; }

		[Category("Extension")]
		[Description(
			"Realiza pagos automáticos\n" +
			"false: No, true: Si (default)")]
		public bool UsaAutoPag { get; set; }

		[Category("Extension")]
		[Description(
			"Separa la linea de TOTAL con lineas en blanco antes y despues\n" +
			"false: No (default), true: Si")]
		public bool AddBlankTotal { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime el total de cantidades unitarias (bultos)\n" +
			"false: No (default), true: Si")]
		public bool PrintCantid { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime el codigo QR del contribuyente\n" +
			"false: No (default), true: Si")]
		public bool PrintQR { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(PrintInfoPago) extension |= 0x01;
				if(AddBlank) extension |= 0x04;
				if(PrintACF) extension |= 0x10;
				if(PrintDOM) extension |= 0x40;
				if(PrintFANT) extension |= 0x80;
				if(PrintIB) extension |= 0x100;
				if(PrintHeader) extension |= 0x400;
				if(UsaAutoPag) extension |= 0x800;
				if(AddBlankTotal) extension |= 0x1000;
				if(PrintCantid) extension |= 0x2000;
				if(PrintQR) extension |= 0x4000;
				SetOpcode(0x0A08, extension, list);
				Append_RT(list, DescPagoAutom, -1, 0, true);
				Append_N(list, (int)CodPagoAutom, 2, 0, false);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TKSetConfig  : MOutput
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

	public class CMD_TKSetConfig 
	    : CMD_CommandBase<MI_TKSetConfig, MO_TKSetConfig>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TKSetConfig"; }}
		public override int Opcode { get { return 0x0A08; }}
		public override string Description { get { return "Configura las preferencias para TODOS los tickets (o notas de credito) que se emitan."; }}

		// Ctor
		public CMD_TKSetConfig()
		{
			Input = new MI_TKSetConfig();
			Output = new MO_TKSetConfig();
		}
	}
}
