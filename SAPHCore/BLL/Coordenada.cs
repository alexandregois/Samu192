using SAMU192Core.DTO;
using System;

namespace SAMU192Core.BLL
{ 
    internal class Coordenada
    {

        private double latitude, longitude;
        private double? latitudeKm, longitudeKm;

        internal double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                latitudeKm = null;
            }
        }
        internal double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                longitudeKm = null;
            }
        }

        internal double LatitudeKm
        {
            get
            {
                if (!latitudeKm.HasValue)
                    latitudeKm = 100.574 * Latitude;
                return latitudeKm.Value;
            }
        }

        internal double LongitudeKm
        {
            get
            {
                if (!longitudeKm.HasValue)
                    longitudeKm = 111.320 * Longitude * Math.Cos(Math.PI * Latitude / 180);
                return longitudeKm.Value;
            }
        }

        internal Coordenada()
        { }

        internal Coordenada(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        internal Coordenada(CoordenadaDTO coordenada)
        {
            this.latitude = coordenada == null ? 0 : coordenada.Latitude;
            this.longitude = coordenada == null ? 0 : coordenada.Longitude;
        }

        internal double DistanciaKm(Coordenada ponto)
        {
            var baseRad = Math.PI * this.Latitude / 180;
            var targetRad = Math.PI * ponto.Latitude / 180;
            var theta = this.Longitude - ponto.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            return dist * 60 * 1.1515 * 1.609344; //Km
        }
    }
}

