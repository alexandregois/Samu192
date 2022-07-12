using System.Text;
using System.Collections.Generic;
using SAMU192Core.DAO;

namespace SAMU192Core.BLL
{
    internal class Area
    {

        /// <summary>
        /// Identificador único da área
        /// </summary>
        internal int cod;
        /// <summary>
        /// Servidor do SAPH que atende esta área
        /// </summary>
        internal int codServidor;
        /// <summary>
        /// Indica a procedência do polígono
        /// </summary>
        internal string fonte;

        private Poligono poligono;

        private Poligono limites;

        /// <summary>
        /// Polígono "complexo e detalhado" que demarca a área
        /// </summary>
        internal Poligono Poligono
        {
            get
            {
                if (poligono == null)
                {
                    string csv = DataAcessObj.Leitor.Carrega(cod.ToString());
                    poligono = this.LeStringArquivoPoligono(csv);
                }

                return poligono;
            }
            set
            {
                poligono = value;
            }
        }

        internal Poligono Limites
        {
            get
            {
                if (limites == null)
                {
                    limites = new Poligono(new List<Coordenada>()
                    {
                        new Coordenada(Poligono.MinLatitude, Poligono.MinLongitude) ,
                        new Coordenada(Poligono.MinLatitude, Poligono.MaxLongitude ),
                        new Coordenada(Poligono.MaxLatitude, Poligono.MaxLongitude ),
                        new Coordenada(Poligono.MaxLatitude, Poligono.MinLongitude )
                    });
                }
                return limites;
            }
        }

        internal void Grava()
        {
            DataAcessObj.Leitor.Grava(cod.ToString(), GeraStringArquivoPoligono());
        }

        internal string GeraStringArquivo()
        {
            return string.Format("{0};{1};{2}", cod.ToString(), codServidor.ToString(), fonte);
        }
        internal void LeStringArquivo(string dados)
        {
            string[] array = dados.Split(';');
            cod = int.Parse(array[0]);
            codServidor = int.Parse(array[1]);
            fonte = array[2];
        }

        internal string GeraStringArquivoPoligono()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Poligono.pontos.Count.ToString());
            Poligono.pontos.ForEach(x => sb.AppendFormat(";{0};{1}", x.Latitude.ToString(), x.Longitude.ToString()));
            return sb.ToString();
        }

        internal Poligono LeStringArquivoPoligono(string dados)
        {
            string[] array = dados.Split(';');
            List<Coordenada> c = new List<Coordenada>();
            if (dados.Length > 0)
            {
                int tam = int.Parse(array[0]);
                for (int a = 1; a < tam * 2; a += 2)
                    c.Add(new Coordenada(double.Parse(array[a]), double.Parse(array[a + 1])));
            }
            return new Poligono(c);
        }

        internal Area FromString(string dados)
        {
            this.LeStringArquivo(dados);
            return this;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}pts", cod.ToString(), codServidor.ToString(), this.fonte, this.Poligono.pontos.Count.ToString());//Poligono.Pontos.Count.ToString());
        }

    }
}
