// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_ConfigRedLee.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Configuracion
{
	public class MI_ConfigRedLee : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x05C2, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_ConfigRedLee  : MOutput
	{

		[Description("Indica si se está o no utilizando DHCP."), Category("Datos"), ReadOnly(true)]
		public bool UsaDHCP { get; set; }

		[Description("Dirección IP."), Category("Datos"), ReadOnly(true)]
		public string IP { get; set; }

		[Description("Máscara de red."), Category("Datos"), ReadOnly(true)]
		public string Mask { get; set; }

		[Description("Puerta de enlace de red (Gateway)."), Category("Datos"), ReadOnly(true)]
		public string Gateway { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					UsaDHCP = Extract_Y(data, 1, false);
					IP = Extract_P(data, 19, true);
					Mask = Extract_P(data, 19, true);
					Gateway = Extract_P(data, 19, true);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_ConfigRedLee 
	    : CMD_CommandBase<MI_ConfigRedLee, MO_ConfigRedLee>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "ConfigRedLee"; }}
		public override int Opcode { get { return 0x05C2; }}
		public override string Description { get { return "Obtiene los parámetros configurados para iniciar una conexión de red."; }}

		// Ctor
		public CMD_ConfigRedLee()
		{
			Input = new MI_ConfigRedLee();
			Output = new MO_ConfigRedLee();
		}
	}
}
