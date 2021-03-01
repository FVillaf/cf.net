// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DNL_Borra.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Descarga
{
	public class MI_DNL_Borra : MInput
	{

		[Description("Número de zeta para ubicar una semana, hasta la que se marcarán para borrar todos los archivos."), Category("Datos")]
		public int Zeta { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0910, extension, list);
				Append_N(list, Zeta, 4, 0, false);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DNL_Borra  : MOutput
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

	public class CMD_DNL_Borra 
	    : CMD_CommandBase<MI_DNL_Borra, MO_DNL_Borra>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "DNL_Borra"; }}
		public override int Opcode { get { return 0x0910; }}
		public override string Description { get { return "Borra los archivos ya descargados."; }}

		// Ctor
		public CMD_DNL_Borra()
		{
			Input = new MI_DNL_Borra();
			Output = new MO_DNL_Borra();
		}
	}
}
