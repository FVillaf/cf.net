// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2017 - Andres Moretti e Hijos S.A - Argentina
// http://www.moretti.com.ar - All Rights Reserved

// CMD_DNL_Info.cs - Automatic processing of commands.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace FiscalProto.Descarga
{
	public class MI_DNL_Info : MInput
	{

		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				SetOpcode(0x0915, extension, list);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}

	public class MO_DNL_Info  : MOutput
	{

		[Description("Indica la fecha desde la cual hay jornadas (CTD) sin descargar."), Category("Datos"), ReadOnly(true)]
		public int DescargaCTDDesde { get; set; }

		[Description("Indica la zeta desde la cual hay jornadas (CTD) sin descargar."), Category("Datos"), ReadOnly(true)]
		public int DescargaCTDDesdeZeta { get; set; }

		[Description("Indica la fecha desde la cual hay duplicados de comprobantes 'A' sin descargar."), Category("Datos"), ReadOnly(true)]
		public int DescargaDUPLIDesde { get; set; }

		[Description("Indica la zeta desde la cual hay duplicados de comprobantes 'A' sin descargar."), Category("Datos"), ReadOnly(true)]
		public int DescargaDUPLIDesdeZeta { get; set; }

		[Description("Indica la fecha desde la cual hay resúmenes de venta sin descargar."), Category("Datos"), ReadOnly(true)]
		public int DescargaRESUDesde { get; set; }

		[Description("Indica la zeta desde la cual hay resúmenes de venta sin descargar."), Category("Datos"), ReadOnly(true)]
		public int DescargaRESUDesdeZeta { get; set; }

		[Description("Indica la fecha (incluída) hasta la cuál están descargadas todos los archivos."), Category("Datos"), ReadOnly(true)]
		public int ListasHasta { get; set; }

		[Description("Indica la fecha (incluída) hasta la cuál están descargadas todos los archivos."), Category("Datos"), ReadOnly(true)]
		public int ListasHastaZeta { get; set; }

		[Description("Indica la fecha (incluída) hasta la cuál están borrados los archivos."), Category("Datos"), ReadOnly(true)]
		public int BorradasHasta { get; set; }

		[Description("Número de la ultima zeta emitida."), Category("Datos"), ReadOnly(true)]
		public int ZetaActual { get; set; }

		[Description("Cantidad de descargas disponibles."), Category("Datos"), ReadOnly(true)]
		public int DescDispo { get; set; }

		public override void SetFromCommand(byte[] bindata)
		{
			Error = string.Empty;
		    var data = new BinReader(bindata);
			try
			{
				ParseAnswerHeader(data);
                if(this.ErrorCodeInt == 0)
				{
					DescargaCTDDesde = Extract_N(data, 6, false);
					DescargaCTDDesdeZeta = Extract_N(data, 4, false);
					DescargaDUPLIDesde = Extract_N(data, 6, false);
					DescargaDUPLIDesdeZeta = Extract_N(data, 4, false);
					DescargaRESUDesde = Extract_N(data, 6, false);
					DescargaRESUDesdeZeta = Extract_N(data, 4, false);
					ListasHasta = Extract_N(data, 6, false);
					ListasHastaZeta = Extract_N(data, 6, false);
					BorradasHasta = Extract_N(data, 4, false);
					ZetaActual = Extract_N(data, 6, false);
					DescDispo = Extract_N(data, 6, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}

	public class CMD_DNL_Info 
	    : CMD_CommandBase<MI_DNL_Info, MO_DNL_Info>
	{
		public override CmdStatus Status {  get { return CmdStatus.Listo; }}
		public override string Nombre { get { return "DNL_Info"; }}
		public override int Opcode { get { return 0x0915; }}
		public override string Description { get { return "Información de estado de la memoria de transacciones."; }}

		// Ctor
		public CMD_DNL_Info()
		{
			Input = new MI_DNL_Info();
			Output = new MO_DNL_Info();
		}
	}
}
