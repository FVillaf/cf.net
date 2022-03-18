// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_DNFHGetConfig.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_DNFHGetConfig_ClassInterface
	{
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_DNFHGetConfig 
		: MInput
		, IMI_DNFHGetConfig_ClassInterface
	{
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x1009, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_DNFHGetConfig_ClassInterface
	{
		bool PrintInfoPago { get; set; }
		string Reservado1 { get; set; }
		bool AddBlank { get; set; }
		string Reservado2 { get; set; }
		string Reservado3 { get; set; }
		string Reservado4 { get; set; }
		string Reservado5 { get; set; }
		bool PrintFANT { get; set; }
		string Reservado6 { get; set; }
		string Reservado7 { get; set; }
		bool PrintHeader { get; set; }
		string Reservado { get; set; }
		bool AddBlankTotal { get; set; }
		string Reservado8 { get; set; }
		string Reservado9 { get; set; }
		string Reservado10 { get; set; }
		bool PrintQR { get; set; }

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_DNFHGetConfig  
		: MOutput
		, IMO_DNFHGetConfig_ClassInterface
	{
		public bool PrintInfoPago { get; set; }
		public string Reservado1 { get; set; }
		public bool AddBlank { get; set; }
		public string Reservado2 { get; set; }
		public string Reservado3 { get; set; }
		public string Reservado4 { get; set; }
		public string Reservado5 { get; set; }
		public bool PrintFANT { get; set; }
		public string Reservado6 { get; set; }
		public string Reservado7 { get; set; }
		public bool PrintHeader { get; set; }
		public string Reservado { get; set; }
		public bool AddBlankTotal { get; set; }
		public string Reservado8 { get; set; }
		public string Reservado9 { get; set; }
		public string Reservado10 { get; set; }
		public bool PrintQR { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					PrintInfoPago = Extract_Y(data, 1, false);
					Reservado1 = Extract_Reservado(data, 1, false);
					AddBlank = Extract_Y(data, 1, false);
					Reservado2 = Extract_Reservado(data, 1, false);
					Reservado3 = Extract_Reservado(data, 1, false);
					Reservado4 = Extract_Reservado(data, 1, false);
					Reservado5 = Extract_Reservado(data, 1, false);
					PrintFANT = Extract_Y(data, 1, false);
					Reservado6 = Extract_Reservado(data, 1, false);
					Reservado7 = Extract_Reservado(data, 1, false);
					PrintHeader = Extract_Y(data, 1, false);
					Reservado = Extract_Reservado(data, 1, false);
					AddBlankTotal = Extract_Y(data, 1, false);
					Reservado8 = Extract_Reservado(data, 1, false);
					Reservado9 = Extract_Reservado(data, 1, false);
					Reservado10 = Extract_Reservado(data, 1, false);
					PrintQR = Extract_Y(data, 1, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_DNFHGetConfig_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_DNFHGetConfig_ClassInterface Input { get; }
		IMO_DNFHGetConfig_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_DNFHGetConfig 
	    : CMD_Generic
		, ICMD_DNFHGetConfig_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "DNFHGetConfig"; }}
		public override int Opcode { get { return 0x1009; }}
		public override string Description { get { return "Obtiene la configuracion por default para todos los documentos no fiscales homologados que se emitan."; }}

		public IMI_DNFHGetConfig_ClassInterface Input { get; protected set; }
		public IMO_DNFHGetConfig_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_DNFHGetConfig();
			Output = new MO_DNFHGetConfig();
		}

		// Ctor
		public CMD_DNFHGetConfig()
		{
			SetDefaults();
		}
	}

}
