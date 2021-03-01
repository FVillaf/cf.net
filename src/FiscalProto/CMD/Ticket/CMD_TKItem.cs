// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_TKItem.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Ticket
{
	public class MI_TKItem : MInput
	{

		[Description("Descripcion extra #1."), Category("Datos")]
		public string ItemDescExtra1 { get; set; }

		[Description("Descripcion extra #2."), Category("Datos")]
		public string ItemDescExtra2 { get; set; }

		[Description("Descripcion extra #3."), Category("Datos")]
		public string ItemDescExtra3 { get; set; }

		[Description("Descripcion extra #4."), Category("Datos")]
		public string ItemDescExtra4 { get; set; }

		[Description("Descripcion principal del item."), Category("Datos")]
		public string Descrip { get; set; }

		[Description("Cantidad vendida (4 decimales)."), Category("Datos")]
		public decimal Cantidad { get; set; }

		[Description("Precio Unitario (4 decimales)."), Category("Datos")]
		public decimal Unitario { get; set; }

		[Description("Tasa de IVA a usar (los 4 digitos se consideran decimales. Asi, 21% se codifica como 2100."), Category("Datos")]
		public int TasaIVA { get; set; }

		[Description("Impuestos internos fijos (por monto)."), Category("Datos")]
		public decimal ImpIntFijos { get; set; }

		[Description("Coeficiente de impuestos fijos porcentuales. 1.3%, por ejemplo, se codifica como 1300000."), Category("Datos")]
		public decimal ImpIntPorc { get; set; }

		[Description("Unidad de referencia, para el informe Matrix."), Category("Datos")]
		public int UnidadMTX { get; set; }

		[Description("Codigo de producto, para el informe Matrix."), Category("Datos")]
		public string CodigoMTX { get; set; }

		[Description("Codigo interno del producto (tal como lo usa la empresa)."), Category("Datos")]
		public string CodigoInt { get; set; }

		[Description("Codigo de unidad de medida. Ver tabla 'Unidades de Medida'."), Category("Datos")]
		public CodUnidadMedida CodigoMedida { get; set; }

		[Description("Codigo de condicion frente al IVA."), Category("Datos")]
		public CodTKItemCondIva CondIVA { get; set; }

		[Category("Extension")]
		[Description(
			"Envia o no el subtotal del ticket como respuesta de este comando.\n" +
			"false: No, true: Si")]
		public bool SendSubtot { get; set; }

		[Category("Extension")]
		[Description(
			"Marca o no el producto en el comprobante.\n" +
			"false: No, true: Si")]
		public bool Marcar { get; set; }

		[Category("Extension")]
		[Description(
			"Cuando la cantidad es el valor 1, use este bit para elgir si imprimir o no la linea de cantidad\n" +
			"false: No, true: Si")]
		public bool ImprCantidad { get; set; }

		[Description("Indica el tipo de item que se esta enviando"), Category("Extension")]
		public TKI_Tipo Tipo { get; set; }

		[Description("Indica como contabilizar este item en la cuenta total de items"), Category("Extension")]
		public TKI_XCant XCant { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(SendSubtot) extension |= 0x10;
				if(Marcar) extension |= 0x20;
				if(ImprCantidad) extension |= 0x100;
				extension |= (((int) Tipo) & 0x000F);
				extension |= ((((int) XCant) & 0x0003) << 6);
				SetOpcode(0x0A02, extension, list);
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

	public class MO_TKItem  : MOutput
	{

		[Description("Subtotal parcial del ticket. Debe activarse el bit 4 de la extension para que se envie este subtotal."), Category("Datos"), ReadOnly(true)]
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

	public class CMD_TKItem 
	    : CMD_CommandBase<MI_TKItem, MO_TKItem>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "TKItem"; }}
		public override int Opcode { get { return 0x0A02; }}
		public override string Description { get { return "Realiza la venta o devolucion (parcial o total) de un item, actualizando como corresponde los acumuladores de ventas e impuestos."; }}

		// Ctor
		public CMD_TKItem()
		{
			Input = new MI_TKItem();
			Output = new MO_TKItem();
		}
	}

	public enum TKI_Tipo 
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

	public enum TKI_XCant 
	{
		PorQ = 0,	// 00
		Por1 = 1,	// 01
		Por0 = 2,	// 10
	};
}
