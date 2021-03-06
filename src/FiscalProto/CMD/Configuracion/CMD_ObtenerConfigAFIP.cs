// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_ObtenerConfigAFIP.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Configuracion
{
	public class MI_ObtenerConfigAFIP : MInput
	{

		[Description("Indica el tipo de configuraración AFIP a obtener"), Category("Extension")]
		public OCAFIP_Linea Linea { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				extension |= (((int) Linea) & 0x0007);
				SetOpcode(0x0506, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_ObtenerConfigAFIP  : MOutput
	{

		[Description("Configuración solicitada. (TasaIVA=nn.nn, LimiteCF=nnnnnnnn.nn)."), Category("Datos"), ReadOnly(true)]
		public int Dato { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Dato = Extract_N(data, 10, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_ObtenerConfigAFIP 
	    : CMD_CommandBase<MI_ObtenerConfigAFIP, MO_ObtenerConfigAFIP>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "ObtenerConfigAFIP"; }}
		public override int Opcode { get { return 0x0506; }}
		public override string Description { get { return "Obtiene datos y límites establecidos por la AFIP."; }}

		// Ctor
		public CMD_ObtenerConfigAFIP()
		{
			Input = new MI_ObtenerConfigAFIP();
			Output = new MO_ObtenerConfigAFIP();
		}
	}

	public enum OCAFIP_Linea 
	{
		TasaIVADefault = 0,	// 000
		LimiteCF = 1,	// 001
	};
}
