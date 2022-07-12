using System.Collections.Generic;
using System.Linq;

using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class PoligonoDTO
    {
        private Poligono poligono;

        internal Poligono Poligono { get { return poligono; } }

        public PoligonoDTO(List<CoordenadaDTO> pontos)
        {
            poligono = new Poligono(pontos.Select(x=> new Coordenada(x.Latitude, x.Longitude)).ToList());
        }

        internal PoligonoDTO(Poligono poligono)
        {
            this.poligono = poligono;
        }

        public List<CoordenadaDTO> getPontos()
        {
            return poligono.pontos.Select(x=> new CoordenadaDTO(x.Latitude, x.Longitude)).ToList();
        }

        public void getPontos(List<CoordenadaDTO> pontos)
        {
            poligono.pontos = pontos.Select(x => new Coordenada(x.Latitude, x.Longitude)).ToList();
        }

        public double MinLatitude { get { return poligono.MinLatitude; } }
        public double MaxLatitude { get { return poligono.MaxLatitude; } }

        public double MinLongitude { get { return poligono.MinLongitude; } }
        public double MaxLongitude { get { return poligono.MaxLongitude; } }

        public override string ToString()
        {
            return poligono.ToString();
        }

    }
}
