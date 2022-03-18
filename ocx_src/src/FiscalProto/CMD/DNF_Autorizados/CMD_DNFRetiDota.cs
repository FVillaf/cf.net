// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_DNFRetiDota.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_DNFRetiDota_ClassInterface
	{
		decimal Monto { get; set; }
		string LLibre1 { get; set; }
		string LLibre2 { get; set; }
		string LLibre3 { get; set; }
		string LLibre4 { get; set; }
		string LLibre5 { get; set; }
		string LLibre6 { get; set; }
		int ReempColaNum1 { get; set; }
		string ReempColaTexto1 { get; set; }
		int ReempColaNum2 { get; set; }
		string ReempColaTexto2 { get; set; }
		int ReempColaNum3 { get; set; }
		string ReempColaTexto3 { get; set; }
		bool Cut { get; set; }
		bool PrintDoc { get; set; }
		bool PrintFirma { get; set; }
		bool PrintAclara { get; set; }
		bool PrintTelef { get; set; }
		bool Tipo { get; set; }
		bool PrintHeader { get; set; }
		bool PrintEstab { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_DNFRetiDota 
		: MInput
		, IMI_DNFRetiDota_ClassInterface
	{
		public decimal Monto { get; set; }
		public string LLibre1 { get; set; }
		public string LLibre2 { get; set; }
		public string LLibre3 { get; set; }
		public string LLibre4 { get; set; }
		public string LLibre5 { get; set; }
		public string LLibre6 { get; set; }
		public int ReempColaNum1 { get; set; }
		public string ReempColaTexto1 { get; set; }
		public int ReempColaNum2 { get; set; }
		public string ReempColaTexto2 { get; set; }
		public int ReempColaNum3 { get; set; }
		public string ReempColaTexto3 { get; set; }
		public bool Cut { get; set; }
		public bool PrintDoc { get; set; }
		public bool PrintFirma { get; set; }
		public bool PrintAclara { get; set; }
		public bool PrintTelef { get; set; }
		public bool Tipo { get; set; }
		public bool PrintHeader { get; set; }
		public bool PrintEstab { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Cut) extension |= 0x01;
				if(PrintDoc) extension |= 0x10;
				if(PrintFirma) extension |= 0x20;
				if(PrintAclara) extension |= 0x40;
				if(PrintTelef) extension |= 0x80;
				if(Tipo) extension |= 0x100;
				if(PrintHeader) extension |= 0x400;
				if(PrintEstab) extension |= 0x800;
				SetOpcode(0x0F06, extension, list);
				Append_N(list, Monto, 9, 2, false);
				Append_RT(list, LLibre1, -1, 0, true);
				Append_RT(list, LLibre2, -1, 0, true);
				Append_RT(list, LLibre3, -1, 0, true);
				Append_RT(list, LLibre4, -1, 0, true);
				Append_RT(list, LLibre5, -1, 0, true);
				Append_RT(list, LLibre6, -1, 0, true);
				Append_N(list, ReempColaNum1, 3, 0, true);
				Append_RT(list, ReempColaTexto1, -1, 0, true);
				Append_N(list, ReempColaNum2, 3, 0, true);
				Append_RT(list, ReempColaTexto2, -1, 0, true);
				Append_N(list, ReempColaNum3, 3, 0, true);
				Append_RT(list, ReempColaTexto3, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_DNFRetiDota_ClassInterface
	{
		int Numero { get; set; }

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_DNFRetiDota  
		: MOutput
		, IMO_DNFRetiDota_ClassInterface
	{
		public int Numero { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Numero = Extract_N(data, 5, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_DNFRetiDota_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_DNFRetiDota_ClassInterface Input { get; }
		IMO_DNFRetiDota_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_DNFRetiDota 
	    : CMD_Generic
		, ICMD_DNFRetiDota_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "DNFRetiDota"; }}
		public override int Opcode { get { return 0x0F06; }}
		public override string Description { get { return "Realiza la impresión de un documento de retiro o ingreso de dinero."; }}

		public IMI_DNFRetiDota_ClassInterface Input { get; protected set; }
		public IMO_DNFRetiDota_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_DNFRetiDota();
			Output = new MO_DNFRetiDota();
		}

		// Ctor
		public CMD_DNFRetiDota()
		{
			SetDefaults();
		}
	}

}
