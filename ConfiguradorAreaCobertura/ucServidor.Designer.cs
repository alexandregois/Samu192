namespace ConfiguradorAreaCobertura
{
    partial class ucServidor
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
            this.txtEndereço = new System.Windows.Forms.TextBox();
            this.lblEndereco = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.txtCodServidor = new System.Windows.Forms.TextBox();
            this.lblCodServidor = new System.Windows.Forms.Label();
            this.pnBotoesServidores = new System.Windows.Forms.Panel();
            this.pbRemoverServidor = new System.Windows.Forms.Button();
            this.pbAlterarServidor = new System.Windows.Forms.Button();
            this.pbInserirServidor = new System.Windows.Forms.Button();
            this.lbServidores = new System.Windows.Forms.ListBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.pbRegostroServidores.SuspendLayout();
            this.pnBotoesServidores.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbRegostroServidores
            // 
            this.pbRegostroServidores.Controls.Add(this.txtSenha);
            this.pbRegostroServidores.Controls.Add(this.txtUsuario);
            this.pbRegostroServidores.Controls.Add(this.txtEndereço);
            this.pbRegostroServidores.Controls.Add(this.lblSenha);
            this.pbRegostroServidores.Controls.Add(this.lblUsuario);
            this.pbRegostroServidores.Controls.Add(this.lblEndereco);
            this.pbRegostroServidores.Controls.Add(this.txtNome);
            this.pbRegostroServidores.Controls.Add(this.lblNome);
            this.pbRegostroServidores.Controls.Add(this.txtCodServidor);
            this.pbRegostroServidores.Controls.Add(this.lblCodServidor);
            this.pbRegostroServidores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRegostroServidores.Location = new System.Drawing.Point(200, 0);
            this.pbRegostroServidores.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbRegostroServidores.Name = "pbRegostroServidores";
            this.pbRegostroServidores.Size = new System.Drawing.Size(291, 609);
            this.pbRegostroServidores.TabIndex = 5;
            // 
            // txtEndereço
            // 
            this.txtEndereço.Location = new System.Drawing.Point(25, 135);
            this.txtEndereço.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtEndereço.Name = "txtEndereço";
            this.txtEndereço.Size = new System.Drawing.Size(262, 20);
            this.txtEndereço.TabIndex = 5;
            // 
            // lblEndereco
            // 
            this.lblEndereco.Location = new System.Drawing.Point(22, 111);
            this.lblEndereco.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEndereco.Name = "lblEndereco";
            this.lblEndereco.Size = new System.Drawing.Size(75, 21);
            this.lblEndereco.TabIndex = 0;
            this.lblEndereco.Text = "Endereço";
            this.lblEndereco.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(25, 91);
            this.txtNome.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(212, 20);
            this.txtNome.TabIndex = 3;
            // 
            // lblNome
            // 
            this.lblNome.Location = new System.Drawing.Point(22, 67);
            this.lblNome.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(75, 21);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Nome";
            this.lblNome.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtCodServidor
            // 
            this.txtCodServidor.Location = new System.Drawing.Point(25, 47);
            this.txtCodServidor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCodServidor.Name = "txtCodServidor";
            this.txtCodServidor.Size = new System.Drawing.Size(76, 20);
            this.txtCodServidor.TabIndex = 1;
            // 
            // lblCodServidor
            // 
            this.lblCodServidor.Location = new System.Drawing.Point(22, 24);
            this.lblCodServidor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCodServidor.Name = "lblCodServidor";
            this.lblCodServidor.Size = new System.Drawing.Size(75, 21);
            this.lblCodServidor.TabIndex = 0;
            this.lblCodServidor.Text = "Cod";
            this.lblCodServidor.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnBotoesServidores
            // 
            this.pnBotoesServidores.Controls.Add(this.pbRemoverServidor);
            this.pnBotoesServidores.Controls.Add(this.pbAlterarServidor);
            this.pnBotoesServidores.Controls.Add(this.pbInserirServidor);
            this.pnBotoesServidores.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnBotoesServidores.Location = new System.Drawing.Point(491, 0);
            this.pnBotoesServidores.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnBotoesServidores.Name = "pnBotoesServidores";
            this.pnBotoesServidores.Size = new System.Drawing.Size(93, 609);
            this.pnBotoesServidores.TabIndex = 4;
            // 
            // pbRemoverServidor
            // 
            this.pbRemoverServidor.Location = new System.Drawing.Point(17, 64);
            this.pbRemoverServidor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbRemoverServidor.Name = "pbRemoverServidor";
            this.pbRemoverServidor.Size = new System.Drawing.Size(56, 19);
            this.pbRemoverServidor.TabIndex = 0;
            this.pbRemoverServidor.Text = "Remover";
            this.pbRemoverServidor.UseVisualStyleBackColor = true;
            this.pbRemoverServidor.Click += new System.EventHandler(this.pbRemoverServidor_Click);
            // 
            // pbAlterarServidor
            // 
            this.pbAlterarServidor.Location = new System.Drawing.Point(17, 41);
            this.pbAlterarServidor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbAlterarServidor.Name = "pbAlterarServidor";
            this.pbAlterarServidor.Size = new System.Drawing.Size(56, 19);
            this.pbAlterarServidor.TabIndex = 0;
            this.pbAlterarServidor.Text = "Alterar";
            this.pbAlterarServidor.UseVisualStyleBackColor = true;
            this.pbAlterarServidor.Click += new System.EventHandler(this.pbAlterarServidor_Click);
            // 
            // pbInserirServidor
            // 
            this.pbInserirServidor.Location = new System.Drawing.Point(17, 17);
            this.pbInserirServidor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbInserirServidor.Name = "pbInserirServidor";
            this.pbInserirServidor.Size = new System.Drawing.Size(56, 19);
            this.pbInserirServidor.TabIndex = 0;
            this.pbInserirServidor.Text = "Inserir";
            this.pbInserirServidor.UseVisualStyleBackColor = true;
            this.pbInserirServidor.Click += new System.EventHandler(this.pbInserirServidor_Click);
            // 
            // lbServidores
            // 
            this.lbServidores.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbServidores.FormattingEnabled = true;
            this.lbServidores.Location = new System.Drawing.Point(0, 0);
            this.lbServidores.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lbServidores.Name = "lbServidores";
            this.lbServidores.Size = new System.Drawing.Size(200, 609);
            this.lbServidores.TabIndex = 3;
            this.lbServidores.SelectedIndexChanged += new System.EventHandler(this.lbServidores_SelectedIndexChanged);
            // 
            // lblUsuario
            // 
            this.lblUsuario.Location = new System.Drawing.Point(22, 157);
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(75, 21);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuário";
            this.lblUsuario.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(25, 181);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(262, 20);
            this.txtUsuario.TabIndex = 5;
            // 
            // lblSenha
            // 
            this.lblSenha.Location = new System.Drawing.Point(22, 203);
            this.lblSenha.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(75, 21);
            this.lblSenha.TabIndex = 0;
            this.lblSenha.Text = "Senha";
            this.lblSenha.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(25, 227);
            this.txtSenha.Margin = new System.Windows.Forms.Padding(2);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(262, 20);
            this.txtSenha.TabIndex = 5;
            // 
            // ucServidor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbRegostroServidores);
            this.Controls.Add(this.pnBotoesServidores);
            this.Controls.Add(this.lbServidores);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ucServidor";
            this.Size = new System.Drawing.Size(584, 609);
            this.pbRegostroServidores.ResumeLayout(false);
            this.pbRegostroServidores.PerformLayout();
            this.pnBotoesServidores.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pbRegostroServidores;
        private System.Windows.Forms.TextBox txtEndereço;
        private System.Windows.Forms.Label lblEndereco;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtCodServidor;
        private System.Windows.Forms.Label lblCodServidor;
        private System.Windows.Forms.Panel pnBotoesServidores;
        private System.Windows.Forms.Button pbRemoverServidor;
        private System.Windows.Forms.Button pbAlterarServidor;
        private System.Windows.Forms.Button pbInserirServidor;
        private System.Windows.Forms.ListBox lbServidores;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.Label lblUsuario;
    }
}
