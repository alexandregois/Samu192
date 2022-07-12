using System;
using System.Collections.Generic;
using System.Text;

namespace SAMU192Core.Interfaces
{
    public abstract class Geometria
    {
        public class Ponto
        {
            public double x;
            public double y;

            public Ponto(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public abstract bool ExisteIntersecao(List<Ponto> a, List<Ponto> b);
    }
}
