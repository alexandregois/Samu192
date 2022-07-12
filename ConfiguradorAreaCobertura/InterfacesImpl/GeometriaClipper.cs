using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using ClipperLib;

namespace ConfiguradorAreaCobertura.InterfacesImpl
{
    class GeometriaClipper : Geometria
    {
        const long PRECISAO = 10000000000L;

        private List<IntPoint> PoligonoClipper(List<Geometria.Ponto> pontos)
        {
            List<IntPoint> resp = new List<IntPoint>();
            pontos.ForEach(x => resp.Add(new IntPoint((long)(x.x * PRECISAO), (long)(x.y * PRECISAO))));
            return resp;
        }       

        public override bool ExisteIntersecao(List<Ponto> a, List<Ponto> b)
        {
            Clipper c = new Clipper();
            c.AddPolygon(PoligonoClipper(a), PolyType.ptSubject);
            c.AddPolygon(PoligonoClipper(b), PolyType.ptClip);
            List<List<IntPoint>> solucao = new List<List<IntPoint>>();
            c.Execute(ClipType.ctIntersection, solucao, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
            return (solucao.Count > 0);
        }
    }
}
