using System;
using System.ComponentModel;

namespace FiscalProto
{
    public enum CodUnidadMedida
    {

        [Description("Sin Descripción")]
        nada = 0,
        Kilogramo = 1,
        Metros = 2,

        [Description("Metro cuadrado")]
        M2 = 3,

        [Description("Metro cubico")]
        M3 = 4,
        Litros = 5,
        Unidad = 7,
        Par = 8,
        Docena = 9,
        Quilate = 10,
        Millar = 11,

        [Description("Mega u. inter. act. antib.")]
        Mega = 12,

        [Description("Unidad int. act. inmung.")]
        UInterna = 13,
        Gramo = 14,
        Milimetro = 15,

        [Description("Milimetro cubico")]
        MM3 = 16,
        Kilometro = 17,
        Hectolitro = 18,
        Centimetro = 20,

        [Description("Kilogramo activo")]
        KGAct = 21,

        [Description("Gramo activo")]
        GRAct = 22,

        [Description("Gramo base")]
        GRBase = 23,
        Uiacthor = 24,

        [Description("Jgo. pqt. mazo naipes")]
        Juego = 25,
        Muiacthor = 26,

        [Description("Centimetro cubico")]
        CM3 = 27,
        Uiactant = 28,
        Tonelada = 29,

        [Description("Decametro cubico")]
        DCM3 = 30,

        [Description("Hectometro cubico")]
        HCM3 = 31,

        [Description("Kilometro cubico")]
        KM3 = 32,
        Microgramo = 33,
        Nanogramo = 34,
        Picogramo = 35,
        Muiactant = 36,
        Uiactig = 37,
        Miligramo = 41,
        Mililitro = 47,
        Curie = 48,
        Milicurie = 49,
        Microcurie = 50,

        [Description("U inter. act. hormonal")]
        UHormo = 51,

        [Description("Mega u. inter. act. hormonal")]
        MegaHormo = 52,

        [Description("Kilogramo base")]
        KGBase = 53,
        Gruesa = 54,
        Muiactig = 55,

        [Description("Kilogramo bruto")]
        KGBruto = 61,

        [Description("Pack 63 - Horma")]
        Horma = 62
    }

    public enum CodTKItemCondIva
    {
        Gravado = 7,
        Exento = 2,
        NoGravado = 1,
        NoCorresponde = 0
    }

    public enum CodMedioDePago
    {

        [Description("Carta de credito documentario")]
        Carta_de_credito_documentario = 1,

        [Description("Cartas de credito simple")]
        Cartas_de_credito_simple = 2,
        Cheque = 3,

        [Description("Cheques cancelatorios")]
        Cheques_cancelatorios = 4,

        [Description("Credito documentario")]
        Credito_documentario = 5,

        [Description("Cuenta corriente")]
        Cuenta_corriente = 6,
        Deposito = 7,
        Efectivo = 8,

        [Description("Endoso de cheque")]
        Endoso_de_cheque = 9,

        [Description("Factura de credito")]
        Factura_de_credito = 10,

        [Description("Garantias bancarias")]
        Garantias_bancarias = 11,
        Giros = 12,

        [Description("Letras de cambio")]
        Letras_de_cambio = 13,

        [Description("Medios de pago de comercio exterior")]
        Medios_de_pago_de_comercio_exterior = 14,

        [Description("Orden de pago documentaria")]
        Orden_de_pago_documentaria = 15,

        [Description("Orden de pago simple")]
        Orden_de_pago_simple = 16,

        [Description("Pago contra reembolso")]
        Pago_contra_reembolso = 17,

        [Description("Remesa documentaria")]
        Remesa_documentaria = 18,

        [Description("Remesa simple")]
        Remesa_simple = 19,

        [Description("Tarjeta de credito")]
        Tarjeta_de_credito = 20,

        [Description("Tarjeta de debito")]
        Tarjeta_de_debito = 21,
        Ticket = 22,

        [Description("Transferencia bancaria")]
        Transferencia_bancaria = 23,

        [Description("Transferencia no bancaria")]
        Transferencia_no_bancaria = 24,

        [Description("Otros medios de pago")]
        Otros_medios_de_pago = 99
    }

    public enum TipoDocEnum
    {
        DNI = 'D',
        CUIL = 'L',
        CUIT = 'T',
        Cedula = 'C',
        Pasaporte = 'P',

        [Description("Libreta Cívica")]
        LibrCivica = 'V',

        [Description("Libreta Enrolamiento")]
        LibrEnrolamiento = 'E'
    }

    public enum TipoRespEnum
    {
        Inscripto = 'I',

        [Description("No Inscripto")]
        No_Inscripto = 'N',
        Monotributo = 'M',
        Exento = 'E',

        [Description("No Categorizado")]
        No_Categorizado = 'U',

        [Description("ConsumidorFinal")]
        Consumidor_Final = 'F',

        [Description("Monotributo Social")]
        Monotributo_Social = 'T',

        [Description("Monotributo Promovido")]
        Monotributo_Promovido = 'P'
    }

    public partial class GenesisOCXObject
    {
        public CodUnidadMedida CodUnidadMedida;
    }
}
