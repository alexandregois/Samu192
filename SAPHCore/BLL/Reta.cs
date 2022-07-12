using System;

namespace SAMU192Core.BLL
{
    internal class Reta
    {

        internal Coordenada ponto1;
        internal Coordenada ponto2;

        internal Reta() { }

        internal Reta(Coordenada p1, Coordenada p2)
        {
            this.ponto1 = p1;
            this.ponto2 = p2;
        }

        internal double DistanciaPontoSegmentoKm(Coordenada ponto)
        {
            bool dentro = (ponto.Longitude  > Math.Min(this.ponto1.Longitude, this.ponto2.Longitude) &&
                ponto.Longitude < Math.Max(this.ponto1.Longitude, this.ponto2.Longitude)) ||
                ponto.Latitude  > Math.Min(this.ponto1.Latitude, this.ponto2.Latitude) &&
                ponto.Latitude  < Math.Max(this.ponto1.Latitude, this.ponto2.Latitude);
            if (dentro)
                return DistanciaPontoRetaKm(ponto);
            else
                return Math.Min(ponto.DistanciaKm(this.ponto1), ponto.DistanciaKm(this.ponto2));
        }

        internal double DistanciaPontoRetaKm(Coordenada ponto)
        {
            Double minLong = Math.Min(Math.Min(ponto1.LongitudeKm, ponto2.LongitudeKm), ponto.LongitudeKm);
            Double minLat = Math.Min(Math.Min(ponto1.LatitudeKm, ponto2.LatitudeKm), ponto.LatitudeKm);

            Coordenada c = new Coordenada(ponto.LatitudeKm - minLat, ponto.LongitudeKm - minLong );
            Coordenada c1 = new Coordenada(ponto1.LatitudeKm - minLat, ponto1.LongitudeKm - minLong);
            Coordenada c2 = new Coordenada(ponto2.LatitudeKm - minLat, ponto2.LongitudeKm - minLong);

            return Math.Abs((c2.Longitude - c1.Longitude) * (c1.Latitude - c.Latitude) - (c1.Longitude - c.Longitude) * (c2.Latitude - c1.Latitude)) /
                Math.Sqrt(Math.Pow(c2.Longitude - c1.Longitude, 2) + Math.Pow(c2.Latitude - c1.Latitude, 2));
        }

    }
}
