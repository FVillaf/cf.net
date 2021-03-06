// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DBDelete.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Bases_de_Datos
{
	public class MI_DBDelete : MInput
	{

		[Description("La clave principal de la fila a eliminar."), Category("Datos")]
		public int Key { get; set; }

		[Description("Indica la tabla que se usara"), Category("Extension")]
		public DBD_Tabla Tabla { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				extension |= (((int) Tabla) & 0x0007);
				SetOpcode(0x9103, extension, list);
				Append_N(list, Key, 12, 0, false);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DBDelete  : MOutput
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

	public class CMD_DBDelete 
	    : CMD_CommandBase<MI_DBDelete, MO_DBDelete>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "DBDelete"; }}
		public override int Opcode { get { return 0x9103; }}
		public override string Description { get { return "Elimina un registro de la tabla que se indica."; }}

		// Ctor
		public CMD_DBDelete()
		{
			Input = new MI_DBDelete();
			Output = new MO_DBDelete();
		}
	}

	public enum DBD_Tabla 
	{
		Plu = 0,	// 000
		Clientes = 1,	// 001
		Cajeros = 2,	// 010
		MPagos = 3,	// 011
		Rubros = 4,	// 100
	};
}
