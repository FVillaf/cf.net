using System;
using System.Windows.Forms;

namespace OpenPEM
{
    /// <summary>
    /// Code behind de la form principal
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.Text = $"OpenPEM Versión { PemConverter.Version } - Copyright (c) 2021 federvillaf@hotmail.com";
        }

        /// <summary>
        /// Handler del evento 'click' del boton de "Generar XML"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btGo_Click(object sender, EventArgs e)
        {
            try
            {
                PemConverter.Convert2Xml(tbSource.Text, tbTarget.Text);
                MessageBox.Show(
                    "Listo. Archivo XML generado correctamente",
                    "Atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    $"Error: '{ex.Message}'",
                    "Atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handler del evento 'click' del boton para buscar el archivo a convertir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFind1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                tbSource.Text = openFileDialog.FileName;
        }

        /// <summary>
        /// Actualiza el nombre del target cada vez que se edita el source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSource_TextChanged(object sender, EventArgs e)
        {
            tbTarget.Text = PemConverter.DeriveTarget(tbSource.Text);
        }

        /// <summary>
        /// Al cerrar la form restaura la consola
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.HandleConsoleWindow(true);
        }

        /// <summary>
        /// Al cargarse la form, oculta la ventana de comandos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.HandleConsoleWindow(false);
        }
    }
}
