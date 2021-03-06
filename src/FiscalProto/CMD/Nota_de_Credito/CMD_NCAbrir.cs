// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_NCAbrir.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Nota_de_Credito
{
	public class MI_NCAbrir : MInput
	{

		[Description("Linea #1 con el nombre del cliente o comprador."), Category("Datos")]
		public string NomCliente_1 { get; set; }

		[Description("Linea #2 con el nombre del cliente o comprador."), Category("Datos")]
		public string NomCliente_2 { get; set; }

		[Description("Linea #1 con la dirección del cliente o comprador."), Category("Datos")]
		public string DirecCliente_1 { get; set; }

		[Description("Linea #2 con la dirección del cliente o comprador."), Category("Datos")]
		public string DirecCliente_2 { get; set; }

		[Description("Linea #3 con la dirección del cliente o comprador."), Category("Datos")]
		public string DirecCliente_3 { get; set; }

		[Description("Tipo de documento del cliente o comprador."), Category("Datos")]
		public TipoDocEnum TipoDoc { get; set; }

		[Description("Número de documento del cliente o comprador."), Category("Datos")]
		public string NroDoc { get; set; }

		[Description("Responsabilidad ante el IVA del cliente o comprador."), Category("Datos")]
		public TipoRespEnum RespIva { get; set; }

		[Description("Linea #1 del documento asociado (Remito)."), Category("Datos")]
		public string LineaDoc_1 { get; set; }

		[Description("Linea #2 del documento asociado (Remito)."), Category("Datos")]
		public string LineaDoc_2 { get; set; }

		[Description("Linea #3 del documento asociado (Remito)."), Category("Datos")]
		public string LineaDoc_3 { get; set; }

		[Description("Linea de 'Cheque de Reintegro para Turista."), Category("Datos")]
		public string LineaCheque { get; set; }

		[Category("Extension")]
		[Description(
			"Indica si imprime solo original u original y tripolicado\n" +
			"false: Original solo, true: Original y Triplicado")]
		public bool ImpreTriplicado { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(ImpreTriplicado) extension |= 0x02;
				SetOpcode(0x0D01, extension, list);
				Append_RT(list, NomCliente_1, -1, 0, true);
				Append_RT(list, NomCliente_2, -1, 0, true);
				Append_RT(list, DirecCliente_1, -1, 0, true);
				Append_RT(list, DirecCliente_2, -1, 0, true);
				Append_RT(list, DirecCliente_3, -1, 0, true);
				Append_L(list, ((char)TipoDoc).ToString(), 1, 0, false);
				Append_A(list, NroDoc, 20, 0, false);
				Append_L(list, ((char)RespIva).ToString(), 1, 0, false);
				Append_RT(list, LineaDoc_1, -1, 0, true);
				Append_RT(list, LineaDoc_2, -1, 0, true);
				Append_RT(list, LineaDoc_3, -1, 0, true);
				Append_RT(list, LineaCheque, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_NCAbrir  : MOutput
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

	public class CMD_NCAbrir 
	    : CMD_CommandBase<MI_NCAbrir, MO_NCAbrir>
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "NCAbrir"; }}
		public override int Opcode { get { return 0x0D01; }}
		public override string Description { get { return "Abre una nota de crédito."; }}

		// Ctor
		public CMD_NCAbrir()
		{
			Input = new MI_NCAbrir();
			Output = new MO_NCAbrir();
		}
	}
}
