using System.Collections.Generic;
using System.Linq;

namespace SAMU192Core.BLL
{
    internal class Poligono
    {
        private const double DISTANCIA_PROXIMO = 0.25; // em Km

        private double? minLatitude, minLongitude, maxLatitude, maxLongitude;

        internal List<Coordenada> pontos;

        internal double MinLatitude
        {
            get
            {
                if (!minLatitude.HasValue && pontos != null && pontos.Count > 0)
                    minLatitude = pontos.Min(x => x.Latitude);
                return minLatitude.GetValueOrDefault(0);
            }
        }

        internal double MaxLatitude
        {
            get
            {
                if (!maxLatitude.HasValue && pontos != null && pontos.Count > 0)
                    maxLatitude = pontos.Max(x => x.Latitude);
                return maxLatitude.GetValueOrDefault(0);
            }
        }

        internal double MinLongitude
        {
            get
            {
                if (!minLongitude.HasValue && pontos != null && pontos.Count > 0)
                    minLongitude = pontos.Min(x => x.Longitude);
                return minLongitude.GetValueOrDefault(0);
            }
        }

        internal double MaxLongitude
        {
            get
            {
                if (!maxLongitude.HasValue && pontos != null && pontos.Count > 0)
                    maxLongitude = pontos.Max(x => x.Longitude);
                return maxLongitude.GetValueOrDefault(0);
            }
        }

        internal Poligono(List<Coordenada> pontos)
        {
            this.pontos = pontos;
        }

        /// <summary>
        /// Testa se um ponto está dentro do polígono
        /// </summary>
        /// <param name="ponto"></param>
        /// <returns></returns>
        internal bool Contem(Coordenada ponto)  // Avaliar performance/correção da BOInfoGPS.DentroPoligono
        {
            int i, j;
            bool c = false;
            for (i = 0, j = pontos.Count - 1; i < pontos.Count; j = i++)
            {
                if ((((pontos[i].Latitude <= ponto.Latitude) && (ponto.Latitude < pontos[j].Latitude))
                        || ((pontos[j].Latitude <= ponto.Latitude) && (ponto.Latitude < pontos[i].Latitude)))
                        && (ponto.Longitude < (pontos[j].Longitude - pontos[i].Longitude) * (ponto.Latitude - pontos[i].Latitude)
                            / (pontos[j].Latitude - pontos[i].Latitude) + pontos[i].Longitude))
                    c = !c;
            }
            return c;
        }

        /// <summary>
        /// Testa se um ponto está próximo ao polígono, respeitando a distância definida em DISTANCIA_PROXIMO
        /// </summary>
        /// <param name="ponto"></param>
        /// <returns></returns>
        internal bool Proximo(Coordenada ponto)
        {
            Coordenada[] vertices = pontos.ToArray();
            for (int i = 0; i < vertices.Length; i++)
            {
                int next = (i == vertices.Length - 1 ? 0 : i + 1);

                Reta r = new Reta(vertices[i], vertices[next]);
                if (r.DistanciaPontoSegmentoKm(ponto) < DISTANCIA_PROXIMO)
                    return true;
            }
            return false;
        }

    }
}
