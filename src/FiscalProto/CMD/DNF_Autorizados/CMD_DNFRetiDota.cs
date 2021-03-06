// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DNFRetiDota.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.DNF_Autorizados
{
	public class MI_DNFRetiDota : MInput
	{

		[Description("Monto a ingresar o retirar."), Category("Datos")]
		public decimal Monto { get; set; }

		[Description("Linea de datos libre número 1."), Category("Datos")]
		public string LLibre1 { get; set; }

		[Description("Linea de datos libre número 2."), Category("Datos")]
		public string LLibre2 { get; set; }

		[Description("Linea de datos libre número 3."), Category("Datos")]
		public string LLibre3 { get; set; }

		[Description("Linea de datos libre número 4."), Category("Datos")]
		public string LLibre4 { get; set; }

		[Description("Linea de datos libre número 5."), Category("Datos")]
		public string LLibre5 { get; set; }

		[Description("Linea de datos libre número 6."), Category("Datos")]
		public string LLibre6 { get; set; }

		[Description("Si se indica, número de línea de la cola a reemplazar #1."), Category("Datos")]
		public int ReempColaNum1 { get; set; }

		[Description("Si se indica, texto de línea de la cola a reemplazar #1."), Category("Datos")]
		public string ReempColaTexto1 { get; set; }

		[Description("Si se indica, número de línea de la cola a reemplazar #2."), Category("Datos")]
		public int ReempColaNum2 { get; set; }

		[Description("Si se indica, texto de línea de la cola a reemplazar #2."), Category("Datos")]
		public string ReempColaTexto2 { get; set; }

		[Description("Si se indica, número de línea de la cola a reemplazar #3."), Category("Datos")]
		public int ReempColaNum3 { get; set; }

		[Description("Si se indica, texto de línea de la cola a reemplazar #3."), Category("Datos")]
		public string ReempColaTexto3 { get; set; }

		[Category("Extension")]
		[Description(
			"Corta el papel después de éste documento.\n" +
			"false: No, true: Si")]
		public bool Cut { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime o no la línea de documento.\n" +
			"false: No, true: Si")]
		public bool PrintDoc { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime o no la línea de firma.\n" +
			"false: No, true: Si")]
		public bool PrintFirma { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime o no la línea de aclaración de firma.\n" +
			"false: No, true: Si")]
		public bool PrintAclara { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime o no la línea de teléfono.\n" +
			"false: No, true: Si")]
		public bool PrintTelef { get; set; }

		[Category("Extension")]
		[Description(
			"Tipo de operación.\n" +
			"false: Ingreso, true: Retiro")]
		public bool Tipo { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime las líneas de encabezamiento y colas.\n" +
			"false: No, true: Si")]
		public bool PrintHeader { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime las líneas de encabezamiento.\n" +
			"false: No, true: Si")]
		public bool PrintEstab { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Cut) extension |= 0x01;
				if(PrintDoc) extension |= 0x10;
				if(PrintFirma) extension |= 0x20;
				if(PrintAclara) extension |= 0x40;
				if(PrintTelef) extension |= 0x80;
				if(Tipo) extension |= 0x100;
				if(PrintHeader) extension |= 0x400;
				if(PrintEstab) extension |= 0x800;
				SetOpcode(0x0F06, extension, list);
				Append_N(list, Monto, 9, 2, false);
				Append_RT(list, LLibre1, -1, 0, true);
				Append_RT(list, LLibre2, -1, 0, true);
				Append_RT(list, LLibre3, -1, 0, true);
				Append_RT(list, LLibre4, -1, 0, true);
				Append_RT(list, LLibre5, -1, 0, true);
				Append_RT(list, LLibre6, -1, 0, true);
				Append_N(list, ReempColaNum1, 3, 0, true);
				Append_RT(list, ReempColaTexto1, -1, 0, true);
				Append_N(list, ReempColaNum2, 3, 0, true);
				Append_RT(list, ReempColaTexto2, -1, 0, true);
				Append_N(list, ReempColaNum3, 3, 0, true);
				Append_RT(list, ReempColaTexto3, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DNFRetiDota  : MOutput
	{

		[Description("Número de documento emitido."), Category("Datos"), ReadOnly(true)]
		public int Numero { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Numero = Extract_N(data, 5, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_DNFRetiDota 
	    : CMD_CommandBase<MI_DNFRetiDota, MO_DNFRetiDota>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "DNFRetiDota"; }}
		public override int Opcode { get { return 0x0F06; }}
		public override string Description { get { return "Realiza la impresión de un documento de retiro o ingreso de dinero."; }}

		// Ctor
		public CMD_DNFRetiDota()
		{
			Input = new MI_DNFRetiDota();
			Output = new MO_DNFRetiDota();
		}
	}
}
