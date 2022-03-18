// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_DNL_MemoriaFiscal.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_DNL_MemoriaFiscal_ClassInterface
	{
		int DireccionDesde { get; set; }
		int Cantidad { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_DNL_MemoriaFiscal 
		: MInput
		, IMI_DNL_MemoriaFiscal_ClassInterface
	{
		public int DireccionDesde { get; set; }
		public int Cantidad { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0954, extension, list);
				Append_N(list, DireccionDesde, 6, 0, false);
				Append_N(list, Cantidad, 6, 0, false);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_DNL_MemoriaFiscal_ClassInterface
	{
		string NomArchivo { get; set; }
		int Largo { get; set; }

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_DNL_MemoriaFiscal  
		: MOutput
		, IMO_DNL_MemoriaFiscal_ClassInterface
	{
		public string NomArchivo { get; set; }
		public int Largo { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					NomArchivo = Extract_P(data, -1, false);
					Largo = Extract_N(data, 10, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_DNL_MemoriaFiscal_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_DNL_MemoriaFiscal_ClassInterface Input { get; }
		IMO_DNL_MemoriaFiscal_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_DNL_MemoriaFiscal 
	    : CMD_Generic
		, ICMD_DNL_MemoriaFiscal_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "DNL_MemoriaFiscal"; }}
		public override int Opcode { get { return 0x0954; }}
		public override string Description { get { return "Inicia la descarga del contenido de la memoria fiscal."; }}

		public IMI_DNL_MemoriaFiscal_ClassInterface Input { get; protected set; }
		public IMO_DNL_MemoriaFiscal_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_DNL_MemoriaFiscal();
			Output = new MO_DNL_MemoriaFiscal();
		}

		// Ctor
		public CMD_DNL_MemoriaFiscal()
		{
			SetDefaults();
		}
	}

}
