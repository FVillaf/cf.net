using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Management;

using FiscalProto;

namespace Ejemplos
{
    /// <summary>
    /// Forma para configurar el canal de comunicacion que usaremos
    /// </summary>
    public partial class TargetConfigure : Form
    {
        /// <summary>
        /// Información sobre el target
        /// </summary>
        TargetBag originalBag;

        /// <summary>
        /// Velocidad a usar en el puerto serial
        /// </summary>
        SpeedCode useSpeed;

        /// <summary>
        /// La lista de los puertos seriales descubiertos
        /// </summary>
        List<PortDescription> portList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bag">El tipo de target al que nos estamos conectando</param>
        public TargetConfigure(TargetBag bag)
        {
            InitializeComponent();

            this.originalBag = bag;
            portList = new List<PortDescription>();
            tbIP.Top = tbPort.Top = cbSpeed.Top;
            btPing.Top = cbSpeed.Top - 1;
            lbPort.Top = cbSpeed.Top + 3;

            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity"))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        object obj = queryObj["Name"];
                        if (obj != null)
                        {
                            var nom = obj.ToString();
                            var index = nom.ToLower().IndexOf("(com");
                            if (index >= 0)
                            {
                                index++;
                                for (int i = index; i < nom.Length; i++)
                                {
                                    if (nom[i] == ')')
                                    {
                                        string portName = nom.Substring(index, i - index);
                                        portList.Add(new PortDescription { Name = portName, Description = nom });
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show($"Error examinando el equipo: '{e.Message}'");
            }

            portList.Add(new PortDescription { Name = "Red", Description = "Red Ethernet por TCP/IP" });
            portList.Sort((a, b) => a.Name.ToUpper().CompareTo(b.Name.ToUpper()));

            cbPortName.Items.Clear();
            foreach (var kvp in portList)
            {
                cbPortName.Items.Add(kvp);
                bool doSelect = originalBag.UseNetwork ?
                    kvp.Name.ToLower() == "red" :
                    kvp.Name.ToLower() == originalBag.PortName.ToLower();
                if (doSelect)
                    cbPortName.SelectedIndex = cbPortName.Items.Count - 1;
            }

            this.useSpeed = bag.Speed;
            this.Text = " " + bag.Name;

            int sel = -1;
            string actual = useSpeed.ToString();
            foreach (var vn in Enum.GetNames(typeof(SpeedCode)))
            {
                if (vn == actual)
                    sel = cbSpeed.Items.Count;
                cbSpeed.Items.Add(vn.Substring(1));
            }
            if (sel >= 0)
                cbSpeed.SelectedIndex = sel;
            tbIP.Text = bag.IPAddress;
        }

        /// <summary>
        /// Manejador para el evento 'SelectedIndexChanged' del combobox usado para elegir el port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pd = (PortDescription)cbPortName.SelectedItem;
            if (pd.Name.ToLower() == "red")
            {
                lbPropName.Text = "Dirección IP";
                lbPropName.Visible = true;
                cbSpeed.Visible = false;
                tbIP.Visible = lbPort.Visible = tbPort.Visible = btPing.Visible = true;
            }
            else if (pd.Description.IndexOf("CH340") >= 0)
            {
                lbPropName.Visible = false;
                cbSpeed.Visible = false;
                tbIP.Visible = lbPort.Visible = tbPort.Visible = btPing.Visible =
                    false;
            }
            else
            {
                lbPropName.Text = "Velocidad de Comunicación:";
                lbPropName.Visible = true;
                cbSpeed.Visible = true;
                tbIP.Visible = lbPort.Visible = tbPort.Visible = btPing.Visible =
                    false;
            }

            Valid_Changes(null, null);
        }

        /// <summary>
        /// Manejador del evento 'SelectedIndexChanged' del combobox usado para elegir la 
        /// velocidad de comunicacion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pn = "B" + (string)cbSpeed.SelectedItem;
            useSpeed = (SpeedCode)Enum.Parse(typeof(SpeedCode), pn);
        }

        /// <summary>
        /// Manejador del evento 'Click' del boton 'Cancelar'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCanc_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Manejador del evento 'Click' del boton 'Ok'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (tbIP.Visible)
            {
                originalBag.UseNetwork = true;
                originalBag.IPAddress = tbIP.Text.Trim();
            }
            else
            {
                originalBag.UseNetwork = false;
                originalBag.PortName = ((PortDescription)cbPortName.SelectedItem).Name;
                originalBag.Speed = cbSpeed.Visible ? useSpeed : SpeedCode.B115200;
            }
            originalBag.Save();
        }

        /// <summary>
        /// Manejador de evento que se usa para saber si se tipio una direccion IP valida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Valid_Changes(object sender, EventArgs e)
        {
            IPAddress addr;

            if (tbIP.Visible)
            {
                bool valid = IPAddress.TryParse(tbIP.Text.Trim(), out addr);
                btPing.Enabled = btOk.Enabled = valid;
            }
            else
            {
                btOk.Enabled = true;
            }
        }

        /// <summary>
        /// Evento de carga de la form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetConfigure_Load(object sender, EventArgs e)
        {
            Valid_Changes(null, null);
        }

        /// <summary>
        /// Evento 'Click' del boton de 'Ping'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPing_Click(object sender, EventArgs e)
        {
            using (var ping = new Ping())
            {
                var reply = ping.Send(IPAddress.Parse(tbIP.Text.Trim()));
                var msg = (reply.Status == IPStatus.Success) ?
                    "¡¡PING Exitoso!!" :
                    $"El PING falló con el error '{reply.Status.ToString()}'";
                var icon = (reply.Status == IPStatus.Success) ?
                    MessageBoxIcon.Information :
                    MessageBoxIcon.Error;

                MessageBox.Show(msg, "Resultado", MessageBoxButtons.OK, icon);
            }
        }

        /// <summary>
        /// Clase auxiliar para contener la descripción de un puerto serial.
        /// </summary>
        class PortDescription
        {
            public string Name;
            public string Description;
            public override string ToString()
            {
                return Description;
            }
        }
    }
}
