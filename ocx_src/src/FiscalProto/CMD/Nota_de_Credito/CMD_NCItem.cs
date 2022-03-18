// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_NCItem.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_NCItem_ClassInterface
	{
		string ItemDescExtra1 { get; set; }
		string ItemDescExtra2 { get; set; }
		string ItemDescExtra3 { get; set; }
		string ItemDescExtra4 { get; set; }
		string Descrip { get; set; }
		decimal Cantidad { get; set; }
		decimal Unitario { get; set; }
		int TasaIVA { get; set; }
		decimal ImpIntFijos { get; set; }
		decimal ImpIntPorc { get; set; }
		int UnidadMTX { get; set; }
		string CodigoMTX { get; set; }
		string CodigoInt { get; set; }
		CodUnidadMedida CodigoMedida { get; set; }
		CodTKItemCondIva CondIVA { get; set; }
		bool SendSubtot { get; set; }
		bool ImprCantidad { get; set; }
		bool Leyenda { get; set; }
		bool PosiLeyenda { get; set; }
		bool ImportesBrutos { get; set; }
		NCI_Tipo Tipo { get; set; }
		NCI_XCant XCant { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_NCItem 
		: MInput
		, IMI_NCItem_ClassInterface
	{
		public string ItemDescExtra1 { get; set; }
		public string ItemDescExtra2 { get; set; }
		public string ItemDescExtra3 { get; set; }
		public string ItemDescExtra4 { get; set; }
		public string Descrip { get; set; }
		public decimal Cantidad { get; set; }
		public decimal Unitario { get; set; }
		public int TasaIVA { get; set; }
		public decimal ImpIntFijos { get; set; }
		public decimal ImpIntPorc { get; set; }
		public int UnidadMTX { get; set; }
		public string CodigoMTX { get; set; }
		public string CodigoInt { get; set; }
		public CodUnidadMedida CodigoMedida { get; set; }
		public CodTKItemCondIva CondIVA { get; set; }
		public bool SendSubtot { get; set; }
		public bool ImprCantidad { get; set; }
		public bool Leyenda { get; set; }
		public bool PosiLeyenda { get; set; }
		public bool ImportesBrutos { get; set; }
		public NCI_Tipo Tipo { get; set; }
		public NCI_XCant XCant { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(SendSubtot) extension |= 0x10;
				if(ImprCantidad) extension |= 0x100;
				if(Leyenda) extension |= 0x1000;
				if(PosiLeyenda) extension |= 0x2000;
				if(ImportesBrutos) extension |= 0x4000;
				extension |= (((int) Tipo) & 0x000F);
				extension |= ((((int) XCant) & 0x0003) << 6);
				SetOpcode(0x0D02, extension, list);
				Append_RT(list, ItemDescExtra1, -1, 0, true);
				Append_RT(list, ItemDescExtra2, -1, 0, true);
				Append_RT(list, ItemDescExtra3, -1, 0, true);
				Append_RT(list, ItemDescExtra4, -1, 0, true);
				Append_RT(list, Descrip, -1, 0, false);
				Append_N(list, Cantidad, 5, 4, false);
				Append_N(list, Unitario, 7, 4, false);
				Append_N(list, TasaIVA, 4, 0, false);
				Append_N(list, ImpIntFijos, 7, 4, true);
				Append_N(list, ImpIntPorc, 0, 8, true);
				Append_N(list, UnidadMTX, 6, 0, true);
				Append_A(list, CodigoMTX, 13, 0, true);
				Append_A(list, CodigoInt, 50, 0, true);
				Append_N(list, (int)CodigoMedida, 2, 0, false);
				Append_N(list, (int)CondIVA, 1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_NCItem_ClassInterface
	{
		decimal Subtotal { get; set; }

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_NCItem  
		: MOutput
		, IMO_NCItem_ClassInterface
	{
		public decimal Subtotal { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					Subtotal = Extract_N(data, 12, 4, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_NCItem_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_NCItem_ClassInterface Input { get; }
		IMO_NCItem_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_NCItem 
	    : CMD_Generic
		, ICMD_NCItem_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "NCItem"; }}
		public override int Opcode { get { return 0x0D02; }}
		public override string Description { get { return "Realiza la devolucion (parcial o total) de un item, actualizando como corresponde los acumuladores de ventas e impuestos."; }}

		public IMI_NCItem_ClassInterface Input { get; protected set; }
		public IMO_NCItem_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_NCItem();
			Output = new MO_NCItem();
		}

		// Ctor
		public CMD_NCItem()
		{
			SetDefaults();
		}
	}

	public enum NCI_Tipo 
	{
		Venta = 0,	// 0000
		AnulVenta = 1,	// 0001
		Bonif = 4,	// 0100
		AnulBonif = 5,	// 0101
		Descuento = 6,	// 0110
		AnulDescuento = 7,	// 0111
		Recargo = 8,	// 1000
		AnulRecargo = 9,	// 1001
	};

	public enum NCI_XCant 
	{
		PorQ = 0,	// 00
		Por1 = 1,	// 01
		Por0 = 2,	// 10
	};

}
