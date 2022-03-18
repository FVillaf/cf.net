// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_TFGetImpInt.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_TFGetImpInt_ClassInterface
	{
		bool Discriminar { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_TFGetImpInt 
		: MInput
		, IMI_TFGetImpInt_ClassInterface
	{
		public bool Discriminar { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Discriminar) extension |= 0x01;
				SetOpcode(0x0B0E, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_TFGetImpInt_ClassInterface
	{
		decimal TotalImpInt { get; set; }
		decimal TotalImpIntFijos { get; set; }
		decimal TotalImpIntPorc { get; set; }
		int TasaIVA_1 { get; set; }
		decimal Monto_1 { get; set; }
		decimal MontoImpIntFijos_1 { get; set; }
		decimal MontoImpIntPorcen_1 { get; set; }
		int TasaIVA_2 { get; set; }
		decimal Monto_2 { get; set; }
		decimal MontoImpIntFijos_2 { get; set; }
		decimal MontoImpIntPorcen_2 { get; set; }
		int TasaIVA_3 { get; set; }
		decimal Monto_3 { get; set; }
		decimal MontoImpIntFijos_3 { get; set; }
		decimal MontoImpIntPorcen_3 { get; set; }
		int TasaIVA_4 { get; set; }
		decimal Monto_4 { get; set; }
		decimal MontoImpIntFijos_4 { get; set; }
		decimal MontoImpIntPorcen_4 { get; set; }
		int TasaIVA_5 { get; set; }
		decimal Monto_5 { get; set; }
		decimal MontoImpIntFijos_5 { get; set; }
		decimal MontoImpIntPorcen_5 { get; set; }
		int TasaIVA_6 { get; set; }
		decimal Monto_6 { get; set; }
		decimal MontoImpIntFijos_6 { get; set; }
		decimal MontoImpIntPorcen_6 { get; set; }
		int TasaIVA_7 { get; set; }
		decimal Monto_7 { get; set; }
		decimal MontoImpIntFijos_7 { get; set; }
		decimal MontoImpIntPorcen_7 { get; set; }

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_TFGetImpInt  
		: MOutput
		, IMO_TFGetImpInt_ClassInterface
	{
		public decimal TotalImpInt { get; set; }
		public decimal TotalImpIntFijos { get; set; }
		public decimal TotalImpIntPorc { get; set; }
		public int TasaIVA_1 { get; set; }
		public decimal Monto_1 { get; set; }
		public decimal MontoImpIntFijos_1 { get; set; }
		public decimal MontoImpIntPorcen_1 { get; set; }
		public int TasaIVA_2 { get; set; }
		public decimal Monto_2 { get; set; }
		public decimal MontoImpIntFijos_2 { get; set; }
		public decimal MontoImpIntPorcen_2 { get; set; }
		public int TasaIVA_3 { get; set; }
		public decimal Monto_3 { get; set; }
		public decimal MontoImpIntFijos_3 { get; set; }
		public decimal MontoImpIntPorcen_3 { get; set; }
		public int TasaIVA_4 { get; set; }
		public decimal Monto_4 { get; set; }
		public decimal MontoImpIntFijos_4 { get; set; }
		public decimal MontoImpIntPorcen_4 { get; set; }
		public int TasaIVA_5 { get; set; }
		public decimal Monto_5 { get; set; }
		public decimal MontoImpIntFijos_5 { get; set; }
		public decimal MontoImpIntPorcen_5 { get; set; }
		public int TasaIVA_6 { get; set; }
		public decimal Monto_6 { get; set; }
		public decimal MontoImpIntFijos_6 { get; set; }
		public decimal MontoImpIntPorcen_6 { get; set; }
		public int TasaIVA_7 { get; set; }
		public decimal Monto_7 { get; set; }
		public decimal MontoImpIntFijos_7 { get; set; }
		public decimal MontoImpIntPorcen_7 { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					TotalImpInt = Extract_N(data, 10, 2, false);
					TotalImpIntFijos = Extract_N(data, 10, 2, false);
					TotalImpIntPorc = Extract_N(data, 10, 2, false);
					TasaIVA_1 = Extract_N(data, 4, true);
					Monto_1 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_1 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_1 = Extract_N(data, 10, 2, true);
					TasaIVA_2 = Extract_N(data, 4, true);
					Monto_2 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_2 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_2 = Extract_N(data, 10, 2, true);
					TasaIVA_3 = Extract_N(data, 4, true);
					Monto_3 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_3 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_3 = Extract_N(data, 10, 2, true);
					TasaIVA_4 = Extract_N(data, 4, true);
					Monto_4 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_4 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_4 = Extract_N(data, 10, 2, true);
					TasaIVA_5 = Extract_N(data, 4, true);
					Monto_5 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_5 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_5 = Extract_N(data, 10, 2, true);
					TasaIVA_6 = Extract_N(data, 4, true);
					Monto_6 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_6 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_6 = Extract_N(data, 10, 2, true);
					TasaIVA_7 = Extract_N(data, 4, true);
					Monto_7 = Extract_N(data, 10, 2, true);
					MontoImpIntFijos_7 = Extract_N(data, 10, 2, true);
					MontoImpIntPorcen_7 = Extract_N(data, 10, 2, true);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_TFGetImpInt_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_TFGetImpInt_ClassInterface Input { get; }
		IMO_TFGetImpInt_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_TFGetImpInt 
	    : CMD_Generic
		, ICMD_TFGetImpInt_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TFGetImpInt"; }}
		public override int Opcode { get { return 0x0B0E; }}
		public override string Description { get { return "Obtiene información detallada sobre los impuestos internos involucrados en la operación en curso."; }}

		public IMI_TFGetImpInt_ClassInterface Input { get; protected set; }
		public IMO_TFGetImpInt_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_TFGetImpInt();
			Output = new MO_TFGetImpInt();
		}

		// Ctor
		public CMD_TFGetImpInt()
		{
			SetDefaults();
		}
	}

}
