using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using ConfiguradorAreaCobertura.Events;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using ConfiguradorAreaCobertura.InterfacesImpl;

namespace ConfiguradorAreaCobertura
{
    public partial class Form1 : Form
    {
        public GMapControl mapa;
        List<AreaDTO> areas;
        List<ServidorDTO> servidores;
        List<QuadranteDTO> quadrantes;

        public Form1()
        {
            InitializeComponent();
            splitMapa.SplitterDistance = 350;
            splitCadastros.SplitterDistance = 400;
            splitQuadrantes.SplitterDistance = 400;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InicializaDados();
            InicializaControles();
            ImportaDados();
        }

        private void InicializaControles()
        {
            ucServidor1.ServidorSelecionado += servidorSelecionado;
            ucArea1.AreaSelecionada += areaSelecionada;
            ucQuadrante1.QuadranteSelecionado += quadranteSelecionado;
            InicializaMapa();
        }

        private void InicializaDados()
        {
            FacadeLocalizacao.Inicializar();

            servidores = FacadeLocalizacao.getServidores();
            areas = FacadeLocalizacao.getAreas();
            quadrantes = FacadeLocalizacao.getQuadrantes();
        }

        private void ImportaDados()
        {
            servidores = FacadeLocalizacao.getServidores();
            areas = FacadeLocalizacao.getAreas();
            quadrantes = FacadeLocalizacao.getQuadrantes();

            ucServidor1.servidores = servidores;
            ucServidor1.AtualizaDados();

            ucArea1.areas = areas;
            ucArea1.AtualizaDados();

            if (quadrantes == null)
                quadrantes = new List<QuadranteDTO>();
            ucQuadrante1.quadrantes = quadrantes;
            ucQuadrante1.AtualizaDados();
        }
        private void ExportaDados()
        {
            FacadeLocalizacao.setServidores(servidores);
            FacadeLocalizacao.setAreas(areas);
            FacadeLocalizacao.setQuadrantes(quadrantes);
        }

        private void MostraTodosServidores()
        {
            LimpaCamada(PegaCamada("quadrantes"));
            GMapOverlay camada = PegaCamada("areas");
            LimpaCamada(camada);
            servidores.ForEach(y=>areas.Where(x => x.CodServidor == y.Cod).ToList().ForEach(x => DesenhaArea(x, camada, y.Cod)));
        }

        private void MostraTodasAreas()
        {
            LimpaCamada(PegaCamada("quadrantes"));
            GMapOverlay camada = PegaCamada("areas");
            LimpaCamada(camada);
            areas.ForEach(x => DesenhaArea(x, camada));
        }

        private void MostraTodosQuadrantes()
        {
            LimpaCamada(PegaCamada("areas"));
            GMapOverlay camada = PegaCamada("quadrantes");
            LimpaCamada(camada);
            quadrantes.ForEach(x => DesenhaQuadrante(x, camada));
        }

        #region Eventos Diretos de Interface
        private void pbExibirServidores_Click(object sender, EventArgs e)
        {
            try { 
            MostraTodosServidores();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void pbExibirAreas_Click(object sender, EventArgs e)
        {
            try { 
            MostraTodasAreas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void pbQuadrantes_Click(object sender, EventArgs e)
        {
            try
            {
                MostraTodosQuadrantes();
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void pbGerarQuadrantes_Click(object sender, EventArgs e)
        {
            try
            {
                ExportaDados();
                FacadeLocalizacao.GeraQuadrantes(new GeometriaClipper(), (int)txtMaxNiveis.Value, (int)txtMinAreas.Value);
                ImportaDados();
                MostraTodosQuadrantes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Eventos de Seleção

        private void areaSelecionada(object sender, AreaEventArgs e)
        {
            LimpaCamada(PegaCamada("areas"));
            if (e.Area != null)
                DesenhaArea(e.Area, PegaCamada("areas"));
        }

        private void servidorSelecionado(object sender, ServidorEventArgs e)
        {
            LimpaCamada(PegaCamada("quadrantes"));
            GMapOverlay camada = PegaCamada("areas");
            LimpaCamada(camada);
            if (e.Servidor != null)
                areas.Where(x => x.CodServidor == e.Servidor.Cod).ToList().ForEach(x => DesenhaArea(x, camada));
        }

        private void quadranteSelecionado(object sender, QuadranteEventArgs e)
        {
            GMapOverlay camada = PegaCamada("areas");
            LimpaCamada(camada);
            if (e.Quadrante != null)
            {
                Action<QuadranteDTO> traverse = null;
                traverse = (x) => {
                    x.Areas.ForEach(z => DesenhaArea(areas.Where(y => y.Cod == z).FirstOrDefault(), camada));
                    quadrantes.Where(q => q.CodPai == x.Cod).ToList().ForEach(traverse);
                };
                traverse(e.Quadrante);
            }
            camada = PegaCamada("quadrantes");
            LimpaCamada(camada);
            DesenhaQuadrante(e.Quadrante, camada);
        }

        #endregion


        #region Imp/Exp
        private void pbImportarCSV_Click(object sender, EventArgs e)
        {

            try
            {
                using (FolderBrowserDialog frm = new FolderBrowserDialog())
                {
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(frm.SelectedPath))
                    {
                        LeitorCSVImpl leitor = new LeitorCSVImpl(frm.SelectedPath);
                        FacadeLocalizacao.Carregar(leitor);
                        ImportaDados();
                        MostraTodosServidores();
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void pbExportarCSV_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog frm = new FolderBrowserDialog())
                {
                    DialogResult result = frm.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(frm.SelectedPath))
                    {
                        ExportaDados();
                        LeitorCSVImpl leitor = new LeitorCSVImpl(frm.SelectedPath);
                        FacadeLocalizacao.Gravar(leitor);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Mapa

        private void InicializaMapa()
        {
            mapa = new GMapControl();
            this.splitMapa.Panel2.Controls.Add(mapa);
            mapa.Dock = DockStyle.Fill;
            GMapProvider.UserAgent = "TrueMaps";
            GMapProvider.Language = LanguageType.PortugueseBrazil;
            mapa.CanDragMap = true;
            mapa.MapProvider = GMapProviders.OpenStreetMap;
            mapa.Position = new PointLatLng(-30.05, -51.1);
            mapa.MinZoom = 1;
            mapa.MaxZoom = 18;
            mapa.DisableFocusOnMouseEnter = true;
            mapa.Bearing = 0;
            mapa.EmptyTileColor = System.Drawing.Color.Navy;
            mapa.GrayScaleMode = false;
            mapa.HelperLineOption = HelperLineOptions.DontShow;
            mapa.LevelsKeepInMemmory = 5;
            mapa.Location = new System.Drawing.Point(118, 133);
            mapa.MarkersEnabled = true;
            mapa.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            mapa.Name = "gmap";
            mapa.NegativeMode = false;
            mapa.PolygonsEnabled = true;
            mapa.RetryLoadTile = 0;
            mapa.RoutesEnabled = true;
            mapa.ScaleMode = ScaleModes.Integer;
            mapa.SelectedAreaFillColor = System.Drawing.Color.FromArgb(33, 65, 105, 225);
            mapa.ShowTileGridLines = false;
            mapa.Size = new System.Drawing.Size(150, 150);
            mapa.TabIndex = 0;
            mapa.Zoom = 8.0;
            mapa.DragButton = MouseButtons.Left;
            mapa.MouseDoubleClick += Mapa_MouseDoubleClick;
        }

        private void Mapa_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                //Coordenada pt = new Coordenada(1, 1);
                PointLatLng pt = mapa.FromLocalToLatLng(e.X, e.Y);
                DateTime inicio = DateTime.Now;
                List<ServidorDTO> lista = FacadeLocalizacao.Localizar(new CoordenadaDTO(pt.Lat, pt.Lng));
                TimeSpan ts = DateTime.Now.Subtract(inicio);
                if (lista.Count > 0)
                {
                    lblHitTest.Text = string.Format("{1}{0}{2}{0}Servidores: {3}{0}{4}", Environment.NewLine, pt.Lat.ToString(), pt.Lng.ToString(), string.Join(Environment.NewLine, lista.Select(x=>x.Nome)), ts.TotalMilliseconds.ToString());
                }
                else
                    lblHitTest.Text = string.Format("{1}{0}{2}{0}Fora de cobertura!{0}{3}", Environment.NewLine, pt.Lat.ToString(), pt.Lng.ToString(), ts.TotalMilliseconds.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public GMapOverlay PegaCamada(string nome)
        {
            GMapOverlay overlay = (mapa.Overlays.Where(x => x.Id == nome)).FirstOrDefault();
            if (overlay == null)
            {
                overlay = new GMapOverlay(nome);
                mapa.Overlays.Add(overlay);
            }
            return overlay;
        }

        public void LimpaCamada(GMapOverlay camada)
        {
            camada.Polygons.ToList().ForEach(x => x.Dispose());
            camada.Polygons.Clear();
        }

        public void DesenhaArea(AreaDTO area, GMapOverlay camada, int? cor = null)
        {

            if (area != null)
            {
                List<PointLatLng> pontos = area.getPoligono().getPontos().Select(x => new PointLatLng(x.Latitude, x.Longitude)).ToList();

                GMapPolygon gpol = (camada.Polygons.Where(x => x.Name == area.Cod.ToString())).FirstOrDefault();
                if (gpol != null)
                {
                    camada.Polygons.Remove(gpol);
                    gpol.Dispose();
                }
                gpol = new GMapPolygon(pontos, area.Cod.ToString());
                gpol.IsVisible = true;
                gpol.Stroke = new Pen(DefineCor(cor.GetValueOrDefault(camada.Polygons.Count)));
                gpol.Fill = new SolidBrush(Color.FromArgb(50, gpol.Stroke.Color));
                camada.Polygons.Add(gpol);

                if (chkAreasExibirLimites.Checked)
                {
                    pontos = area.Limites.getPontos().Select(x => new PointLatLng(x.Latitude, x.Longitude)).ToList();
                    gpol = new GMapPolygon(pontos, area.Cod.ToString() + "Q");
                    gpol.IsVisible = true;
                    gpol.Stroke = new Pen(DefineCor(camada.Polygons.Count));
                    gpol.Fill = new SolidBrush(Color.FromArgb(100, gpol.Stroke.Color));
                    camada.Polygons.Add(gpol);
                }
            }

        }

        public void DesenhaQuadrante(QuadranteDTO quadrante, GMapOverlay camada)
        {

            if (quadrante != null)
            {
                List<PointLatLng> pontos = quadrante.Poligono.getPontos().Select(x => new PointLatLng(x.Latitude, x.Longitude)).ToList();

                GMapPolygon gpol = (camada.Polygons.Where(x => x.Name == quadrante.Cod.ToString())).FirstOrDefault();
                if (gpol != null)
                {
                    camada.Polygons.Remove(gpol);
                    gpol.Dispose();
                }
                gpol = new GMapPolygon(pontos, quadrante.Cod.ToString());
                gpol.IsVisible = true;
                gpol.Stroke = new Pen(DefineCor(camada.Polygons.Count));
                gpol.Fill = new SolidBrush(Color.FromArgb(50, gpol.Stroke.Color));
                //camada.Markers.Add( new GMap.NET.WindowsForms.Markers.GMarkerCross(new PointLatLng(
                //    quadrante.Ponto1.Latitude + (quadrante.Ponto2.Latitude - quadrante.Ponto1.Latitude)/2,
                //    quadrante.Ponto1.Longitude + (quadrante.Ponto2.Longitude - quadrante.Ponto1.Longitude) / 2)));
                camada.Polygons.Add(gpol);
                
                
            }

        }


        private Color[] cores = new Color[] { Color.Red, Color.Lime, Color.Magenta, Color.Gold,
            Color.Blue, Color.Purple, Color.DarkOrange, Color.Cyan, Color.Chocolate,  Color.LightGreen,
            Color.SeaGreen, Color.Plum, Color.Green, Color.SteelBlue, Color.Gray, Color.Violet, Color.LightSeaGreen,
            Color.Maroon, Color.Indigo, Color.LimeGreen, Color.Firebrick, Color.Teal };
        private Color DefineCor(int num)
        {
            return cores[num % cores.Length];
        }

        #endregion

        private void Test()
        {
            List<CoordenadaDTO> lista = new List<CoordenadaDTO>();

            lista.Add(new CoordenadaDTO(-30.00064225334523, -51.15429380060592));
            lista.Add(new CoordenadaDTO(-30.00065278744093, -51.15329682757646));
            lista.Add(new CoordenadaDTO(-30.00005265838885, -51.15335542670763));
            lista.Add(new CoordenadaDTO(-30.00008675439951, -51.15429326041998));

            PoligonoDTO poligono = new PoligonoDTO(lista);

            CoordenadaDTO pista = new CoordenadaDTO(-29.99684143208941, -51.15451824425162);
            CoordenadaDTO vizinho = new CoordenadaDTO(-29.99960750867355, -51.15388065386765);
            CoordenadaDTO rei = new CoordenadaDTO(-30.0004474526529, -51.15347546219294);

            bool dentro = FacadeLocalizacao.PoligonoContem(poligono, pista);
            dentro = FacadeLocalizacao.PoligonoContem(poligono, vizinho);
            dentro = FacadeLocalizacao.PoligonoContem(poligono, rei);

            bool perto = FacadeLocalizacao.PoligonoProximo(poligono, pista);
            perto = FacadeLocalizacao.PoligonoProximo(poligono, vizinho);
            perto = FacadeLocalizacao.PoligonoProximo(poligono, rei);

        }

    }
}
