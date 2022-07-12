using System;
using System.Linq;
using System.Collections.Generic;

namespace SAMU192Core.BLL
{
    internal class Quadrante
    {
        /// <summary>
        /// Identificador único do quadrante
        /// </summary>
        internal int cod;

        /// <summary>
        /// "Canto" superior esquerdo do quadrante
        /// </summary>
        internal Coordenada ponto1;
        /// <summary>
        /// "Canto" inferior direito do quadrante
        /// </summary>
        internal Coordenada ponto2;
        /// <summary>
        /// Quadrante "pai" deste quadrante
        /// </summary>
        internal int? codPai;
        /// <summary>
        /// Se este quadrante não tiver filhos, indica quais areas estão dentro deste quadrante
        /// </summary>
        internal List<int> areas;

        private Poligono poligono;

        internal Poligono Poligono { 
            get
            {
                if (poligono == null)
                {
                    poligono = new Poligono(new List<Coordenada>()
                        {
                            ponto1,
                            new Coordenada(ponto1.Latitude, ponto2.Longitude),
                            ponto2,
                            new Coordenada(ponto2.Latitude, ponto1.Longitude)
                        });
                }
                return poligono;
            }
        }

        internal Coordenada DefinePonto(int ponto, int posicao)
        {
            switch (ponto)
            {
                case 1:
                    switch (posicao)
                    {
                        case 0:
                            return ponto1;
                        case 1:
                            return new Coordenada(ponto1.Latitude,
                                ponto1.Longitude + (ponto2.Longitude - ponto1.Longitude) / 2);
                        case 2:
                            return new Coordenada(ponto1.Latitude + (ponto2.Latitude - ponto1.Latitude) / 2,
                                ponto1.Longitude);
                        case 3:
                            return new Coordenada(ponto1.Latitude + (ponto2.Latitude - ponto1.Latitude) / 2,
                                ponto1.Longitude + (ponto2.Longitude - ponto1.Longitude) / 2);
                        default:
                            throw new ArgumentException(string.Format("DefinePonto: {0} não é um valor válido!", ponto.ToString()), "posicao");
                    }
                case 2:
                    switch (posicao)
                    {
                        case 0:
                            return new Coordenada(ponto1.Latitude + (ponto2.Latitude - ponto1.Latitude) / 2,
                                ponto1.Longitude + (ponto2.Longitude - ponto1.Longitude) / 2);
                        case 1:
                            return new Coordenada(ponto1.Latitude + (ponto2.Latitude - ponto1.Latitude) / 2,
                                ponto2.Longitude);
                        case 2:
                            return new Coordenada(ponto2.Latitude,
                                ponto1.Longitude + (ponto2.Longitude - ponto1.Longitude) / 2);
                        case 3:
                            return ponto2;
                        default:
                            throw new ArgumentException(string.Format("DefinePonto: {0} não é um valor válido!", ponto.ToString()), "posicao");
                    }
                default:
                    throw new ArgumentException(string.Format("DefinePonto: {0} não é um valor válido!", ponto.ToString()), "ponto");
            }
        }
        
        internal string GeraStringArquivo()
        {
            return string.Format("{0};{1};{2};{3};{4};{5}{6}{7}", cod.ToString(), codPai.ToString(), ponto1.Latitude.ToString(), ponto1.Longitude.ToString(), ponto2.Latitude.ToString(), ponto2.Longitude.ToString(), (areas.Count>0?";":""), string.Join(";", areas));
        }

        internal void LeStringArquivo(string dados)
        {
            string[] array = dados.Split(';');
            cod = int.Parse(array[0]);
            codPai = int.Parse(array[1]);
            ponto1 = new Coordenada(double.Parse(array[2]), double.Parse(array[3]));
            ponto2 = new Coordenada(double.Parse(array[4]), double.Parse(array[5]));
            areas = new List<int>();
            for (int a = 6; a <= array.GetUpperBound(0); a++)
                areas.Add(int.Parse(array[a]));
        }

        internal Quadrante FromString(string dados)
        {
            this.LeStringArquivo(dados);
            return this;
        }
        public override string ToString()
        {
            return string.Format("{0}->{1}: {2}({3})", cod.ToString(), codPai.ToString(), string.Join(",", areas.Select(x => x.ToString()).ToArray()), Poligono.pontos.Count().ToString());
        }

    }
}

