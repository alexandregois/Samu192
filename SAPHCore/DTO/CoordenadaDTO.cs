using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class CoordenadaDTO
    {
        private Coordenada coordenada;

        internal Coordenada Coordenada { get { return coordenada; } }

        public CoordenadaDTO() { 
            coordenada = new Coordenada();
        }

        public CoordenadaDTO(double latitude, double longitude)
        {
            coordenada = new Coordenada(latitude, longitude);
        }

        internal CoordenadaDTO(Coordenada coordenada)
        {
            if (coordenada == null) coordenada = new Coordenada();
            this.coordenada = coordenada;
        }

        public double Latitude { get { return coordenada.Latitude; } set { coordenada.Latitude = value; } }
        public double Longitude { get { return coordenada.Longitude; } set { coordenada.Longitude = value; } }

        public override string ToString()
        {
            return coordenada.ToString();
        }

        public override bool Equals(System.Object obj)
        {
            CoordenadaDTO comparada = (CoordenadaDTO)obj;
            int countTruncatedLat = 0, countTruncatedLng = 0, countLat = 0, countLng = 0;
            string truncatedLat = "0", truncatedLng = "0", lat = "0", lng = "0";
            if (comparada.Latitude != 0)
            {
                countTruncatedLat = comparada.Latitude.ToString().Substring(0, 1) == "-" ? 8 : 7;
                truncatedLat = comparada.Latitude.ToString().Substring(0, countTruncatedLat);
            }
            if (comparada.Longitude != 0)
            {
                countTruncatedLng = comparada.Longitude.ToString().Substring(0, 1) == "-" ? 8 : 7;
                truncatedLng = comparada.Longitude.ToString().Substring(0, countTruncatedLng);
            }
            if (this.Latitude != 0)
            {
                countLat = this.Latitude.ToString().Substring(0, 1) == "-" ? 8 : 7;
                lat = this.Latitude.ToString().Substring(0, countLat);
            }
            if (this.Longitude != 0)
            {
                countLng = this.Longitude.ToString().Substring(0, 1) == "-" ? 8 : 7;
                lng = this.Longitude.ToString().Substring(0, countLng);
            }

            return (truncatedLat == lat && truncatedLng == lng);
        }

    }
}
