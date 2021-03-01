// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DNL_Enumera.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Descarga
{
	public class MI_DNL_Enumera : MInput
	{

		[Category("Extension")]
		[Description(
			"Indica si codifica y firma el archivo como PKCS7\n" +
			"false: Si, true: No")]
		public bool PKCS7 { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(PKCS7) extension |= 0x10;
				SetOpcode(0x0970, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DNL_Enumera  : MOutput
	{

		[Description("Data solicitada."), Category("Datos"), ReadOnly(true)]
		public string Data { get; set; }

		[Description("Indica si hay o no mas datos a descargar."), Category("Datos"), ReadOnly(true)]
		public bool Continua { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Data = Extract_P(data, 4000, false);
					Continua = Extract_Y(data, 1, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_DNL_Enumera 
	    : CMD_CommandBase<MI_DNL_Enumera, MO_DNL_Enumera>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "DNL_Enumera"; }}
		public override int Opcode { get { return 0x0970; }}
		public override string Description { get { return "Continúa la descarga del archivo, iniciada con uno de los comandos 0951 o 0952."; }}

		// Ctor
		public CMD_DNL_Enumera()
		{
			Input = new MI_DNL_Enumera();
			Output = new MO_DNL_Enumera();
		}
	}
}
