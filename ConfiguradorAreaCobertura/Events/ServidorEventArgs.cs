using System;
using SAMU192Core.DTO;

namespace ConfiguradorAreaCobertura.Events
{
    public class ServidorEventArgs:EventArgs
    {

        private ServidorDTO servidor;
        public ServidorDTO Servidor { get { return servidor; } }

        public ServidorEventArgs(ServidorDTO servidor)
        {
            this.servidor = servidor;
        }

    }
}
