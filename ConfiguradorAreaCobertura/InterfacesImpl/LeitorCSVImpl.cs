using System.IO;
using SAMU192Core.Interfaces;

namespace ConfiguradorAreaCobertura.InterfacesImpl
{
    public class LeitorCSVImpl : ILeitorDados
    {
        string caminho;
        public LeitorCSVImpl(string caminho)
        {
            this.caminho = caminho;
        }

        public string Carrega(string nome)
        {
            return File.ReadAllText(Path.Combine(caminho, string.Format("{0}.CSV", nome)));
        }

        public void Grava(string nome, string dados)
        {
            File.WriteAllText(Path.Combine(caminho, string.Format("{0}.CSV", nome)), dados);
        }
    }
}