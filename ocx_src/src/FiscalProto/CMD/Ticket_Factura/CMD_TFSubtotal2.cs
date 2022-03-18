// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_TFSubtotal2.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_TFSubtotal2_ClassInterface
	{
		bool Print { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_TFSubtotal2 
		: MInput
		, IMI_TFSubtotal2_ClassInterface
	{
		public bool Print { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Print) extension |= 0x01;
				SetOpcode(0x0B23, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_TFSubtotal2_ClassInterface
	{
		decimal TotalBruto { get; set; }
		decimal TotalNeto { get; set; }
		decimal TotalIva { get; set; }
		decimal TotalImpInt { get; set; }
		decimal TotalOtrosImp { get; set; }
		int CantItems { get; set; }

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_TFSubtotal2  
		: MOutput
		, IMO_TFSubtotal2_ClassInterface
	{
		public decimal TotalBruto { get; set; }
		public decimal TotalNeto { get; set; }
		public decimal TotalIva { get; set; }
		public decimal TotalImpInt { get; set; }
		public decimal TotalOtrosImp { get; set; }
		public int CantItems { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					TotalBruto = Extract_N(data, 12, 2, false);
					TotalNeto = Extract_N(data, 12, 2, false);
					TotalIva = Extract_N(data, 12, 2, false);
					TotalImpInt = Extract_N(data, 12, 2, false);
					TotalOtrosImp = Extract_N(data, 12, 2, false);
					CantItems = Extract_N(data, 12, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_TFSubtotal2_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_TFSubtotal2_ClassInterface Input { get; }
		IMO_TFSubtotal2_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_TFSubtotal2 
	    : CMD_Generic
		, ICMD_TFSubtotal2_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TFSubtotal2"; }}
		public override int Opcode { get { return 0x0B23; }}
		public override string Description { get { return "Calcula los totales de la operación actual y lo imprime y/o devuelve en la respuesta."; }}

		public IMI_TFSubtotal2_ClassInterface Input { get; protected set; }
		public IMO_TFSubtotal2_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_TFSubtotal2();
			Output = new MO_TFSubtotal2();
		}

		// Ctor
		public CMD_TFSubtotal2()
		{
			SetDefaults();
		}
	}

}
