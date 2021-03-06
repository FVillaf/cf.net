// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_BajaFiscal.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Inicializacion
{
	public class MI_BajaFiscal : MInput
	{

		[Category("Extension")]
		[Description(
			"Sin uso\n" +
			"false: No, true: Si")]
		public bool SinUso { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(SinUso) extension |= 0x01;
				SetOpcode(0x0410, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_BajaFiscal  : MOutput
	{

		[Description("Datos del ultimo SBF generado, para enviar a la AFIP."), Category("Datos"), ReadOnly(true)]
		public string SBF { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					SBF = Extract_P(data, 5500, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_BajaFiscal 
	    : CMD_CommandBase<MI_BajaFiscal, MO_BajaFiscal>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "BajaFiscal"; }}
		public override int Opcode { get { return 0x0410; }}
		public override string Description { get { return "Retorna los datos SBF, para la baja fiscal, y pasa al controlador a modo entrenamiento. Si el CF ya está en modo entrenamiento, vuelve a retornar los datos del SBF de la ultima baja."; }}

		// Ctor
		public CMD_BajaFiscal()
		{
			Input = new MI_BajaFiscal();
			Output = new MO_BajaFiscal();
		}
	}
}
