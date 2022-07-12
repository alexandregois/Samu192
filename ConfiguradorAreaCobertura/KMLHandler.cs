using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using ClipperLib;
using SAMU192Core.DTO;

namespace ConfiguradorAreaCobertura
{
    public class KMLHandler
    {
        KmlFile _kmlData;
        private List<PoligonoDTO> poligonos;
        public List<PoligonoDTO> Poligonos
        {
            get
            {
                if (poligonos == null)
                {
                    poligonos = KmlCarregaPoligonos();
                }

                return poligonos;
            }
        }

        List<AreaDTO> areas;
        public List<AreaDTO> Areas
        {
            get
            {
                if (areas == null)
                    areas = KMLCarregaAreas();
                return areas;
            }
        }

        public KmlFile KmlData
        {
            get
            {
                return _kmlData;
            }
        }

        public void CarregaKml(string filename)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            xml.DocumentElement.RemoveAttribute("xmlns");
            xml.DocumentElement.SetAttribute("xmlns", "http://www.opengis.net/kml/2.2");
            StringWriter sw = new StringWriter();
            xml.Save(sw);
            string sKML = sw.ToString();
            sw.Close();
            InsereKML(sKML);
            poligonos = null;
        }

        private void InsereKML(string sKML)
        {
            MemoryStream MS = new MemoryStream(Encoding.UTF8.GetBytes(sKML));
            _kmlData = KmlFile.Load(MS);
            MS.Close();
        }

        private List<AreaDTO> KMLCarregaAreas()
        {
            List<AreaDTO> resp = new List<AreaDTO>();

            if (KmlData == null)
                throw new ApplicationException("Arquivo KML não inicializado!");

            foreach (Folder f in KmlData.Root.Flatten().OfType<Folder>())
            {
                if (f.Parent is Folder)
                {
                    List<List<IntPoint>> poligonos = new List<List<IntPoint>>();

                    foreach (Polygon poly in f.Flatten().OfType<Polygon>())
                    {
                        List<IntPoint> poligono = new List<IntPoint>();
                        poly.OuterBoundary.LinearRing.Coordinates.ToList().ForEach(x => poligono.Add(new IntPoint((long)(x.Latitude * Precisao), (long)(x.Longitude * Precisao))));
                        poligonos.Add(poligono);
                    }

                    poligonos = UnePoligonosClipper(poligonos);

                    foreach (List<IntPoint> x in poligonos)
                    {
                        AreaDTO area = new AreaDTO() { Fonte = f.Name };
                        area.setPoligono(ConvertePoligonoClipper(x));
                        resp.Add(area);
                    }
                }
            }

            return resp;
        }

        private List<List<IntPoint>> UnePoligonosClipper(List<List<IntPoint>> poligonos)
        {
            if (poligonos.Count > 1)
            {
                Clipper c = new Clipper();
                List<List<IntPoint>> unidos = new List<List<IntPoint>>();
                c.AddPolygons(poligonos, PolyType.ptSubject);
                c.Execute(ClipType.ctUnion, unidos, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
                return unidos;
            }
            else
                return poligonos;

        }

        const long Precisao = 10000000000L;

        private PoligonoDTO ConvertePoligonoClipper(List<IntPoint> poligono)
        {
            List<CoordenadaDTO> lista = new List<CoordenadaDTO>();
            poligono.ForEach(x => lista.Add(new CoordenadaDTO((double)x.X / (double)Precisao, (double)x.Y / (double)Precisao)));
            return new PoligonoDTO(lista);
        }

        private List<PoligonoDTO> KmlCarregaPoligonos()
        {
            List<List<IntPoint>> poligonos = new List<List<IntPoint>>();
            List<PoligonoDTO> resp = new List<PoligonoDTO>();
            if (KmlData == null)
                throw new ApplicationException("Arquivo KML não inicializado!");

            foreach (Polygon poly in KmlData.Root.Flatten().OfType<Polygon>())
            {
                List<IntPoint> poligono = new List<IntPoint>();
                poly.OuterBoundary.LinearRing.Coordinates.ToList().ForEach(x => poligono.Add(new IntPoint((long)(x.Latitude * Precisao), (long)(x.Longitude * Precisao))));
                poligonos.Add(poligono);
            }

            poligonos = UnePoligonosClipper(poligonos);

            poligonos.ForEach(x => resp.Add(ConvertePoligonoClipper(x)));

            return resp;
        }

    }
}
