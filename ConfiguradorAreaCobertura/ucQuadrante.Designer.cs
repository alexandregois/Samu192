using System;

namespace ConfiguradorAreaCobertura
{
    partial class ucQuadrante
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbRegostroServidores = new System.Windows.Forms.Panel();
            this.txtAreas = new System.Windows.Forms.TextBox();
            this.lblAreas = new System.Windows.Forms.Label();
            this.txtCod = new System.Windows.Forms.TextBox();
            this.lblCodServidor = new System.Windows.Forms.Label();
            this.pnBotoesServidores = new System.Windows.Forms.Panel();
            this.lbQuadrantes = new System.Windows.Forms.ListBox();
            this.pbRegostroServidores.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbRegostroServidores
            // 
            this.pbRegostroServidores.Controls.Add(this.txtAreas);
            this.pbRegostroServidores.Controls.Add(this.lblAreas);
            this.pbRegostroServidores.Controls.Add(this.txtCod);
            this.pbRegostroServidores.Controls.Add(this.lblCodServidor);
            this.pbRegostroServidores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRegostroServidores.Location = new System.Drawing.Point(265, 0);
            this.pbRegostroServidores.Name = "pbRegostroServidores";
            this.pbRegostroServidores.Size = new System.Drawing.Size(389, 749);
            this.pbRegostroServidores.TabIndex = 5;
            // 
            // txtAreas
            // 
            this.txtAreas.Location = new System.Drawing.Point(33, 112);
            this.txtAreas.Name = "txtAreas";
            this.txtAreas.ReadOnly = true;
            this.txtAreas.Size = new System.Drawing.Size(282, 22);
            this.txtAreas.TabIndex = 3;
            // 
            // lblAreas
            // 
            this.lblAreas.Location = new System.Drawing.Point(30, 83);
            this.lblAreas.Name = "lblAreas";
            this.lblAreas.Size = new System.Drawing.Size(100, 26);
            this.lblAreas.TabIndex = 0;
            this.lblAreas.Text = "Areas";
            this.lblAreas.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtCodServidor
            // 
            this.txtCod.Location = new System.Drawing.Point(33, 58);
            this.txtCod.Name = "txtCodServidor";
            this.txtCod.Size = new System.Drawing.Size(100, 22);
            this.txtCod.TabIndex = 1;
            // 
            // lblCodServidor
            // 
            this.lblCodServidor.Location = new System.Drawing.Point(30, 29);
            this.lblCodServidor.Name = "lblCodServidor";
            this.lblCodServidor.Size = new System.Drawing.Size(100, 26);
            this.lblCodServidor.TabIndex = 0;
            this.lblCodServidor.Text = "Cod";
            this.lblCodServidor.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnBotoesServidores
            // 
            this.pnBotoesServidores.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnBotoesServidores.Location = new System.Drawing.Point(654, 0);
            this.pnBotoesServidores.Name = "pnBotoesServidores";
            this.pnBotoesServidores.Size = new System.Drawing.Size(124, 749);
            this.pnBotoesServidores.TabIndex = 4;
            // 
            // lbQuadrantes
            // 
            this.lbQuadrantes.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbQuadrantes.FormattingEnabled = true;
            this.lbQuadrantes.ItemHeight = 16;
            this.lbQuadrantes.Location = new System.Drawing.Point(0, 0);
            this.lbQuadrantes.Name = "lbQuadrantes";
            this.lbQuadrantes.Size = new System.Drawing.Size(265, 749);
            this.lbQuadrantes.TabIndex = 3;
            this.lbQuadrantes.SelectedIndexChanged += new System.EventHandler(this.lbQuadrantes_SelectedIndexChanged);
            // 
            // ucQuadrante
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbRegostroServidores);
            this.Controls.Add(this.pnBotoesServidores);
            this.Controls.Add(this.lbQuadrantes);
            this.Name = "ucQuadrante";
            this.Size = new System.Drawing.Size(778, 749);
            this.pbRegostroServidores.ResumeLayout(false);
            this.pbRegostroServidores.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pbRegostroServidores;
        private System.Windows.Forms.TextBox txtAreas;
        private System.Windows.Forms.Label lblAreas;
        private System.Windows.Forms.TextBox txtCod;
        private System.Windows.Forms.Label lblCodServidor;
        private System.Windows.Forms.Panel pnBotoesServidores;
        private System.Windows.Forms.ListBox lbQuadrantes;
    }
}
