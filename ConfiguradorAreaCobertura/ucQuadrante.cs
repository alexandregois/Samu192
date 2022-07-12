using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SAMU192Core.DTO;
using ConfiguradorAreaCobertura.Events;

namespace ConfiguradorAreaCobertura
{
    public partial class ucQuadrante : UserControl
    {
        public List<QuadranteDTO> quadrantes;

        public delegate void QuadranteEventHandler(object sender, QuadranteEventArgs e);
        public event QuadranteEventHandler QuadranteSelecionado;

        public ucQuadrante()
        {
            InitializeComponent();
        }

        public void AtualizaDados()
        {
            lbQuadrantes.DataSource = new BindingList<QuadranteDTO>(quadrantes);
        }

        private void ExibeQuadrante()
        {
            QuadranteDTO quadrante = (QuadranteDTO)lbQuadrantes.SelectedItem;
            if (quadrante != null)
            {
                txtCod.Text = quadrante.Cod.ToString();
                txtAreas.Text = string.Join(",", quadrante.Areas.Select(x => x.ToString()).ToArray());
            }
            else
            {
                txtCod.Clear();
                txtAreas.Clear();
            }
        }

        private void lbQuadrantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExibeQuadrante();
            OnQuadranteSelecionado(new QuadranteEventArgs((QuadranteDTO)lbQuadrantes.SelectedItem));
        }

        protected virtual void OnQuadranteSelecionado(QuadranteEventArgs e)
        {
            QuadranteSelecionado?.Invoke(this, e);
        }

    }
}
