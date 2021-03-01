using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FiscalProto;
using FiscalProto.Ticket;
using FiscalProto.Ticket_Factura;
using FiscalProto.Nota_de_Credito;
using FiscalProto.Jornada_Fiscal;
using FiscalProto.DNFH_Genericos;

namespace Ejemplos
{
    /// <summary>
    /// Clase que se encarga de ejecutar los ejemplos.
    /// </summary>
    public class ERunner
    {
        ProtoHandler proto;
        Action<string> errorShow;

        /// <summary>
        /// Ejecuta un comando mientras verifica errores, etc.
        /// </summary>
        /// 
        /// <param name="cmd">El comando a ejecutar</param>
        /// <returns><b>true</b> si todo salió bien.</returns>
        bool ExecCommand(CMD_Generic cmd)
        {
            var mo = proto.ExchangePacket(cmd);
            if(mo == null)
            {
                errorShow(proto.LastError);
                return false;
            }
            if(mo.ErrorCodeInt != 0)
            {
                errorShow($"Error {mo.ErrorCode} - '{mo.Error}'");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        /// <param name="proto">
        /// El manejador del protocolo fiscal que se usará para enviar comandos al CF.
        /// </param>
        /// 
        /// <param name="errorShow">
        /// Método que se invocará cuando9 se produzca un error.
        /// </param>
        public ERunner(ProtoHandler proto, Action<string> errorShow)
        {
            this.proto = proto;
            this.errorShow = errorShow;
        }

        #region Monotributo/Exento/Otros (Operaciones 'B')

        /// <summary>
        /// Ejemplo de nota de crédito B, emitida a un responsable monotributo.
        /// </summary>
        /// 
        /// <remarks>
        /// Solo se demuestra la impresión de un ítem, pero todas las funciones demostradas
        /// en el "Ticket-Factura B" también están disponibles para la emisión de Notas de
        /// Crédito.
        /// 
        /// Solo debe tener cuidado de usar el comando que corresponda. Por ejemplo, el descuento
        /// global se demuestra en el ticket factura usando el comando 'CMD_TFDescuento' pero, si
        /// quisiera usarlo en nota de credito, el comando sería 'CMD_NCDescuento'
        /// </remarks>
        public void NotaCreditoB()
        {
            // Abre la operación. 
            var cmdO = new CMD_NCAbrir();
            cmdO.Input.NomCliente_1 = "Martin Guzman";
            cmdO.Input.DirecCliente_1 = "Casa Rosada - CABA";
            cmdO.Input.RespIva = TipoRespEnum.Monotributo;
            cmdO.Input.TipoDoc = TipoDocEnum.CUIT;
            cmdO.Input.NroDoc = "20168993278";
            cmdO.Input.LineaDoc_1 = "083-0001-00000001";
            if (!ExecCommand(cmdO))
                return;

            // Envía un ítem de $12.50 (10 x $1.25)
            // Prestar atención a la opción 'ImportesBrutos'. Al estar activa significa que el precio
            // unitario ($1.25, en este ejemplo), incluye impuestos. Se imprimirá discriminado, como
            // corresponde, pero el total a cobrar será 12.50.
            var cmdI = new CMD_NCItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.Unitario = 1.25m;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;                                  // 2 decimales por default.
            cmdI.Input.CodigoMedida = CodUnidadMedida.Litros;
            cmdI.Input.Descrip = "Aceite Patito PVC";
            cmdI.Input.ItemDescExtra1 = "Inmejorable para milanesas!";
            ExecCommand(cmdI);

            // Cierra la nota de debito.
            var cmdC = new CMD_NCCerrar();
            ExecCommand(cmdC);
        }
        /// <summary>
        /// Ejemplo de nota de débito B, emitida a un responsable monotributista
        /// </summary>
        /// 
        /// <remarks>
        /// Solo se demuestra la impresión de un ítem, pero todas las funciones demostradas
        /// en el "Ticket-Factura A" también están disponibles para la emisión de Notas de
        /// Débito.
        /// </remarks>
        public void NotaDebitoB()
        {
            // Abre la operación. 
            // IMPORTANTE: Solo se establecen los datos mínimos necesarios para poder abrir el
            // ticket factura.
            //
            // Inspeccione la estructura cmdO.Input por otros datos que son opcionales, por ejemplo,
            // mas campos de dirección del cliente, etc.
            //
            // Respecto del 'TipoDeDocumento', el DNI/etc solo se permiten para consumidores finales.
            // El resto de los responsable deben presentar CUIT (o para el exento, CUIL)
            var cmdO = new CMD_TFAbrir();
            cmdO.Input.Tipo = true;                         // Indica abrir NOTA DE DEBITO
            cmdO.Input.NomCliente_1 = "Martin Guzman";
            cmdO.Input.DirecCliente_1 = "Casa Rosada - CABA";
            cmdO.Input.RespIva = TipoRespEnum.Monotributo;
            cmdO.Input.TipoDoc = TipoDocEnum.CUIT;
            cmdO.Input.NroDoc = "20168993278";
            cmdO.Input.LineaDoc_1 = "083-0001-00000001";
            if (!ExecCommand(cmdO))
                return;

            // Envía un ítem de $12.50 (10 x $1.25)
            // Prestar atención a la opción 'ImportesBrutos'. Al estar activa significa que el precio
            // unitario ($1.25, en este ejemplo), incluye impuestos. Se imprimirá discriminado, como
            // corresponde, pero el total a cobrar será 12.50.
            var cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.Unitario = 1.25m;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;                                  // 2 decimales por default.
            cmdI.Input.CodigoMedida = CodUnidadMedida.Litros;
            cmdI.Input.Descrip = "Aceite Patito PVC";
            cmdI.Input.ItemDescExtra1 = "Inmejorable para milanesas!";
            ExecCommand(cmdI);

            // Cierra la nota de debito.
            var cmdC = new CMD_TFCerrar();
            ExecCommand(cmdC);
        }

        /// <summary>
        /// Ejemplo de ticket-factura "B", emitido a un responsable inscripto.
        /// </summary>
        /// 
        /// <remarks>
        /// Se demuestran la mayor parte de las operaciones posibles:
        /// - Ventas/Anulaciones a distintos IVAS y con distintos impuestos internos.
        /// - Descuentos/Recargos/Bonificaciones por ítem.
        /// - Descuentos/Recargos globales
        /// - Otros impuestos
        /// - Ingreso de medios de cobro.
        /// - Cierre de ticket con manipulación de las líneas de pie.
        /// </remarks>
        public void TicketFacturaB()
        {
            // Abre la operación. 
            // IMPORTANTE: Solo se establecen los datos mínimos necesarios para poder abrir el
            // ticket factura.
            //
            // Inspeccione la estructura cmdO.Input por otros datos que son opcionales, por ejemplo,
            // mas campos de dirección del cliente, etc.
            //
            // Respecto del 'TipoDeDocumento', el DNI/etc solo se permiten para consumidores finales.
            // El resto de los responsable deben presentar CUIT (o para el exento, CUIL)
            var cmdO = new CMD_TFAbrir();
            cmdO.Input.NomCliente_1 = "Martin Guzman";
            cmdO.Input.DirecCliente_1 = "Casa Rosada - CABA";
            cmdO.Input.RespIva = TipoRespEnum.Monotributo;
            cmdO.Input.TipoDoc = TipoDocEnum.CUIT;
            cmdO.Input.NroDoc = "20168993278";
            cmdO.Input.LineaDoc_1 = "083-0001-00000001";
            if (!ExecCommand(cmdO))
                return;

            // Una vez abierto el comprobante, el método de realizar operaciones es común entre 
            // Ticket Factura A/B y Nota de débito A/B
            CommonTF();
        }

        #endregion

        #region Responsable Inscripto (A)

        /// <summary>
        /// Ejemplo de nota de crédito A, emitida a un responsable inscripto
        /// </summary>
        /// 
        /// <remarks>
        /// Solo se demuestra la impresión de un ítem, pero todas las funciones demostradas
        /// en el "Ticket-Factura A" también están disponibles para la emisión de Notas de
        /// Débito.
        /// 
        /// Solo debe tener cuidado de usar el comando que corresponda. Por ejemplo, el descuento
        /// global se demuestra en el ticket factura usando el comando 'CMD_TFDescuento' pero, si
        /// quisiera usarlo en nota de credito, el comando sería 'CMD_NCDescuento'
        /// </remarks>
        public void NotaCreditoA()
        {
            // Abre la operación. 
            var cmdO = new CMD_NCAbrir();
            cmdO.Input.NomCliente_1 = "Alberdi S.A.";
            cmdO.Input.DirecCliente_1 = "Alberdi 1256 - Salta Capital";
            cmdO.Input.RespIva = TipoRespEnum.Inscripto;
            cmdO.Input.TipoDoc = TipoDocEnum.CUIT;
            cmdO.Input.NroDoc = "30578411174";
            cmdO.Input.LineaDoc_1 = "083-0001-00000001";
            if (!ExecCommand(cmdO))
                return;

            // Envía un ítem de $12.50 (10 x $1.25)
            // Prestar atención a la opción 'ImportesBrutos'. Al estar activa significa que el precio
            // unitario ($1.25, en este ejemplo), incluye impuestos. Se imprimirá discriminado, como
            // corresponde, pero el total a cobrar será 12.50.
            var cmdI = new CMD_NCItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.Unitario = 1.25m;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;                                  // 2 decimales por default.
            cmdI.Input.CodigoMedida = CodUnidadMedida.Litros;
            cmdI.Input.Descrip = "Aceite Patito PVC";
            cmdI.Input.ItemDescExtra1 = "Inmejorable para milanesas!";
            ExecCommand(cmdI);

            // Cierra la nota de debito.
            var cmdC = new CMD_NCCerrar();
            ExecCommand(cmdC);
        }

        /// <summary>
        /// Ejemplo de nota de débito A, emitida a un responsable inscripto
        /// </summary>
        /// 
        /// <remarks>
        /// Solo se demuestra la impresión de un ítem, pero todas las funciones demostradas
        /// en el "Ticket-Factura A" también están disponibles para la emisión de Notas de
        /// Débito.
        /// </remarks>
        public void NotaDebitoA()
        {
            // Abre la operación. 
            var cmdO = new CMD_TFAbrir();
            cmdO.Input.Tipo = true;                 // Indica que es 'Nota de Debito'
            cmdO.Input.NomCliente_1 = "Alberdi S.A.";
            cmdO.Input.DirecCliente_1 = "Alberdi 1256 - Salta Capital";
            cmdO.Input.RespIva = TipoRespEnum.Inscripto;
            cmdO.Input.TipoDoc = TipoDocEnum.CUIT;
            cmdO.Input.NroDoc = "30578411174";
            cmdO.Input.LineaDoc_1 = "083-0001-00000001";
            if (!ExecCommand(cmdO))
                return;

            // Envía un ítem de $12.50 (10 x $1.25)
            // Prestar atención a la opción 'ImportesBrutos'. Al estar activa significa que el precio
            // unitario ($1.25, en este ejemplo), incluye impuestos. Se imprimirá discriminado, como
            // corresponde, pero el total a cobrar será 12.50.
            var cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.Unitario = 1.25m;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;                                  // 2 decimales por default.
            cmdI.Input.CodigoMedida = CodUnidadMedida.Litros;
            cmdI.Input.Descrip = "Aceite Patito PVC";
            cmdI.Input.ItemDescExtra1 = "Inmejorable para milanesas!";
            ExecCommand(cmdI);

            // Cierra la nota de debito.
            var cmdC = new CMD_TFCerrar();
            ExecCommand(cmdC);
        }

        /// <summary>
        /// Ejemplo de ticket-factura "A", emitido a un responsable inscripto.
        /// </summary>
        /// 
        /// <remarks>
        /// Se demuestran la mayor parte de las operaciones posibles:
        /// - Ventas/Anulaciones a distintos IVAS y con distintos impuestos internos.
        /// - Descuentos/Recargos/Bonificaciones por ítem.
        /// - Descuentos/Recargos globales
        /// - Otros impuestos
        /// - Ingreso de medios de cobro.
        /// - Cierre de ticket con manipulación de las líneas de pie.
        /// </remarks>
        public void TicketFacturaA()
        {
            // Abre la operación. 
            // IMPORTANTE: Solo se establecen los datos mínimos necesarios para poder abrir el
            // ticket factura.
            // Inspeccione la estructura cmdO.Input por otros datos que son opcionales, por ejemplo,
            // mas campos de dirección del cliente, etc.
            var cmdO = new CMD_TFAbrir();
            cmdO.Input.NomCliente_1 = "Alberdi S.A.";
            cmdO.Input.DirecCliente_1 = "Alberdi 1256 - Salta Capital";
            cmdO.Input.RespIva = TipoRespEnum.Inscripto;
            cmdO.Input.TipoDoc = TipoDocEnum.CUIT;
            cmdO.Input.NroDoc = "30578411174";
            cmdO.Input.LineaDoc_1 = "083-0001-00000001";
            if (!ExecCommand(cmdO))
                return;

            // Una vez abierto el comprobante, el método de realizar operaciones es común entre 
            // Ticket Factura A/B y Nota de débito A/B
            CommonTF();
        }

        void CommonTF()
        { 
            // Envía un ítem de $12.50 (10 x $1.25)
            // Prestar atención a la opción 'ImportesBrutos'. Al estar activa significa que el precio
            // unitario ($1.25, en este ejemplo), incluye impuestos. Se imprimirá discriminado, como
            // corresponde, pero el total a cobrar será 12.50.
            var cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.Unitario = 1.25m;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;                                  // 2 decimales por default.
            cmdI.Input.CodigoMedida = CodUnidadMedida.Litros;
            cmdI.Input.Descrip = "Aceite Patito PVC";
            cmdI.Input.ItemDescExtra1 = "Inmejorable para milanesas!";
            ExecCommand(cmdI);

            // El mismo producto del renglón anterior, pero ahora el precio unitario es neto de 
            // impuestos (se corrige en este ejemplo $1.033 para que dé $1.25 al final, igual que
            // en el item anterior.
            cmdI.Input.Unitario = 1.033m;
            cmdI.Input.ImportesBrutos = false;
            ExecCommand(cmdI);

            // Envía un ítem que posteriormente se ANULARA
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Unitario = 15.10m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 1050;
            cmdI.Input.Descrip = "Renglon que anularemos";
            ExecCommand(cmdI);

            // Anula el renglón anterior. NO ES NECESARIO QUE LA ANULACIÓN sea correlativa con el
            // renglón a anular. Lo que importa es que todos los datos coincidan.
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Tipo = TFI_Tipo.AnulVenta;
            cmdI.Input.Unitario = 15.10m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 1050;
            cmdI.Input.Descrip = "Renglon que anularemos";
            ExecCommand(cmdI);

            // Prueba enviando un ítem exento de IVA
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Unitario = 30m;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.CondIVA = CodTKItemCondIva.Exento;
            cmdI.Input.Descrip = "Prueba producto exento";
            ExecCommand(cmdI);

            // Probamos un producto con $1.35 de impuesto interno. En estos casos, el producto debe
            // tener IVA distito de cero.
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Unitario = 30m;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.ImpIntFijos = 1.35m;
            cmdI.Input.Descrip = "ImpInt por Monto";
            ExecCommand(cmdI);

            // Ahora probamos un producto con 10% de impuesto interno.
            // El impuesto interno porcencual se codifica con 8 dígitos donde los 2 primeros son la
            // parte entera y el resto la decimal. Por ejemplo, 10% se configura 10000000 y un
            // valor de 5.374 será 5374000
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Unitario = 30m;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.ImpIntPorc = 10000000m;
            cmdI.Input.Descrip = "ImpInt por Monto";
            ExecCommand(cmdI);

            // Se pueden imprimir hasta 4 descripciones adicionales. Los datos MTX son para un posible
            // "archivo matrix" todavía no implementado por la AFIP, pero deberían ser el código EAN 
            // del producto y la cantidad real de ítems EAN vendidos (Sin son kgs, por ejemplo, cuenta
            // 1 cada paquete)
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Unitario = 12.35m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.Descrip = "Item completo";
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.CodigoMTX = "779779779779";
            cmdI.Input.UnidadMTX = 13;
            cmdI.Input.ItemDescExtra1 = "Linea de Descripcion 4";
            cmdI.Input.ItemDescExtra2 = "Linea de Descripcion 3";
            cmdI.Input.ItemDescExtra3 = "Linea de Descripcion 2";
            cmdI.Input.ItemDescExtra4 = "Linea de Descripcion 1";
            ExecCommand(cmdI);

            // Los recargos sobre un ítem anterior se codifican muy parecido que un ítem de venta
            // Importante: La cantidad debe ser '1'
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Tipo = TFI_Tipo.Recargo;
            cmdI.Input.Unitario = 12.35m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.Descrip = "Recargo de $12.35";
            ExecCommand(cmdI);

            // Los siguientes ejemplos muestran como hacer descuentos o bonificaciones sobre un ítem
            // ya vendido.
            //
            // IMPORTANTE: No se permitirá restar IVA que no se haya vendido antes.
            cmdI = new CMD_TFItem();
            cmdI.Input.ImportesBrutos = true;
            cmdI.Input.Tipo = TFI_Tipo.Bonif;
            cmdI.Input.Unitario = 9.60m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.Descrip = "Bonificación de $9.60";
            ExecCommand(cmdI);
            cmdI.Input.Tipo = TFI_Tipo.Descuento;
            cmdI.Input.Unitario = 11m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.Descrip = "Descuento de $11";
            ExecCommand(cmdI);

            // Si enviamos un subtotal ANTES de entrar a la fase de descuentos/recargos globales u
            // de "otros tributos" o de medios de cobro, el subtotal podrá imprimirse. Sino, no se
            // imprimirá pero si se devolverá el total actual del ticket.
            var cmdS = new CMD_TFSubtotal();
            cmdS.Input.Print = true;
            ExecCommand(cmdS);

            // Agregamos un recargo "global". Como tal, prorratea automáticamente el IVA/Impuesto 
            // interno que corresponda entre todas las tasas de IVA y de impuesto interno que se 
            // hayan usado dentro de la operación.
            var cmdG = new CMD_TFDescuento();
            cmdG.Input.Tipo = TFD_Tipo.Recargo;
            cmdG.Input.Monto = 25.50m;
            cmdG.Input.Descrip = "Recargo de prueba de $25";
            ExecCommand(cmdG);

            // Ahora hacemos un descuento global. Igual que en el recargo global, los impuestos
            // se prorratean automáticamente.
            cmdG = new CMD_TFDescuento();
            cmdG.Input.Tipo = TFD_Tipo.Descuento;
            cmdG.Input.Monto = 13.33m;
            cmdG.Input.Descrip = "Descuento de prueba de $13.33";
            ExecCommand(cmdG);

            // Como muestra de los "otros impuestos", agregamos una percepción de ingresos brutos
            var cmdOT = new CMD_TFOtroTributo();
            cmdOT.Input.Descrip = "Percepción IIBB de $7.30";
            cmdOT.Input.Monto = 7.30m;
            cmdOT.Input.TasaIVA = 2100;
            cmdOT.Input.Tipo = TFOT_Tipo.ImpIngBrutos;
            ExecCommand(cmdOT);

            // Mandamos otro subtotal. Este no se imprimirá nunca pero sirve, por ejemplo, para 
            // capturar el total final de la operación. Lo usaremos en este ejemplo, posteriormente,
            // para imprimir un código EAN al pie del ticket con el total.
            ExecCommand(cmdS);
            var totalTkt = cmdS.Output.MontoBruto;

            // Ingresando ya a la sección de pagos, registramos un pago parcial de $120 con tarjeta
            // de crédito.
            var cmdP = new CMD_TFPago();
            cmdP.Input.Codigo = CodMedioDePago.Tarjeta_de_credito;
            cmdP.Input.Cuotas = 3;
            cmdP.Input.DetaAdic = "Mastercard Bco. Galicia";
            cmdP.Input.DetaCupon = "1321";
            cmdP.Input.Monto = 100m;
            cmdP.Input.PagoDescExtra1 = "Plan 'Paga como quieras'";
            cmdP.Input.PagoDescExtra2 = "www.mastercard.com.ar";
            ExecCommand(cmdP);

            // Completamos con otro medio de pago que incluso nos genera el vuelto.
            // IMPORTANTE: Si los medios de pago no llegaran a completar la operación, el comando
            // de cierre de operación "completará" la operación con un ingreso automático de 
            // efectivo.
            cmdP = new CMD_TFPago();
            cmdP.Input.Codigo = CodMedioDePago.Efectivo;
            cmdP.Input.Monto = 1000m;
            ExecCommand(cmdP);

            // Cierra el ticket. Observa como se reemplaza una de la línea nro 5 del pié de ticket
            // por un código de barra EAN con el total del ticket.
            var cmdC = new CMD_TFCerrar();
            cmdC.Input.RNumLinea1 = 5;
            cmdC.Input.RTextLinea1 = "\\cc2501" + ((int)totalTkt).ToString().PadLeft(8, '0');
            ExecCommand(cmdC);
        }
        #endregion

        #region Consumidor Final

        /// <summary>
        /// Ejemplo de ticket a consumidor final.
        /// </summary>
        /// 
        /// <remarks>
        /// Se demuestran la mayor parte de las operaciones posibles:
        /// - Ventas/Anulaciones a distintos IVAS y con distintos impuestos internos.
        /// - Descuentos/Recargos/Bonificaciones por ítem.
        /// - Descuentos/Recargos globales
        /// - Otros impuestos
        /// - Ingreso de medios de cobro.
        /// - Cierre de ticket con manipulación de las líneas de pie.
        /// </remarks>
        public void TicketConsumidorFinal()
        {
            // Abre la operación. 
            // TCF= No hace falta setear nada más en el comando.
            var cmdO = new CMD_TKAbrir();
            if (!ExecCommand(cmdO))
                return;

            // Envía un ítem de $12.50 (10 x $1.25)
            var cmdI = new CMD_TKItem();
            cmdI.Input.Cantidad = 10;
            cmdI.Input.Unitario = 1.25m;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;                                  // 2 decimales por default.
            cmdI.Input.CodigoMedida = CodUnidadMedida.Litros;
            cmdI.Input.Descrip = "Aceite Patito PVC";
            cmdI.Input.ItemDescExtra1 = "Inmejorable para milanesas!";
            ExecCommand(cmdI);

            // Envía un ítem que posteriormente se ANULARA
            cmdI = new CMD_TKItem();
            cmdI.Input.Unitario = 15.10m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 1050;
            cmdI.Input.Descrip = "Renglon que anularemos";
            ExecCommand(cmdI);

            // Anula el renglón anterior. NO ES NECESARIO QUE LA ANULACIÓN sea correlativa con el
            // renglón a anular. Lo que importa es que todos los datos coincidan.
            cmdI = new CMD_TKItem();
            cmdI.Input.Tipo = TKI_Tipo.AnulVenta;
            cmdI.Input.Unitario = 15.10m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 1050;
            cmdI.Input.Descrip = "Renglon que anularemos";
            ExecCommand(cmdI);

            // Prueba enviando un ítem exento de IVA
            cmdI = new CMD_TKItem();
            cmdI.Input.Unitario = 30m;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.CondIVA = CodTKItemCondIva.Exento;
            cmdI.Input.Descrip = "Prueba producto exento";
            ExecCommand(cmdI);

            // Probamos un producto con $1.35 de impuesto interno. En estos casos, el producto debe
            // tener IVA distito de cero.
            cmdI = new CMD_TKItem();
            cmdI.Input.Unitario = 30m;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.ImpIntFijos = 1.35m;
            cmdI.Input.Descrip = "ImpInt por Monto";
            ExecCommand(cmdI);

            // Ahora probamos un producto con 10% de impuesto interno.
            // El impuesto interno porcencual se codifica con 8 dígitos donde los 2 primeros son la
            // parte entera y el resto la decimal. Por ejemplo, 10% se configura 10000000 y un
            // valor de 5.374 será 5374000
            cmdI = new CMD_TKItem();
            cmdI.Input.Unitario = 30m;
            cmdI.Input.Cantidad = 10;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.ImpIntPorc = 10000000m;
            cmdI.Input.Descrip = "ImpInt por Monto";
            ExecCommand(cmdI);

            // Se pueden imprimir hasta 4 descripciones adicionales. Los datos MTX son para un posible
            // "archivo matrix" todavía no implementado por la AFIP, pero deberían ser el código EAN 
            // del producto y la cantidad real de ítems EAN vendidos (Sin son kgs, por ejemplo, cuenta
            // 1 cada paquete)
            cmdI = new CMD_TKItem();
            cmdI.Input.Unitario = 12.35m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.Descrip = "Item completo";
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.CodigoMTX = "779779779779";
            cmdI.Input.UnidadMTX = 13;
            cmdI.Input.ItemDescExtra1 = "Linea de Descripcion 4";
            cmdI.Input.ItemDescExtra2 = "Linea de Descripcion 3";
            cmdI.Input.ItemDescExtra3 = "Linea de Descripcion 2";
            cmdI.Input.ItemDescExtra4 = "Linea de Descripcion 1";
            ExecCommand(cmdI);

            // Los recargos sobre un ítem anterior se codifican muy parecido que un ítem de venta
            // Importante: La cantidad debe ser '1'
            cmdI = new CMD_TKItem();
            cmdI.Input.Tipo = TKI_Tipo.Recargo;
            cmdI.Input.Unitario = 12.35m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.Descrip = "Recargo de $12.35";
            ExecCommand(cmdI);

            // Los siguientes ejemplos muestran como hacer descuentos o bonificaciones sobre un ítem
            // ya vendido.
            //
            // IMPORTANTE: No se permitirá restar IVA que no se haya vendido antes.
            cmdI = new CMD_TKItem();
            cmdI.Input.Tipo = TKI_Tipo.Bonif;
            cmdI.Input.Unitario = 9.60m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;
            cmdI.Input.Descrip = "Bonificación de $9.60";
            ExecCommand(cmdI);
            cmdI.Input.Tipo = TKI_Tipo.Descuento;
            cmdI.Input.Unitario = 11m;
            cmdI.Input.Cantidad = 1;
            cmdI.Input.Descrip = "Descuento de $11";
            ExecCommand(cmdI);

            // Si enviamos un subtotal ANTES de entrar a la fase de descuentos/recargos globales u
            // de "otros tributos" o de medios de cobro, el subtotal podrá imprimirse. Sino, no se
            // imprimirá pero si se devolverá el total actual del ticket.
            var cmdS = new CMD_TKSubtotal();
            cmdS.Input.Print = true;
            ExecCommand(cmdS);

            // Agregamos un recargo "global". Como tal, prorratea automáticamente el IVA/Impuesto 
            // interno que corresponda entre todas las tasas de IVA y de impuesto interno que se 
            // hayan usado dentro de la operación.
            var cmdG = new CMD_TKDescuento();
            cmdG.Input.Tipo = TKD_Tipo.Recargo;
            cmdG.Input.Monto = 25.50m;
            cmdG.Input.Descrip = "Recargo de prueba de $25";
            ExecCommand(cmdG);

            // Ahora hacemos un descuento global. Igual que en el recargo global, los impuestos
            // se prorratean automáticamente.
            cmdG = new CMD_TKDescuento();
            cmdG.Input.Tipo = TKD_Tipo.Descuento;
            cmdG.Input.Monto = 13.33m;
            cmdG.Input.Descrip = "Descuento de prueba de $13.33";
            ExecCommand(cmdG);

            // Como muestra de los "otros impuestos", agregamos una percepción de ingresos brutos
            var cmdOT = new CMD_TKOtroTributo();
            cmdOT.Input.Descrip = "Percepción IIBB de $7.30";
            cmdOT.Input.Monto = 7.30m;
            cmdOT.Input.TasaIVA = 2100;
            cmdOT.Input.Tipo = TKOT_Tipo.ImpIngBrutos;
            ExecCommand(cmdOT);

            // Mandamos otro subtotal. Este no se imprimirá nunca pero sirve, por ejemplo, para 
            // capturar el total final de la operación. Lo usaremos en este ejemplo, posteriormente,
            // para imprimir un código EAN al pie del ticket con el total.
            ExecCommand(cmdS);
            var totalTkt = cmdS.Output.Monto;

            // Ingresando ya a la sección de pagos, registramos un pago parcial de $120 con tarjeta
            // de crédito.
            var cmdP = new CMD_TKPago();
            cmdP.Input.Codigo = CodMedioDePago.Tarjeta_de_credito;
            cmdP.Input.Cuotas = 3;
            cmdP.Input.DetaAdic = "Mastercard Bco. Galicia";
            cmdP.Input.DetaCupon = "1321";
            cmdP.Input.Monto = 100m;
            cmdP.Input.PagoDescExtra1 = "Plan 'Paga como quieras'";
            cmdP.Input.PagoDescExtra2 = "www.mastercard.com.ar";
            ExecCommand(cmdP);

            // Completamos con otro medio de pago que incluso nos genera el vuelto.
            // IMPORTANTE: Si los medios de pago no llegaran a completar la operación, el comando
            // de cierre de operación "completará" la operación con un ingreso automático de 
            // efectivo.
            cmdP = new CMD_TKPago();
            cmdP.Input.Codigo = CodMedioDePago.Efectivo;
            cmdP.Input.Monto = 1000m;
            ExecCommand(cmdP);

            // Cierra el ticket. Observa como se reemplaza una de la línea nro 5 del pié de ticket
            // por un código de barra EAN con el total del ticket.
            var cmdC = new CMD_TKCerrar();
            cmdC.Input.RNumLinea1 = 5;
            cmdC.Input.RTextLinea1 = "\\cc2501" + ((int)totalTkt).ToString().PadLeft(8, '0');
            ExecCommand(cmdC);
        }

        /// <summary>
        /// Emite una nota de credito de ejemplo a consumidor final.
        /// </summary>
        /// 
        /// <remarks>
        /// Son posibles TODAS las operaciones que también son posibles para el ticket a consumidor
        /// final. Por una cuestión de brevedad, no se vuelven a demostrar acá.
        /// </remarks>
        public void NotaCreditoConsumidorFinal()
        {
            // Abre la operación. 
            // Hace falta indicar que es N/C poniendo 'Tipo' en true
            var cmdO = new CMD_TKAbrir();
            cmdO.Input.TipoDoc = true;
            if (!ExecCommand(cmdO))
                return;

            // Envía un ítem de $12.50 (10 x $1.25)
            var cmdI = new CMD_TKItem();
            cmdI.Input.Cantidad = 10;
            cmdI.Input.Unitario = 1.25m;
            cmdI.Input.CondIVA = CodTKItemCondIva.Gravado;
            cmdI.Input.TasaIVA = 2100;                                  // 2 decimales por default.
            cmdI.Input.CodigoMedida = CodUnidadMedida.Litros;
            cmdI.Input.Descrip = "Aceite Patito PVC";
            ExecCommand(cmdI);

            // Cierra la nota de crédito
            var cmdC = new CMD_TKCerrar();
            ExecCommand(cmdC);
        }
        #endregion

        #region Operaciones Varias

        /// <summary>
        /// Demuestra como emitir un cierre diario Z.
        /// </summary>
        /// 
        /// <remarks>
        /// El comando de cierre Z solo ordena la ejecución del reporte. Si, aparte, deseara
        /// obtener información de ventas de forma electrónica, examine los otros comandos
        /// disponibles dentro del namespace 'Jornada_Fiscal', en especial, el juego de
        /// comandos "JOR_GetInfo*".
        /// 
        /// Estos comandos para obtener información deben ejecutar <b>antes</b> de enviar
        /// el comando de cierre Z.
        /// </remarks>
        public void CierreZeta()
        {
            var cmdZ = new CMD_JORZeta();
            ExecCommand(cmdZ);
        }

        /// <summary>
        /// Impresión de voucher genérico.
        /// </summary>
        /// 
        /// <remarks>
        /// Se puede usar para imprimir cualquier comprobante que no sea de venta, por ejemplo,
        /// un voucher de tarjeta de crédito.
        /// </remarks>
        public void VoucherGenérico()
        {
            // Abre el DNF
            var cmdO = new CMD_DNFH1Abrir();
            cmdO.Input.Encabezamientos = true;      // Indica que se impriman TODOS los header
            ExecCommand(cmdO);

            // Imprime el texto del voucher. Se pueden enviar 30 lineas de texto a la vez, en
            // el mismo comando.
            var cmdL1 = new CMD_DNFH1Lineas();
            cmdL1.Input.Linea1 = "Estamos emitiendo 5 lineas a la vez (1/5)";
            cmdL1.Input.Linea2 = "Estamos emitiendo 5 lineas a la vez (2/5)";
            cmdL1.Input.Linea3 = "Estamos emitiendo 5 lineas a la vez (3/5)";
            cmdL1.Input.Linea4 = "Estamos emitiendo 5 lineas a la vez (4/5)";
            cmdL1.Input.Linea5 = "Estamos emitiendo 5 lineas a la vez (5/5)";
            ExecCommand(cmdL1);

            // También se puede imprimir el texto del voucher a a una línea por vez.
            var cmdL2 = new CMD_DNFH1Texto();
            cmdL2.Input.Texto = "Linea de texto enviada en solitario";
            ExecCommand(cmdL2);

            // Cierra el DNF. Aunque acá no se demuestra, también se pueden reemplazar hasta 3
            // líneas del pié de ticket.
            var cmdC = new CMD_DNFH1Cerrar();
            cmdC.Input.Colas = true;            // Indica imprimir TODOS los pie de ticket
            cmdC.Input.CortaPapel = true;       // Indica guillotinar el papel al final
            ExecCommand(cmdC);
        }
        #endregion
    }
}
