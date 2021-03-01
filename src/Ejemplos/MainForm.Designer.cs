
namespace Ejemplos
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btConfComu = new System.Windows.Forms.Button();
            this.labPapel = new System.Windows.Forms.Label();
            this.labPort = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btTest_TCF = new System.Windows.Forms.Button();
            this.btTest_NCF = new System.Windows.Forms.Button();
            this.btTest_TFA = new System.Windows.Forms.Button();
            this.btTest_TFB = new System.Windows.Forms.Button();
            this.btTest_NDB = new System.Windows.Forms.Button();
            this.btTest_NDA = new System.Windows.Forms.Button();
            this.btTest_NCB = new System.Windows.Forms.Button();
            this.btTest_NCA = new System.Windows.Forms.Button();
            this.btTest_Z = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btTest_GEN = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btConfComu
            // 
            this.btConfComu.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.btConfComu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btConfComu.Location = new System.Drawing.Point(15, 29);
            this.btConfComu.Name = "btConfComu";
            this.btConfComu.Size = new System.Drawing.Size(134, 21);
            this.btConfComu.TabIndex = 0;
            this.btConfComu.Text = "Cambiar...";
            this.btConfComu.UseVisualStyleBackColor = true;
            this.btConfComu.Click += new System.EventHandler(this.btConfComu_Click);
            // 
            // labPapel
            // 
            this.labPapel.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold);
            this.labPapel.ForeColor = System.Drawing.Color.Red;
            this.labPapel.Location = new System.Drawing.Point(322, 76);
            this.labPapel.Name = "labPapel";
            this.labPapel.Size = new System.Drawing.Size(207, 26);
            this.labPapel.TabIndex = 1;
            this.labPapel.Text = "¡¡ Falta Papel !!";
            this.labPapel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labPapel.Visible = false;
            // 
            // labPort
            // 
            this.labPort.AutoSize = true;
            this.labPort.Location = new System.Drawing.Point(108, 10);
            this.labPort.Name = "labPort";
            this.labPort.Size = new System.Drawing.Size(41, 13);
            this.labPort.TabIndex = 2;
            this.labPort.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Puerto a Usar:";
            // 
            // btTest_TCF
            // 
            this.btTest_TCF.Location = new System.Drawing.Point(12, 14);
            this.btTest_TCF.Name = "btTest_TCF";
            this.btTest_TCF.Size = new System.Drawing.Size(214, 23);
            this.btTest_TCF.TabIndex = 4;
            this.btTest_TCF.Text = "Ticket a Consumidor Final";
            this.btTest_TCF.UseVisualStyleBackColor = true;
            this.btTest_TCF.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_NCF
            // 
            this.btTest_NCF.Location = new System.Drawing.Point(12, 43);
            this.btTest_NCF.Name = "btTest_NCF";
            this.btTest_NCF.Size = new System.Drawing.Size(214, 23);
            this.btTest_NCF.TabIndex = 5;
            this.btTest_NCF.Text = "Nota de Crédito a Cons. Final";
            this.btTest_NCF.UseVisualStyleBackColor = true;
            this.btTest_NCF.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_TFA
            // 
            this.btTest_TFA.Location = new System.Drawing.Point(12, 90);
            this.btTest_TFA.Name = "btTest_TFA";
            this.btTest_TFA.Size = new System.Drawing.Size(214, 23);
            this.btTest_TFA.TabIndex = 6;
            this.btTest_TFA.Text = "Ticket Factura \'A\'";
            this.btTest_TFA.UseVisualStyleBackColor = true;
            this.btTest_TFA.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_TFB
            // 
            this.btTest_TFB.Location = new System.Drawing.Point(12, 119);
            this.btTest_TFB.Name = "btTest_TFB";
            this.btTest_TFB.Size = new System.Drawing.Size(214, 23);
            this.btTest_TFB.TabIndex = 7;
            this.btTest_TFB.Text = "Ticket Factura \'B\' o \'C\'";
            this.btTest_TFB.UseVisualStyleBackColor = true;
            this.btTest_TFB.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_NDB
            // 
            this.btTest_NDB.Location = new System.Drawing.Point(12, 176);
            this.btTest_NDB.Name = "btTest_NDB";
            this.btTest_NDB.Size = new System.Drawing.Size(214, 23);
            this.btTest_NDB.TabIndex = 9;
            this.btTest_NDB.Text = "Nota de Débito \'B\' o \'C\'";
            this.btTest_NDB.UseVisualStyleBackColor = true;
            this.btTest_NDB.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_NDA
            // 
            this.btTest_NDA.Location = new System.Drawing.Point(12, 147);
            this.btTest_NDA.Name = "btTest_NDA";
            this.btTest_NDA.Size = new System.Drawing.Size(214, 23);
            this.btTest_NDA.TabIndex = 8;
            this.btTest_NDA.Text = "Nota de Débito \'A\'";
            this.btTest_NDA.UseVisualStyleBackColor = true;
            this.btTest_NDA.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_NCB
            // 
            this.btTest_NCB.Location = new System.Drawing.Point(12, 234);
            this.btTest_NCB.Name = "btTest_NCB";
            this.btTest_NCB.Size = new System.Drawing.Size(214, 23);
            this.btTest_NCB.TabIndex = 11;
            this.btTest_NCB.Text = "Nota de Crédito \'B\' o \'C\'";
            this.btTest_NCB.UseVisualStyleBackColor = true;
            this.btTest_NCB.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_NCA
            // 
            this.btTest_NCA.Location = new System.Drawing.Point(12, 205);
            this.btTest_NCA.Name = "btTest_NCA";
            this.btTest_NCA.Size = new System.Drawing.Size(214, 23);
            this.btTest_NCA.TabIndex = 10;
            this.btTest_NCA.Text = "Nota de Crédito \'A\'";
            this.btTest_NCA.UseVisualStyleBackColor = true;
            this.btTest_NCA.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btTest_Z
            // 
            this.btTest_Z.Location = new System.Drawing.Point(315, 43);
            this.btTest_Z.Name = "btTest_Z";
            this.btTest_Z.Size = new System.Drawing.Size(214, 23);
            this.btTest_Z.TabIndex = 12;
            this.btTest_Z.Text = "Cierre Diario (Z)";
            this.btTest_Z.UseVisualStyleBackColor = true;
            this.btTest_Z.Click += new System.EventHandler(this.btTest_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btConfComu);
            this.panel1.Controls.Add(this.labPort);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(269, 191);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(319, 66);
            this.panel1.TabIndex = 13;
            // 
            // btTest_GEN
            // 
            this.btTest_GEN.Location = new System.Drawing.Point(315, 14);
            this.btTest_GEN.Name = "btTest_GEN";
            this.btTest_GEN.Size = new System.Drawing.Size(214, 23);
            this.btTest_GEN.TabIndex = 15;
            this.btTest_GEN.Text = "Voucher Genérico";
            this.btTest_GEN.UseVisualStyleBackColor = true;
            this.btTest_GEN.Click += new System.EventHandler(this.btTest_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(600, 270);
            this.Controls.Add(this.btTest_GEN);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btTest_Z);
            this.Controls.Add(this.btTest_NCB);
            this.Controls.Add(this.btTest_NCA);
            this.Controls.Add(this.btTest_NDB);
            this.Controls.Add(this.btTest_NDA);
            this.Controls.Add(this.btTest_TFB);
            this.Controls.Add(this.btTest_TFA);
            this.Controls.Add(this.btTest_NCF);
            this.Controls.Add(this.btTest_TCF);
            this.Controls.Add(this.labPapel);
            this.Font = new System.Drawing.Font("Verdana", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = " Moretti Génesis - Pruebas";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btConfComu;
        private System.Windows.Forms.Label labPapel;
        private System.Windows.Forms.Label labPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btTest_TCF;
        private System.Windows.Forms.Button btTest_NCF;
        private System.Windows.Forms.Button btTest_TFA;
        private System.Windows.Forms.Button btTest_TFB;
        private System.Windows.Forms.Button btTest_NDB;
        private System.Windows.Forms.Button btTest_NDA;
        private System.Windows.Forms.Button btTest_NCB;
        private System.Windows.Forms.Button btTest_NCA;
        private System.Windows.Forms.Button btTest_Z;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btTest_GEN;
    }
}

