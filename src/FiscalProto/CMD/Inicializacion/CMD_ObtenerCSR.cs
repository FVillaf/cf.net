// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_ObtenerCSR.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Inicializacion
{
	public class MI_ObtenerCSR : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0403, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_ObtenerCSR  : MOutput
	{

		[Description("Largo en bytes el CSR."), Category("Datos"), ReadOnly(true)]
		public int SIZE { get; set; }

		[Description("CSR: 'Certificate Signing Request'."), Category("Datos"), ReadOnly(true)]
		public string CSR { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					SIZE = Extract_N(data, 5, false);
					CSR = Extract_P(data, 5500, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_ObtenerCSR 
	    : CMD_CommandBase<MI_ObtenerCSR, MO_ObtenerCSR>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "ObtenerCSR"; }}
		public override int Opcode { get { return 0x0403; }}
		public override string Description { get { return "Obtiene el CSR del equipo, para actualizar el certificado."; }}

		// Ctor
		public CMD_ObtenerCSR()
		{
			Input = new MI_ObtenerCSR();
			Output = new MO_ObtenerCSR();
		}
	}
}
