namespace ConfiguradorAreaCobertura
{
    partial class frmLista
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
            this.lbLista = new System.Windows.Forms.CheckedListBox();
            this.pbOk = new System.Windows.Forms.Button();
            this.pbCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbLista
            // 
            this.lbLista.FormattingEnabled = true;
            this.lbLista.Location = new System.Drawing.Point(12, 12);
            this.lbLista.Name = "lbLista";
            this.lbLista.Size = new System.Drawing.Size(434, 469);
            this.lbLista.TabIndex = 0;
            // 
            // pbOk
            // 
            this.pbOk.Location = new System.Drawing.Point(463, 12);
            this.pbOk.Name = "pbOk";
            this.pbOk.Size = new System.Drawing.Size(75, 23);
            this.pbOk.TabIndex = 1;
            this.pbOk.Text = "Ok";
            this.pbOk.UseVisualStyleBackColor = true;
            this.pbOk.Click += new System.EventHandler(this.pbOk_Click);
            // 
            // pbCancelar
            // 
            this.pbCancelar.Location = new System.Drawing.Point(463, 50);
            this.pbCancelar.Name = "pbCancelar";
            this.pbCancelar.Size = new System.Drawing.Size(75, 23);
            this.pbCancelar.TabIndex = 2;
            this.pbCancelar.Text = "Cancelar";
            this.pbCancelar.UseVisualStyleBackColor = true;
            this.pbCancelar.Click += new System.EventHandler(this.pbCancelar_Click);
            // 
            // frmLista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 502);
            this.Controls.Add(this.pbCancelar);
            this.Controls.Add(this.pbOk);
            this.Controls.Add(this.lbLista);
            this.Name = "frmLista";
            this.Text = "frmLista";
            this.Load += new System.EventHandler(this.frmLista_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lbLista;
        private System.Windows.Forms.Button pbOk;
        private System.Windows.Forms.Button pbCancelar;
    }
}