using System;
using SAMU192Core.DTO;

namespace ConfiguradorAreaCobertura.Events
{
    public class QuadranteEventArgs:EventArgs
    {

        private QuadranteDTO quadrante;
        public QuadranteDTO Quadrante { get { return quadrante; } }

        public QuadranteEventArgs(QuadranteDTO quadrante)
        {
            this.quadrante = quadrante;
        }

    }
}
