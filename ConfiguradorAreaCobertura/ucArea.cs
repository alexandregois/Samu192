using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using SAMU192Core.DTO;
using ConfiguradorAreaCobertura.Events;

namespace ConfiguradorAreaCobertura
{
    public partial class ucArea : UserControl
    {
        const int MIN_PONTOS = 10;

        public List<AreaDTO> areas;

        public delegate void AreaEventHandler(object sender, AreaEventArgs e);
        public event AreaEventHandler AreaSelecionada;

        public ucArea()
        {
            InitializeComponent();
        }

        public void AtualizaDados(AreaDTO selecao = null)
        {
            lbAreas.DataSource = new BindingList<AreaDTO>(areas);
            if (selecao != null)
                lbAreas.SelectedItem = selecao;
        }

        private AreaDTO CarregaArea()
        {
            AreaDTO resp = new AreaDTO();
            int aux;
            if (!int.TryParse(txtCodArea.Text, out aux))
                throw new ApplicationException(string.Format("'{0}' não é um código válido!", txtCodArea.Text));
            else
                resp.Cod = aux;
            if (!int.TryParse(txtCodServidor.Text, out aux))
                throw new ApplicationException(string.Format("'{0}' não é um código válido!", txtCodServidor.Text));
            else
                resp.CodServidor = aux;
            resp.Fonte = txtFonte.Text;

            return resp;

        }

        private void AlteraArea(AreaDTO area)
        {
            if (lbAreas.SelectedItem != null)
            {
                AreaDTO aux = (AreaDTO)lbAreas.SelectedItem;
                aux.Cod = area.Cod;
                aux.CodServidor = area.CodServidor;
                aux.Fonte = area.Fonte;
                AtualizaDados(aux);
            }
            else
                throw new ApplicationException("Nenhuma área selecionada para alteração!");
        }
        private void RemoveArea()
        {
            if (lbAreas.SelectedItem != null)
            {
                areas.Remove((AreaDTO)lbAreas.SelectedItem);
                AtualizaDados();
            }
            else
                throw new ApplicationException("Nenhuma área selecionada para remoção!");
        }

        private void ExibeArea()
        {
            AreaDTO area = (AreaDTO)lbAreas.SelectedItem;
            if (area != null)
            {
                txtCodArea.Text = area.Cod.ToString();
                txtCodServidor.Text = area.CodServidor.ToString();
                txtQtdPontos.Text = area.getPoligono().getPontos().Count.ToString();
                txtFonte.Text = area.Fonte;
            }
            else
            {
                txtCodArea.Clear();
                txtQtdPontos.Clear();
                txtCodServidor.Clear();
                txtFonte.Clear();
            }
        }

        private void lbAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExibeArea();
            OnAreaSelecionada(new AreaEventArgs((AreaDTO)lbAreas.SelectedItem));
        }

        private void pbAlterarArea_Click(object sender, EventArgs e)
        {
            try
            {
                AlteraArea(CarregaArea());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void pbRemoverArea_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveArea();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pbImportarKML_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog frm = new OpenFileDialog() { Title = "Selecione um arquivo KML:", Filter = "Arquivos kml |*.kml" };
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    KMLHandler handler = new KMLHandler();
                    handler.CarregaKml(frm.FileName);

                    int max = 1;
                    if (areas.Count > 1)
                        max = (areas.Max(x => x.Cod)) + 1;
                    foreach (PoligonoDTO p in handler.Poligonos)
                    {
                        AreaDTO area = new AreaDTO()
                        {
                            Cod = max,
                            CodServidor = 0,
                            Fonte = Path.GetFileName(frm.FileName)
                        };
                        area.setPoligono(p);
                        areas.Add(area);
                        max += 1;
                    }
                    AtualizaDados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        protected virtual void OnAreaSelecionada(AreaEventArgs e)
        {
            AreaSelecionada?.Invoke(this, e);
        }

        private void ImportarAreas()
        {
            try
            {
                OpenFileDialog frm = new OpenFileDialog() { Title = "Selecione um arquivo KML:", Filter = "Arquivos kml |*.kml" };
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    KMLHandler handler = new KMLHandler();
                    handler.CarregaKml(frm.FileName);

                    int max = 1;
                    if (areas.Count > 1)
                        max = (areas.Max(x => x.Cod)) + 1;

                    List<AreaDTO> importar = handler.Areas.Where(x => x.getPoligono().getPontos().Count >= MIN_PONTOS).ToList();

                    DialogResult dr = MessageBox.Show(string.Format("{0} áreas encontradas. Deseja selecionar o que será importado?",
                        importar.Count.ToString()), "Importação", MessageBoxButtons.YesNoCancel);


                    switch (dr)
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.Yes:
                            frmLista frmSelecao = new frmLista(importar.Select(x => (object)x).ToList());
                            if (frmSelecao.ShowDialog() != DialogResult.OK)
                                return;

                            importar = frmSelecao.ItemsSelecionados.Select(x => (AreaDTO)x).ToList();
                            break;

                    }

                    foreach (AreaDTO a in importar)
                    {
                        a.Cod = max;
                        a.CodServidor = 1;
                        areas.Add(a);
                        max += 1;
                    }
                    AtualizaDados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void pbImportarAreasKML_Click(object sender, EventArgs e)
        {
            ImportarAreas();
        }
    }
}
