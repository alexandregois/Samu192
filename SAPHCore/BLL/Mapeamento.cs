using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using SAMU192Core.DAO;

namespace SAMU192Core.BLL
{
    /// <summary>
    /// Classe que armazena toda as informações necessárias para determinar quais centrais SAPH
    /// podem receber a ligação e os dados do usuário
    /// </summary>
    internal class Mapeamento
    {
        private const int MIN_AREAS = 3;
        private const int MAX_NIVEIS = 8;

        internal List<Servidor> servidores;
        internal List<Area> areas;
        internal List<Quadrante> quadrantes;


        internal List<Servidor> ServidoresDaCoordenada(Coordenada ponto)
        {
            Dictionary<int, Servidor> resp = new Dictionary<int, Servidor>();
            ServidoresDaCoordenada(ponto, quadrantes.Where(x => x.codPai == 1).ToList(), ref resp);
            return resp.Values.ToList();
        }

        private void ServidoresDaCoordenada(Coordenada ponto, List<Quadrante> quadrantes, ref Dictionary<int, Servidor> resp)
        {

            foreach (Quadrante q in quadrantes)
            {
                if (q.Poligono.Contem(ponto))
                {
                    if (q.areas != null && q.areas.Count > 0)
                    {
                        //Dois laços: se é mais fácil achar se o polígono está dentro do que está próximo, faz um depois o outro
                        foreach (int codArea in q.areas)
                        {
                            Area a = areas.Where(x => x.cod == codArea).FirstOrDefault();
                            if (a != null && !resp.ContainsKey(a.codServidor))
                            {
                                if (a.Poligono.Contem(ponto))
                                    resp.Add(a.codServidor, servidores.Where(x => x.cod == a.codServidor).FirstOrDefault());
                            }
                        }
                        foreach (int codArea in q.areas)
                        {
                            Area a = areas.Where(x => x.cod == codArea).FirstOrDefault();
                            if (a != null && !resp.ContainsKey(a.codServidor))
                            {
                                if (a.Poligono.Proximo(ponto))
                                    resp.Add(a.codServidor, servidores.Where(x => x.cod == a.codServidor).FirstOrDefault());
                            }
                        }
                    }
                    else
                        ServidoresDaCoordenada(ponto, this.quadrantes.Where(x => x.codPai == q.cod).ToList(), ref resp);
                }
            }

        }

        internal void Carrega()
        {
            string dados = DataAcessObj.Leitor.Carrega("MAPEAMENTO");

            if (!string.IsNullOrEmpty(dados))
            {
                this.LeStringArquivo(dados);
            }
        }

        internal void Grava()
        {
            DataAcessObj.Leitor.Grava("MAPEAMENTO", GeraStringArquivo());
        }

        const char cServidor = 's';
        const char cArea = 'a';
        const char cQuadrante = 'q';

        const char cPeso = 'y';

        internal string GeraStringArquivo()
        {
            StringBuilder sb = new StringBuilder();
            if (servidores.Count > 0)
                servidores.ForEach(x => sb.AppendLine(string.Format("{0};{1}", cServidor, x.GeraStringArquivo())));
            if (areas.Count > 0)
                areas.ForEach(x => sb.AppendLine(string.Format("{0};{1}", cArea, x.GeraStringArquivo())));
            if (quadrantes.Count > 0)
                quadrantes.ForEach(x => sb.AppendLine(string.Format("{0};{1}", cQuadrante, x.GeraStringArquivo())));

            return sb.ToString();
        }

        internal void LeStringArquivo(string dados)
        {
            this.areas = new List<Area>();
            this.quadrantes = new List<Quadrante>();
            this.servidores = new List<Servidor>();

            using (StringReader reader = new StringReader(dados))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null && line.Length > 0)
                    {
                        switch (line[0])
                        {
                            case cArea:
                                this.areas.Add(new Area().FromString(line.Substring(2)));
                                break;
                            case cQuadrante:
                                this.quadrantes.Add(new Quadrante().FromString(line.Substring(2)));
                                break;
                            case cServidor:
                                this.servidores.Add(new Servidor().FromString(line.Substring(2)));
                                break;
                        }
                    }
                } while (line != null);
            }
        }

    }
}
