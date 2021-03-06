// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DNFH1Abrir.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.DNFH_Genericos
{
	public class MI_DNFH1Abrir : MInput
	{

		[Category("Extension")]
		[Description(
			"Indica si el documento es genérico (910) o interno (950)\n" +
			"false: Generico, true: Interno")]
		public bool Uso { get; set; }

		[Category("Extension")]
		[Description(
			"Imprime o no encabezamientos\n" +
			"false: Si, true: No")]
		public bool Encabezamientos { get; set; }

		[Description("Indica la estación donce se imprimirá el documento."), Category("Extension")]
		public DNFHA_Estacion Estacion { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Uso) extension |= 0x40;
				if(Encabezamientos) extension |= 0x400;
				extension |= (((int) Estacion) & 0x0003);
				SetOpcode(0x0E01, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DNFH1Abrir  : MOutput
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

	public class CMD_DNFH1Abrir 
	    : CMD_CommandBase<MI_DNFH1Abrir, MO_DNFH1Abrir>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "DNFH1Abrir"; }}
		public override int Opcode { get { return 0x0E01; }}
		public override string Description { get { return "Abre documento no fiscal homologado genérico."; }}

		// Ctor
		public CMD_DNFH1Abrir()
		{
			Input = new MI_DNFH1Abrir();
			Output = new MO_DNFH1Abrir();
		}
	}

	public enum DNFHA_Estacion 
	{
		Rollo = 0,	// 00
		Slip = 1,	// 01
		RolloSlip = 2,	// 10
	};
}
