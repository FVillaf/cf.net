// [auto-generated]
// This file was autogenerated by a tool.
// Please, don't edit it directly. Else, your changes could be lost next time the tool is executed.
// If you need to touch this file, remove the [auto-generated] key at top. Then, the tool will skip the regeneration of this file.
//
// Copyright (c) 2022 - Federico Villafañes (federvillaf@hotmail.com)
// http://www.moretti.com.ar y Federico Villafañes - All Rights Reserved

// CMD_DNFHAbrir.cs - Automatic processing of commands.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FiscalProto
{
	public interface IMI_DNFHAbrir_ClassInterface
	{
		int TipoDoc { get; set; }
		string NomCliente1 { get; set; }
		string NomCliente2 { get; set; }
		string DirecCliente1 { get; set; }
		string DirecCliente2 { get; set; }
		string DirecCliente3 { get; set; }
		TipoDocEnum TipoDocCli { get; set; }
		string NroDocCli { get; set; }
		TipoRespEnum RespIvaCli { get; set; }
		string LineaDoc1 { get; set; }
		string LineaDoc2 { get; set; }
		string LineaDoc3 { get; set; }
		string LineaCheque { get; set; }
		string RSTransportista1 { get; set; }
		string RSTransportista2 { get; set; }
		string DirecTransportista1 { get; set; }
		string DirecTransportista2 { get; set; }
		string DirecTransportista3 { get; set; }
		TipoDocEnum TipoDocTransp { get; set; }
		string NroDocTransp { get; set; }
		TipoRespEnum RespIvaTransp { get; set; }
		string NomChofer1 { get; set; }
		string NomChofer2 { get; set; }
		TipoDocEnum TipoDocChofer { get; set; }
		string NroDocChofer { get; set; }
		string ExtraInfo1 { get; set; }
		string ExtraInfo2 { get; set; }
		bool ImprimeTriplicado { get; set; }
		bool AlmacenaDesc { get; set; }
		bool ConservaAttrib { get; set; }
		bool SoloPrimeraDesc { get; set; }
		byte[] GetCommand();
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MI_DNFHAbrir 
		: MInput
		, IMI_DNFHAbrir_ClassInterface
	{
		public int TipoDoc { get; set; }
		public string NomCliente1 { get; set; }
		public string NomCliente2 { get; set; }
		public string DirecCliente1 { get; set; }
		public string DirecCliente2 { get; set; }
		public string DirecCliente3 { get; set; }
		public TipoDocEnum TipoDocCli { get; set; }
		public string NroDocCli { get; set; }
		public TipoRespEnum RespIvaCli { get; set; }
		public string LineaDoc1 { get; set; }
		public string LineaDoc2 { get; set; }
		public string LineaDoc3 { get; set; }
		public string LineaCheque { get; set; }
		public string RSTransportista1 { get; set; }
		public string RSTransportista2 { get; set; }
		public string DirecTransportista1 { get; set; }
		public string DirecTransportista2 { get; set; }
		public string DirecTransportista3 { get; set; }
		public TipoDocEnum TipoDocTransp { get; set; }
		public string NroDocTransp { get; set; }
		public TipoRespEnum RespIvaTransp { get; set; }
		public string NomChofer1 { get; set; }
		public string NomChofer2 { get; set; }
		public TipoDocEnum TipoDocChofer { get; set; }
		public string NroDocChofer { get; set; }
		public string ExtraInfo1 { get; set; }
		public string ExtraInfo2 { get; set; }
		public bool ImprimeTriplicado { get; set; }
		public bool AlmacenaDesc { get; set; }
		public bool ConservaAttrib { get; set; }
		public bool SoloPrimeraDesc { get; set; }
		public override byte[] GetCommand()
		{
			Error = string.Empty;
			var list = new List<byte>();
			try
			{
				int extension = 0;
				if(ImprimeTriplicado) extension |= 0x02;
				if(AlmacenaDesc) extension |= 0x80;
				if(ConservaAttrib) extension |= 0x100;
				if(SoloPrimeraDesc) extension |= 0x200;
				SetOpcode(0x1001, extension, list);
				Append_N(list, TipoDoc, 3, 0, false);
				Append_RT(list, NomCliente1, -1, 0, false);
				Append_RT(list, NomCliente2, -1, 0, true);
				Append_RT(list, DirecCliente1, -1, 0, true);
				Append_RT(list, DirecCliente2, -1, 0, true);
				Append_RT(list, DirecCliente3, -1, 0, true);
				Append_L(list, ((char)TipoDocCli).ToString(), 1, 0, false);
				Append_A(list, NroDocCli, 20, 0, false);
				Append_L(list, ((char)RespIvaCli).ToString(), 1, 0, false);
				Append_RT(list, LineaDoc1, -1, 0, true);
				Append_RT(list, LineaDoc2, -1, 0, true);
				Append_RT(list, LineaDoc3, -1, 0, true);
				Append_RT(list, LineaCheque, -1, 0, true);
				Append_RT(list, RSTransportista1, -1, 0, true);
				Append_RT(list, RSTransportista2, -1, 0, true);
				Append_RT(list, DirecTransportista1, -1, 0, true);
				Append_RT(list, DirecTransportista2, -1, 0, true);
				Append_RT(list, DirecTransportista3, -1, 0, true);
				Append_L(list, ((char)TipoDocTransp).ToString(), 1, 0, true);
				Append_A(list, NroDocTransp, 20, 0, true);
				Append_L(list, ((char)RespIvaTransp).ToString(), 1, 0, false);
				Append_RT(list, NomChofer1, -1, 0, true);
				Append_RT(list, NomChofer2, -1, 0, true);
				Append_L(list, ((char)TipoDocChofer).ToString(), 1, 0, true);
				Append_A(list, NroDocChofer, 20, 0, true);
				Append_RT(list, ExtraInfo1, -1, 0, true);
				Append_RT(list, ExtraInfo2, -1, 0, true);
			}
			catch(Exception ex) { Error = ex.Message; list.Clear(); }
			return list.ToArray();
		}
	}


	public interface IMO_DNFHAbrir_ClassInterface
	{
		int Numero { get; set; }

        string PrinterStatus { get; }
		string FiscalStatus { get; }
		int ErrorCodeInt { get; }

		void SetFromCommand(byte[] bindata);
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class MO_DNFHAbrir  
		: MOutput
		, IMO_DNFHAbrir_ClassInterface
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
					Numero = Extract_N(data, 10, false);
				}
			}
			catch(Exception ex) { Error = ex.Message; }
		}
	}


	public interface ICMD_DNFHAbrir_ClassInterface
	{
		CmdStatus Status {  get ; }
		string Nombre { get; }
		int Opcode { get; }
		string Description { get; }

		IMI_DNFHAbrir_ClassInterface Input { get; }
		IMO_DNFHAbrir_ClassInterface Output { get; }
	}

    [ClassInterface(ClassInterfaceType.None)]
	public class CMD_DNFHAbrir 
	    : CMD_Generic
		, ICMD_DNFHAbrir_ClassInterface
	{
		public override CmdStatus Status {  get { return CmdStatus.EnCurso; }}
		public override string Nombre { get { return "DNFHAbrir"; }}
		public override int Opcode { get { return 0x1001; }}
		public override string Description { get { return "Abre un documento no fiscal homologado."; }}

		public IMI_DNFHAbrir_ClassInterface Input { get; protected set; }
		public IMO_DNFHAbrir_ClassInterface Output { get; protected set; }

		public void SetDefaults()
		{
			Input = new MI_DNFHAbrir();
			Output = new MO_DNFHAbrir();
		}

		// Ctor
		public CMD_DNFHAbrir()
		{
			SetDefaults();
		}
	}

}
