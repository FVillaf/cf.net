
namespace Ejemplos
{
        partial class TargetConfigure
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.tbIP = new System.Windows.Forms.TextBox();
                this.btPing = new System.Windows.Forms.Button();
                this.tbPort = new System.Windows.Forms.TextBox();
                this.lbPort = new System.Windows.Forms.Label();
                this.btCanc = new System.Windows.Forms.Button();
                this.btOk = new System.Windows.Forms.Button();
                this.cbSpeed = new System.Windows.Forms.ComboBox();
                this.lbPropName = new System.Windows.Forms.Label();
                this.cbPortName = new System.Windows.Forms.ComboBox();
                this.label2 = new System.Windows.Forms.Label();
                this.SuspendLayout();
                // 
                // tbIP
                // 
                this.tbIP.Location = new System.Drawing.Point(13, 106);
                this.tbIP.Multiline = false;
                this.tbIP.Name = "tbIP";
                this.tbIP.PasswordChar = '\0';
                this.tbIP.ReadOnly = false;
                this.tbIP.ScrollBars = System.Windows.Forms.ScrollBars.None;
                this.tbIP.Size = new System.Drawing.Size(96, 26);
                this.tbIP.TabIndex = 22;
                this.tbIP.TextChanged += new System.EventHandler(this.Valid_Changes);
                // 
                // btPing
                // 
                this.btPing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(141)))));
                this.btPing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btPing.Font = new System.Drawing.Font("Verdana", 7F);
                this.btPing.ForeColor = System.Drawing.Color.White;
                this.btPing.Location = new System.Drawing.Point(228, 105);
                this.btPing.Name = "btPing";
                this.btPing.Size = new System.Drawing.Size(76, 23);
                this.btPing.TabIndex = 21;
                this.btPing.Text = "Ping";
                this.btPing.UseVisualStyleBackColor = true;
                this.btPing.Visible = false;
                this.btPing.Click += new System.EventHandler(this.btPing_Click);
                // 
                // tbPort
                // 
                this.tbPort.Location = new System.Drawing.Point(158, 105);
                this.tbPort.Multiline = false;
                this.tbPort.Name = "tbPort";
                this.tbPort.PasswordChar = '\0';
                this.tbPort.ReadOnly = true;
                this.tbPort.ScrollBars = System.Windows.Forms.ScrollBars.None;
                this.tbPort.Size = new System.Drawing.Size(49, 26);
                this.tbPort.TabIndex = 20;
                this.tbPort.Text = "5003";
                this.tbPort.Visible = false;
                // 
                // lbPort
                // 
                this.lbPort.AutoSize = true;
                this.lbPort.Font = new System.Drawing.Font("NeueHaasGroteskText Pro", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lbPort.Location = new System.Drawing.Point(125, 109);
                this.lbPort.Name = "lbPort";
                this.lbPort.Size = new System.Drawing.Size(35, 15);
                this.lbPort.TabIndex = 19;
                this.lbPort.Text = "Port:";
                this.lbPort.Visible = false;
                // 
                // btCanc
                // 
                this.btCanc.BackColor = System.Drawing.Color.White;
                this.btCanc.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.btCanc.FlatAppearance.BorderSize = 0;
                this.btCanc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btCanc.Font = new System.Drawing.Font("Verdana", 7F);
                this.btCanc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(141)))));
                this.btCanc.Location = new System.Drawing.Point(94, 187);
                this.btCanc.Name = "btCanc";
                this.btCanc.Size = new System.Drawing.Size(135, 23);
                this.btCanc.TabIndex = 18;
                this.btCanc.Text = "Cancelar";
                this.btCanc.UseVisualStyleBackColor = false;
                this.btCanc.Click += new System.EventHandler(this.btCanc_Click);
                // 
                // btOk
                // 
                this.btOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(141)))));
                this.btOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btOk.Font = new System.Drawing.Font("Verdana", 7F);
                this.btOk.ForeColor = System.Drawing.Color.White;
                this.btOk.Location = new System.Drawing.Point(76, 158);
                this.btOk.Name = "btOk";
                this.btOk.Size = new System.Drawing.Size(169, 23);
                this.btOk.TabIndex = 17;
                this.btOk.Text = "Guardar Cambios";
                this.btOk.UseVisualStyleBackColor = true;
                this.btOk.Click += new System.EventHandler(this.btOk_Click);
                // 
                // cbSpeed
                // 
                this.cbSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.cbSpeed.FormattingEnabled = true;
                this.cbSpeed.Location = new System.Drawing.Point(13, 79);
                this.cbSpeed.Name = "cbSpeed";
                this.cbSpeed.Size = new System.Drawing.Size(290, 21);
                this.cbSpeed.TabIndex = 16;
                this.cbSpeed.Visible = false;
                this.cbSpeed.SelectedIndexChanged += new System.EventHandler(this.cbSpeed_SelectedIndexChanged);
                // 
                // lbPropName
                // 
                this.lbPropName.AutoSize = true;
                this.lbPropName.Font = new System.Drawing.Font("NeueHaasGroteskText Pro", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lbPropName.Location = new System.Drawing.Point(12, 62);
                this.lbPropName.Name = "lbPropName";
                this.lbPropName.Size = new System.Drawing.Size(169, 15);
                this.lbPropName.TabIndex = 15;
                this.lbPropName.Text = "Velocidad de Comunicación:";
                this.lbPropName.Visible = false;
                // 
                // cbPortName
                // 
                this.cbPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.cbPortName.FormattingEnabled = true;
                this.cbPortName.Location = new System.Drawing.Point(13, 26);
                this.cbPortName.Name = "cbPortName";
                this.cbPortName.Size = new System.Drawing.Size(290, 21);
                this.cbPortName.TabIndex = 14;
                this.cbPortName.SelectedIndexChanged += new System.EventHandler(this.cbPortName_SelectedIndexChanged);
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Font = new System.Drawing.Font("NeueHaasGroteskText Pro", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.label2.Location = new System.Drawing.Point(12, 9);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(92, 15);
                this.label2.TabIndex = 13;
                this.label2.Text = "Puerto a Usar:";
                // 
                // TargetConfigure
                // 
                this.AcceptButton = this.btOk;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.btCanc;
                this.ClientSize = new System.Drawing.Size(316, 223);
                this.Controls.Add(this.tbIP);
                this.Controls.Add(this.btPing);
                this.Controls.Add(this.tbPort);
                this.Controls.Add(this.lbPort);
                this.Controls.Add(this.btCanc);
                this.Controls.Add(this.btOk);
                this.Controls.Add(this.cbSpeed);
                this.Controls.Add(this.lbPropName);
                this.Controls.Add(this.cbPortName);
                this.Controls.Add(this.label2);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "TargetConfigure";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text = " Configura Comunicación";
                this.Load += new System.EventHandler(this.TargetConfigure_Load);
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.TextBox tbIP;
            private System.Windows.Forms.Button btPing;
            private System.Windows.Forms.TextBox tbPort;
            private System.Windows.Forms.Label lbPort;
            private System.Windows.Forms.Button btCanc;
            private System.Windows.Forms.Button btOk;
            private System.Windows.Forms.ComboBox cbSpeed;
            private System.Windows.Forms.Label lbPropName;
            private System.Windows.Forms.ComboBox cbPortName;
            private System.Windows.Forms.Label label2;
        }
    
}