namespace ConfiguradorAreaCobertura
{
    partial class Form1
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
            this.splitMapa = new System.Windows.Forms.SplitContainer();
            this.pnBotoesMapa = new System.Windows.Forms.Panel();
            this.pbGerarQuadrantes = new System.Windows.Forms.Button();
            this.pbExportarCSV = new System.Windows.Forms.Button();
            this.pbImportarCSV = new System.Windows.Forms.Button();
            this.lblHitTest = new System.Windows.Forms.Label();
            this.lblMaxNiveis = new System.Windows.Forms.Label();
            this.lblMinAreas = new System.Windows.Forms.Label();
            this.txtMinAreas = new System.Windows.Forms.NumericUpDown();
            this.txtMaxNiveis = new System.Windows.Forms.NumericUpDown();
            this.splitCadastros = new System.Windows.Forms.SplitContainer();
            this.ucServidor1 = new ConfiguradorAreaCobertura.ucServidor();
            this.splitQuadrantes = new System.Windows.Forms.SplitContainer();
            this.chkAreasExibirLimites = new System.Windows.Forms.CheckBox();
            this.ucArea1 = new ConfiguradorAreaCobertura.ucArea();
            this.ucQuadrante1 = new ConfiguradorAreaCobertura.ucQuadrante();
            this.pbExibirServidores = new System.Windows.Forms.Button();
            this.pbExibirAreas = new System.Windows.Forms.Button();
            this.pbQuadrantes = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitMapa)).BeginInit();
            this.splitMapa.Panel1.SuspendLayout();
            this.splitMapa.Panel2.SuspendLayout();
            this.splitMapa.SuspendLayout();
            this.pnBotoesMapa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinAreas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxNiveis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCadastros)).BeginInit();
            this.splitCadastros.Panel1.SuspendLayout();
            this.splitCadastros.Panel2.SuspendLayout();
            this.splitCadastros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitQuadrantes)).BeginInit();
            this.splitQuadrantes.Panel1.SuspendLayout();
            this.splitQuadrantes.Panel2.SuspendLayout();
            this.splitQuadrantes.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMapa
            // 
            this.splitMapa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMapa.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitMapa.Location = new System.Drawing.Point(0, 0);
            this.splitMapa.Margin = new System.Windows.Forms.Padding(2);
            this.splitMapa.Name = "splitMapa";
            this.splitMapa.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMapa.Panel1
            // 
            this.splitMapa.Panel1.Controls.Add(this.splitCadastros);
            this.splitMapa.Panel1MinSize = 250;
            // 
            // splitMapa.Panel2
            // 
            this.splitMapa.Panel2.Controls.Add(this.pbQuadrantes);
            this.splitMapa.Panel2.Controls.Add(this.pbExibirAreas);
            this.splitMapa.Panel2.Controls.Add(this.pbExibirServidores);
            this.splitMapa.Panel2.Controls.Add(this.pnBotoesMapa);
            this.splitMapa.Panel2MinSize = 250;
            this.splitMapa.Size = new System.Drawing.Size(1267, 862);
            this.splitMapa.SplitterDistance = 284;
            this.splitMapa.SplitterWidth = 6;
            this.splitMapa.TabIndex = 0;
            // 
            // pnBotoesMapa
            // 
            this.pnBotoesMapa.Controls.Add(this.txtMaxNiveis);
            this.pnBotoesMapa.Controls.Add(this.txtMinAreas);
            this.pnBotoesMapa.Controls.Add(this.lblMinAreas);
            this.pnBotoesMapa.Controls.Add(this.lblMaxNiveis);
            this.pnBotoesMapa.Controls.Add(this.lblHitTest);
            this.pnBotoesMapa.Controls.Add(this.pbGerarQuadrantes);
            this.pnBotoesMapa.Controls.Add(this.pbExportarCSV);
            this.pnBotoesMapa.Controls.Add(this.pbImportarCSV);
            this.pnBotoesMapa.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnBotoesMapa.Location = new System.Drawing.Point(1117, 0);
            this.pnBotoesMapa.Margin = new System.Windows.Forms.Padding(2);
            this.pnBotoesMapa.Name = "pnBotoesMapa";
            this.pnBotoesMapa.Size = new System.Drawing.Size(150, 572);
            this.pnBotoesMapa.TabIndex = 0;
            // 
            // pbGerarQuadrantes
            // 
            this.pbGerarQuadrantes.Location = new System.Drawing.Point(20, 214);
            this.pbGerarQuadrantes.Margin = new System.Windows.Forms.Padding(2);
            this.pbGerarQuadrantes.Name = "pbGerarQuadrantes";
            this.pbGerarQuadrantes.Size = new System.Drawing.Size(99, 34);
            this.pbGerarQuadrantes.TabIndex = 1;
            this.pbGerarQuadrantes.Text = "Gerar Quadrantes";
            this.pbGerarQuadrantes.UseVisualStyleBackColor = true;
            this.pbGerarQuadrantes.Click += new System.EventHandler(this.pbGerarQuadrantes_Click);
            // 
            // pbExportarCSV
            // 
            this.pbExportarCSV.Location = new System.Drawing.Point(20, 58);
            this.pbExportarCSV.Margin = new System.Windows.Forms.Padding(2);
            this.pbExportarCSV.Name = "pbExportarCSV";
            this.pbExportarCSV.Size = new System.Drawing.Size(99, 34);
            this.pbExportarCSV.TabIndex = 0;
            this.pbExportarCSV.Text = "Exportar CSV";
            this.pbExportarCSV.UseVisualStyleBackColor = true;
            this.pbExportarCSV.Click += new System.EventHandler(this.pbExportarCSV_Click);
            // 
            // pbImportarCSV
            // 
            this.pbImportarCSV.Location = new System.Drawing.Point(20, 20);
            this.pbImportarCSV.Margin = new System.Windows.Forms.Padding(2);
            this.pbImportarCSV.Name = "pbImportarCSV";
            this.pbImportarCSV.Size = new System.Drawing.Size(99, 34);
            this.pbImportarCSV.TabIndex = 0;
            this.pbImportarCSV.Text = "Importar CSV";
            this.pbImportarCSV.UseVisualStyleBackColor = true;
            this.pbImportarCSV.Click += new System.EventHandler(this.pbImportarCSV_Click);
            // 
            // lblHitTest
            // 
            this.lblHitTest.Location = new System.Drawing.Point(3, 270);
            this.lblHitTest.Name = "lblHitTest";
            this.lblHitTest.Size = new System.Drawing.Size(144, 145);
            this.lblHitTest.TabIndex = 2;
            this.lblHitTest.Text = "Duplo clique sobre o mapa para testar o local";
            // 
            // lblMaxNiveis
            // 
            this.lblMaxNiveis.Location = new System.Drawing.Point(19, 112);
            this.lblMaxNiveis.Name = "lblMaxNiveis";
            this.lblMaxNiveis.Size = new System.Drawing.Size(100, 19);
            this.lblMaxNiveis.TabIndex = 3;
            this.lblMaxNiveis.Text = "Max. Níveis";
            this.lblMaxNiveis.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblMinAreas
            // 
            this.lblMinAreas.Location = new System.Drawing.Point(19, 157);
            this.lblMinAreas.Name = "lblMinAreas";
            this.lblMinAreas.Size = new System.Drawing.Size(100, 19);
            this.lblMinAreas.TabIndex = 3;
            this.lblMinAreas.Text = "Min. Áreas";
            this.lblMinAreas.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtMinAreas
            // 
            this.txtMinAreas.Location = new System.Drawing.Point(22, 179);
            this.txtMinAreas.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtMinAreas.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMinAreas.Name = "txtMinAreas";
            this.txtMinAreas.Size = new System.Drawing.Size(99, 20);
            this.txtMinAreas.TabIndex = 4;
            this.txtMinAreas.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // txtMaxNiveis
            // 
            this.txtMaxNiveis.Location = new System.Drawing.Point(22, 134);
            this.txtMaxNiveis.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.txtMaxNiveis.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMaxNiveis.Name = "txtMaxNiveis";
            this.txtMaxNiveis.Size = new System.Drawing.Size(99, 20);
            this.txtMaxNiveis.TabIndex = 4;
            this.txtMaxNiveis.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // splitCadastros
            // 
            this.splitCadastros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCadastros.Location = new System.Drawing.Point(0, 0);
            this.splitCadastros.Margin = new System.Windows.Forms.Padding(2);
            this.splitCadastros.Name = "splitCadastros";
            // 
            // splitCadastros.Panel1
            // 
            this.splitCadastros.Panel1.Controls.Add(this.ucServidor1);
            this.splitCadastros.Panel1MinSize = 250;
            // 
            // splitCadastros.Panel2
            // 
            this.splitCadastros.Panel2.Controls.Add(this.splitQuadrantes);
            this.splitCadastros.Panel2MinSize = 250;
            this.splitCadastros.Size = new System.Drawing.Size(1267, 284);
            this.splitCadastros.SplitterDistance = 603;
            this.splitCadastros.SplitterWidth = 6;
            this.splitCadastros.TabIndex = 0;
            // 
            // ucServidor1
            // 
            this.ucServidor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucServidor1.Location = new System.Drawing.Point(0, 0);
            this.ucServidor1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ucServidor1.Name = "ucServidor1";
            this.ucServidor1.Size = new System.Drawing.Size(603, 284);
            this.ucServidor1.TabIndex = 0;
            // 
            // splitQuadrantes
            // 
            this.splitQuadrantes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitQuadrantes.Location = new System.Drawing.Point(0, 0);
            this.splitQuadrantes.Margin = new System.Windows.Forms.Padding(2);
            this.splitQuadrantes.Name = "splitQuadrantes";
            // 
            // splitQuadrantes.Panel1
            // 
            this.splitQuadrantes.Panel1.Controls.Add(this.chkAreasExibirLimites);
            this.splitQuadrantes.Panel1.Controls.Add(this.ucArea1);
            this.splitQuadrantes.Panel1MinSize = 250;
            // 
            // splitQuadrantes.Panel2
            // 
            this.splitQuadrantes.Panel2.Controls.Add(this.ucQuadrante1);
            this.splitQuadrantes.Panel2MinSize = 250;
            this.splitQuadrantes.Size = new System.Drawing.Size(658, 284);
            this.splitQuadrantes.SplitterDistance = 402;
            this.splitQuadrantes.SplitterWidth = 6;
            this.splitQuadrantes.TabIndex = 1;
            // 
            // chkAreasExibirLimites
            // 
            this.chkAreasExibirLimites.Location = new System.Drawing.Point(224, 220);
            this.chkAreasExibirLimites.Margin = new System.Windows.Forms.Padding(2);
            this.chkAreasExibirLimites.Name = "chkAreasExibirLimites";
            this.chkAreasExibirLimites.Size = new System.Drawing.Size(138, 20);
            this.chkAreasExibirLimites.TabIndex = 1;
            this.chkAreasExibirLimites.Text = "Exibir Limites";
            this.chkAreasExibirLimites.UseVisualStyleBackColor = true;
            // 
            // ucArea1
            // 
            this.ucArea1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucArea1.Location = new System.Drawing.Point(0, 0);
            this.ucArea1.Margin = new System.Windows.Forms.Padding(2);
            this.ucArea1.Name = "ucArea1";
            this.ucArea1.Size = new System.Drawing.Size(402, 284);
            this.ucArea1.TabIndex = 0;
            // 
            // ucQuadrante1
            // 
            this.ucQuadrante1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucQuadrante1.Location = new System.Drawing.Point(0, 0);
            this.ucQuadrante1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ucQuadrante1.Name = "ucQuadrante1";
            this.ucQuadrante1.Size = new System.Drawing.Size(250, 284);
            this.ucQuadrante1.TabIndex = 0;
            // 
            // pbExibirServidores
            // 
            this.pbExibirServidores.Location = new System.Drawing.Point(65, 20);
            this.pbExibirServidores.Name = "pbExibirServidores";
            this.pbExibirServidores.Size = new System.Drawing.Size(94, 23);
            this.pbExibirServidores.TabIndex = 1;
            this.pbExibirServidores.Text = "Servidores";
            this.pbExibirServidores.UseVisualStyleBackColor = true;
            this.pbExibirServidores.Click += new System.EventHandler(this.pbExibirServidores_Click);
            // 
            // pbExibirAreas
            // 
            this.pbExibirAreas.Location = new System.Drawing.Point(165, 20);
            this.pbExibirAreas.Name = "pbExibirAreas";
            this.pbExibirAreas.Size = new System.Drawing.Size(94, 23);
            this.pbExibirAreas.TabIndex = 1;
            this.pbExibirAreas.Text = "Áreas";
            this.pbExibirAreas.UseVisualStyleBackColor = true;
            this.pbExibirAreas.Click += new System.EventHandler(this.pbExibirAreas_Click);
            // 
            // pbQuadrantes
            // 
            this.pbQuadrantes.Location = new System.Drawing.Point(265, 20);
            this.pbQuadrantes.Name = "pbQuadrantes";
            this.pbQuadrantes.Size = new System.Drawing.Size(94, 23);
            this.pbQuadrantes.TabIndex = 1;
            this.pbQuadrantes.Text = "Quadrantes";
            this.pbQuadrantes.UseVisualStyleBackColor = true;
            this.pbQuadrantes.Click += new System.EventHandler(this.pbQuadrantes_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 862);
            this.Controls.Add(this.splitMapa);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitMapa.Panel1.ResumeLayout(false);
            this.splitMapa.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMapa)).EndInit();
            this.splitMapa.ResumeLayout(false);
            this.pnBotoesMapa.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMinAreas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxNiveis)).EndInit();
            this.splitCadastros.Panel1.ResumeLayout(false);
            this.splitCadastros.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCadastros)).EndInit();
            this.splitCadastros.ResumeLayout(false);
            this.splitQuadrantes.Panel1.ResumeLayout(false);
            this.splitQuadrantes.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitQuadrantes)).EndInit();
            this.splitQuadrantes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMapa;
        private System.Windows.Forms.SplitContainer splitCadastros;
        private ucServidor ucServidor1;
        private ucArea ucArea1;
        private System.Windows.Forms.Panel pnBotoesMapa;
        private System.Windows.Forms.Button pbExportarCSV;
        private System.Windows.Forms.Button pbImportarCSV;
        private System.Windows.Forms.Button pbGerarQuadrantes;
        private System.Windows.Forms.SplitContainer splitQuadrantes;
        private ucQuadrante ucQuadrante1;
        private System.Windows.Forms.CheckBox chkAreasExibirLimites;
        private System.Windows.Forms.NumericUpDown txtMaxNiveis;
        private System.Windows.Forms.NumericUpDown txtMinAreas;
        private System.Windows.Forms.Label lblMinAreas;
        private System.Windows.Forms.Label lblMaxNiveis;
        private System.Windows.Forms.Label lblHitTest;
        private System.Windows.Forms.Button pbQuadrantes;
        private System.Windows.Forms.Button pbExibirAreas;
        private System.Windows.Forms.Button pbExibirServidores;
    }
}

