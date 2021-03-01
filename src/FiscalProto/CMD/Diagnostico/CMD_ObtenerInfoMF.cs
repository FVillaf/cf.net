// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_ObtenerInfoMF.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Diagnostico
{
	public class MI_ObtenerInfoMF : MInput
	{

		[Category("Extension")]
		[Description(
			"Imprime el resumen generado\n" +
			"false: No, true: Si")]
		public bool Print { get; set; }

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(Print) extension |= 0x01;
				SetOpcode(0x020B, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_ObtenerInfoMF  : MOutput
	{

		[Description("Cantidad de Zetas emitidas por el usuario actual."), Category("Datos"), ReadOnly(true)]
		public int ZetasEmitidas { get; set; }

		[Description("Cantidad total de Zetas emitidas."), Category("Datos"), ReadOnly(true)]
		public int ZetasEmitidasTodas { get; set; }

		[Description("Total de registros reservados para Zetas en la memoria fiscal."), Category("Datos"), ReadOnly(true)]
		public int ZetasReservadas { get; set; }

		[Description("Cantidad de descargas efectuadas."), Category("Datos"), ReadOnly(true)]
		public int Descargas { get; set; }

		[Description("Cantidad total de descargas de reportes efectuadas."), Category("Datos"), ReadOnly(true)]
		public int Descargas_Todas { get; set; }

		[Description("Total de registros reservados para descargas (por archivo) en la memoria fiscal."), Category("Datos"), ReadOnly(true)]
		public int DescargasReservadas { get; set; }

		[Description("Cantidad de altas fiscales efectuadas."), Category("Datos"), ReadOnly(true)]
		public int CantidadAltas { get; set; }

		[Description("Cantidad total de altas fiscales efectuadas."), Category("Datos"), ReadOnly(true)]
		public int CantidadBajas { get; set; }

		[Description("Total de registros reservados para altas (y bajas) en la memoria fiscal."), Category("Datos"), ReadOnly(true)]
		public int AltasReservadas { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					ZetasEmitidas = Extract_N(data, 8, false);
					ZetasEmitidasTodas = Extract_N(data, 8, false);
					ZetasReservadas = Extract_N(data, 8, false);
					Descargas = Extract_N(data, 8, false);
					Descargas_Todas = Extract_N(data, 8, false);
					DescargasReservadas = Extract_N(data, 8, false);
					CantidadAltas = Extract_N(data, 8, false);
					CantidadBajas = Extract_N(data, 8, false);
					AltasReservadas = Extract_N(data, 8, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_ObtenerInfoMF 
	    : CMD_CommandBase<MI_ObtenerInfoMF, MO_ObtenerInfoMF>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "ObtenerInfoMF"; }}
		public override int Opcode { get { return 0x020B; }}
		public override string Description { get { return "Obtiene información histórica respecto al uso de la memoria fiscal del equipo."; }}

		// Ctor
		public CMD_ObtenerInfoMF()
		{
			Input = new MI_ObtenerInfoMF();
			Output = new MO_ObtenerInfoMF();
		}
	}
}
