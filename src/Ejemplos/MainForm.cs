using System;
using System.IO;
using System.Windows.Forms;

using FiscalProto;

namespace Ejemplos
{
    /// <summary>
    /// Form principal del despachador de ejemplos.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// La configuración del dispositivo.
        /// </summary>
        TargetBag bag;

        /// <summary>
        /// El gestor de protocolo a utilizar
        /// </summary>
        ProtoHandler proto;

        /// <summary>
        /// El 'hook' anterior para el evento de falta de papel.
        /// </summary>
        Action<bool> prevHook;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            bag = TargetBag.FromCode(BagCode.Genesis);
            if (!File.Exists("aclas.cfg"))
                btConfComu_Click(null, null);
            else
                SetProtoHandler(new ProtoHandler(bag));
        }

        /// <summary>
        /// Establece un nuevo gestor de protocolo.
        /// </summary>
        /// 
        /// <param name="nproto">El nuevo gestor</param>
        void SetProtoHandler(ProtoHandler nproto)
        {
            this.proto = nproto;
            prevHook = this.proto.LowPaper;
            labPort.Text = nproto.Port.Name;
            this.proto.LowPaper = opened =>
            {
                if (!this.IsDisposed)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (labPapel.Visible)
                            labPapel.Visible = opened;
                    });
                }
                return;
            };
        }

        /// <summary>
        /// 'Click' en el botón de 'Configurar'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConfComu_Click(object sender, EventArgs e)
        {
            var conf = new TargetConfigure(bag);
            if (conf.ShowDialog() == DialogResult.OK)
                SetProtoHandler(new ProtoHandler(bag));
        }

        /// <summary>
        /// Muestra un mensaje de error
        /// </summary>
        /// <param name="msg">el mensaje de error</param>
        /// <param name="isError"><b>true</b>si estamos mostrando un error</param>
        void ShowMessage(string msg, bool isError = false)
        {
            MessageBox.Show(
                msg,
                "Atención",
                MessageBoxButtons.OK,
                isError? MessageBoxIcon.Stop:MessageBoxIcon.Information);
        }

        /// <summary>
        /// 'click' en cualquiera de los botones de generación de ejemplo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btTest_Click(object sender, EventArgs e)
        {
            var erun = new ERunner(proto, emsg => ShowMessage(emsg));
            var cCode = ((Button)sender).Name.Substring(7).ToLower();
            switch(cCode)
            {
                case "tcf": erun.TicketConsumidorFinal(); break;
                case "ncf": erun.NotaCreditoConsumidorFinal(); break;

                case "tfa": erun.TicketFacturaA(); break;
                case "nda": erun.NotaDebitoA(); break;
                case "nca": erun.NotaCreditoA(); break;

                case "tfb": erun.TicketFacturaB(); break;
                case "ndb": erun.NotaDebitoB(); break;
                case "ncb": erun.NotaCreditoB(); break;

                case "z": erun.CierreZeta(); break;
                case "gen": erun.VoucherGenérico();  break;

                default:
                    MessageBox.Show($"Operación '{cCode}' no soportada.");
                    break;
            }
        }

        /// <summary>
        /// Metodo de ayuda para guardar el XML recibido en un determinado archivo
        /// </summary>
        /// <param name="xml"></param>
        void SaveXml(int codDoc, int nroDoc, string xml)
        {
            saveFileDialog.FileName = $"Doc_{ codDoc.ToString().PadLeft(3,'0') }_{ nroDoc.ToString().PadLeft(8,'0') }.xml";
            var res = saveFileDialog.ShowDialog();
            if(res == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, xml);
                    ShowMessage($"Listo, '{ saveFileDialog.FileName } grabado", false);
                }
                catch (Exception ex) { ShowMessage(ex.Message); }
            }
        }

        /// <summary>
        /// 'click' en el botón que lanza la reimpresión/descarga-xml de una operaicón
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReimpProc_Click(object sender, EventArgs e)
        {
            if(!chkPrint.Checked && !chkXml.Checked)
            {
                ShowMessage("Indice 'Imprimir' o 'Xml'");
                return;
            }

            var index = cbDocList.Text.IndexOf('-');
            if(index < 0)
            {
                ShowMessage("Elija el tipo de comprobante a reimprimir");
                return;
            }

            int codDoc = int.Parse(cbDocList.Text.Substring(0, index).Trim());
            int tbNro;
            if(!int.TryParse(cbDocNro.Text.Trim(), out tbNro))
            {
                ShowMessage("Número incorrecto de comprobante");
                return;
            }

            var erun = new ERunner(proto, emsg => ShowMessage(emsg));
            erun.Reprint(
                codDoc, 
                tbNro, 
                chkPrint.Checked, 
                chkXml.Checked, 
                xml => SaveXml(codDoc, tbNro, xml));
        }
    }
}
