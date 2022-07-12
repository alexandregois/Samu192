using System.Collections.Generic;

using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class QuadranteDTO
    {
        private Quadrante quadrante;

        internal Quadrante Quadrante { get { return quadrante; } }

        public QuadranteDTO()
        {
            quadrante = new Quadrante();
        }

        internal QuadranteDTO(Quadrante quadrante)
        {
            this.quadrante = quadrante;
        }

        public List<int> Areas { get { return quadrante.areas; } set { quadrante.areas = value; } }
        public int Cod { get { return quadrante.cod; } set { quadrante.cod = value; } }
        public int? CodPai { get { return quadrante.codPai; } set { quadrante.codPai = value; } }
        public CoordenadaDTO getPonto1()
        {
            return new CoordenadaDTO(quadrante.ponto1);
        }
        public void setPonto1(CoordenadaDTO coordenada)
        {
                quadrante.ponto1 = new Coordenada(coordenada.Latitude, coordenada.Longitude);
        }
        public CoordenadaDTO getPonto2()
        {
            return new CoordenadaDTO(quadrante.ponto2);
        }
        public void setPonto2(CoordenadaDTO coordenada)
        {
            quadrante.ponto2 = new Coordenada(coordenada.Latitude, coordenada.Longitude);
        }

        public PoligonoDTO Poligono { get { return new PoligonoDTO(quadrante.Poligono); }}

        public override string ToString()
        {
            return quadrante.ToString();
        }

    }
}
