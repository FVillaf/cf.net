// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TFSetConfig.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket_Factura
{
	public class MI_TFSetConfig : MInput
	{

		[Description("Descripción a usar cuando se necesite el 'Pago Automatico'."), Category("Datos")]
		public string DescPagoAutom { get; set; }

		[Description("Codigo a usar cuando se ncesite el 'Pago Automatico'."), Category("Datos")]
		public int CodPagoAutom { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime información sobe el total pagado y el vuelto\n" +
			"false: No, true: Si (default)")]
		public bool PrintInfoPago { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime lineas en blanco separando la cola del cuerpo de la operación\n" +
			"false: No, true: Si (default)")]
		public bool AddBlank { get; set; }

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
				if(PrintDOM) extension |= 0x40;
				if(PrintFANT) extension |= 0x80;
				if(PrintIB) extension |= 0x100;
				if(PrintHeader) extension |= 0x400;
				if(UsaAutoPag) extension |= 0x800;
				if(AddBlankTotal) extension |= 0x1000;
				if(PrintQR) extension |= 0x4000;
				SetOpcode(0x0B08, extension, list);
				Append_RT(list, DescPagoAutom, -1, 0, true);
				Append_N(list, CodPagoAutom, 2, 0, false);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TFSetConfig  : MOutput
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

	public class CMD_TFSetConfig 
	    : CMD_CommandBase<MI_TFSetConfig, MO_TFSetConfig>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TFSetConfig"; }}
		public override int Opcode { get { return 0x0B08; }}
		public override string Description { get { return "Configura las preferencias para TODOS los tickets factura (o notas de débito) que se emitan."; }}

		// Ctor
		public CMD_TFSetConfig()
		{
			Input = new MI_TFSetConfig();
			Output = new MO_TFSetConfig();
		}
	}
}
