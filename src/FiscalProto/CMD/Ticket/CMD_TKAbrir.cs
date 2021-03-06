// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TKAbrir.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket
{
	public class MI_TKAbrir : MInput
	{

		[Category("Extension")]
		[Description(
			"Indica si almacena o no las descripciones de los items.\n" +
			"false: No, true: Si")]
		public bool StoreDesc { get; set; }

		[Category("Extension")]
		[Description(
			"Si almacena descripciones, indica si tambien almacena o no los atributos de text0\n" +
			"false: No, true: Si")]
		public bool StoreAttr { get; set; }

		[Category("Extension")]
		[Description(
			"Indica si almacena o no las descripciones extras. La primera linea de las descripciones extras siempre se almacena.\n" +
			"false: No, true: Si")]
		public bool StoreExtra { get; set; }

		[Category("Extension")]
		[Description(
			"Indica si abre un ticket o una nota de credito\n" +
			"false: Ticket, true: NCred")]
		public bool TipoDoc { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(StoreDesc) extension |= 0x80;
				if(StoreAttr) extension |= 0x100;
				if(StoreExtra) extension |= 0x200;
				if(TipoDoc) extension |= 0x4000;
				SetOpcode(0x0A01, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_TKAbrir  : MOutput
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

	public class CMD_TKAbrir 
	    : CMD_CommandBase<MI_TKAbrir, MO_TKAbrir>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TKAbrir"; }}
		public override int Opcode { get { return 0x0A01; }}
		public override string Description { get { return "Abre un ticket o una nota de credito."; }}

		// Ctor
		public CMD_TKAbrir()
		{
			Input = new MI_TKAbrir();
			Output = new MO_TKAbrir();
		}
	}
}
