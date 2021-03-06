// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_ObtenerDensidadImpresion.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Impresora
{
	public class MI_ObtenerDensidadImpresion : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0778, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_ObtenerDensidadImpresion  : MOutput
	{

		[Description("Ignorado - Siempre se retornará '5' (100%)."), Category("Datos"), ReadOnly(true)]
		public int Densidad { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Densidad = Extract_N(data, 2, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_ObtenerDensidadImpresion 
	    : CMD_CommandBase<MI_ObtenerDensidadImpresion, MO_ObtenerDensidadImpresion>
	{
		public override CmdStatus Status {  get { return CmdStatus.Compatible; }}
		public override string Nombre { get { return "ObtenerDensidadImpresion"; }}
		public override int Opcode { get { return 0x0778; }}
		public override string Description { get { return "Obtiene la densidad de impresión configurada."; }}

		// Ctor
		public CMD_ObtenerDensidadImpresion()
		{
			Input = new MI_ObtenerDensidadImpresion();
			Output = new MO_ObtenerDensidadImpresion();
		}
	}
}
