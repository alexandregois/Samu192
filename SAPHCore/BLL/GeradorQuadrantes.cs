using System;
using System.Collections.Generic;
using System.Linq;

using SAMU192Core.Interfaces;

namespace SAMU192Core.BLL
{
    internal class GeradorQuadrantes
    {
        public int MinAreas = 2;
        public int MaxNiveis = 7;

        private Mapeamento m;
        private Geometria geo;

        public GeradorQuadrantes(Mapeamento mapeamento, Geometria geo)
        {
            m = mapeamento;
            this.geo = geo;
        }

        public List<Quadrante> GeraQuadrantes()
        {
            try
            {
                List<Quadrante> quadrantes = OtimizaQuadrante(QuadranteInicial());
                EliminaQuadrantesInuteis(quadrantes);
                return quadrantes;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Não foi possível gerar os quadrantes para os polígonos cadastrados.", ex);
            }
        }

        /// <summary>
        /// Remove os quadrantes que possuem apenas um quadrante filho.
        /// </summary>
        private void EliminaQuadrantesInuteis(List<Quadrante> quadrantes)
        {
            Quadrante inutil;
            do
            {
                inutil = quadrantes.Where(x => x.areas.Count == 0 && quadrantes.Where(y => y.codPai == x.cod).Count() <= 1).FirstOrDefault();
                if (inutil != null)
                {
                    quadrantes.Where(x => x.codPai == inutil.cod).First().codPai = inutil.codPai;
                    quadrantes.Remove(inutil);
                }

            } while (inutil != null);

        }

        private Quadrante QuadranteInicial()
        {
            return new Quadrante()
            {
                cod = 1,
                ponto1 = new Coordenada(m.areas.Min(x => x.Poligono.MinLatitude), m.areas.Min(x => x.Poligono.MinLongitude)),
                ponto2 = new Coordenada(m.areas.Max(x => x.Poligono.MaxLatitude), m.areas.Max(x => x.Poligono.MaxLongitude)),
                areas = m.areas.Select(x => x.cod).ToList()
            };
        }

        private List<Quadrante> OtimizaQuadrante(Quadrante pai)
        {
            List<Quadrante> lista = new List<Quadrante>();
            bool otimizado = false;
            Quadrante q = QuadranteFilho(0, pai);
            if (q.areas.Count > 0)
            {
                lista.Add(q);
                otimizado = true;
                if (q.areas.Count > MinAreas && q.cod.ToString().Length < MaxNiveis)
                    lista.AddRange(OtimizaQuadrante(q));
            }
            q = QuadranteFilho(1, pai);
            if (q.areas.Count > 0)
            {
                lista.Add(q);
                otimizado = true;
                if (q.areas.Count > MinAreas && q.cod.ToString().Length < MaxNiveis)
                    lista.AddRange(OtimizaQuadrante(q));
            }
            q = QuadranteFilho(2, pai);
            if (q.areas.Count > 0)
            {
                lista.Add(q);
                otimizado = true;
                if (q.areas.Count > MinAreas && q.cod.ToString().Length < MaxNiveis)
                    lista.AddRange(OtimizaQuadrante(q));
            }
            q = QuadranteFilho(3, pai);
            if (q.areas.Count > 0)
            {
                lista.Add(q);
                otimizado = true;
                if (q.areas.Count > MinAreas && q.cod.ToString().Length < MaxNiveis)
                    lista.AddRange(OtimizaQuadrante(q));
            }

            if (otimizado)
                pai.areas.Clear();

            return lista;
        }

        private Quadrante QuadranteFilho(int posicao, Quadrante pai)
        {
            Quadrante resp = new Quadrante() { cod = pai.cod * 10 + posicao, codPai = pai.cod, areas = new List<int>() };

            resp.ponto1 = pai.DefinePonto(1, posicao);
            resp.ponto2 = pai.DefinePonto(2, posicao);

            List<Geometria.Ponto> poligono2 = new List<Geometria.Ponto>();
            resp.Poligono.pontos.ForEach(x => poligono2.Add(new Geometria.Ponto(x.Latitude, x.Longitude)));

            foreach (int a in pai.areas)
            {
                Area area = m.areas.Where(x => x.cod == a).First();
                List<Geometria.Ponto> poligono1 = new List<Geometria.Ponto>();
                area.Poligono.pontos.ForEach(x => poligono1.Add(new Geometria.Ponto(x.Latitude, x.Longitude)));

                if (geo.ExisteIntersecao(poligono1, poligono2))
                    resp.areas.Add(a);
            }

            return resp;
        }

    }
}
