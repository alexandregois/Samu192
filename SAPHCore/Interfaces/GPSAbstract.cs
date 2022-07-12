using System;
using SAMU192Core.DTO;

namespace SAMU192Core.Interfaces
{
    public abstract class GPSAbstract
    {
        public delegate void AtualizaDadosGPS(CoordenadaDTO coordenada);
        public AtualizaDadosGPS AtualizaDadosGPS_Event;

        public delegate void AfterNotify(bool val);

        CoordenadaDTO coordenadas;
        public CoordenadaDTO Coordenadas { get => coordenadas; private set => coordenadas = value; }

        DateTime? lastUpdated;
        public DateTime? LastUpdated { get => lastUpdated; private set => lastUpdated = value; }

        public abstract void Carrega(object args = null, AfterNotify afterNotify_event = null, bool inWalkthrough = false);

        public abstract void StopLocationManager();

        public abstract void Dispose();
        public abstract string GetProviders();

        public virtual void OnGPSLocationChanged(CoordenadaDTO coordenada)
        {
            UpdateLocation(coordenada);

            if (AtualizaDadosGPS_Event != null)
                AtualizaDadosGPS_Event(coordenada);
        }

        internal CoordenadaDTO GetLastLocation()
        {
            return Coordenadas;
        }

        internal DateTime? GetTimeOfLastLocation()
        {
            return LastUpdated;
        }

        internal void UpdateLocation(CoordenadaDTO location)
        {
            Coordenadas = location;
            LastUpdated = DateTime.Now;
        }
    }
}