using System.IO;
using SAMU192Core.Interfaces;

namespace SAMU192iOS.Implementaitons
{
    public class LeitorCSVImpl : ILeitorDados
    {
        object assets;
        public LeitorCSVImpl(object assets)
        {
            this.assets = assets;
        }

        public string Carrega(string nome)
        {
            string fileName = string.Format("{0}/CSV/{1}.CSV", this.assets, nome); // remember case-sensitive
            //string localHtmlUrl = Path.Combine(NSBundle.MainBundle.BundlePath, fileName);

            string dados = File.ReadAllText(fileName);
            
            return dados;
        }

        public void Grava(string nome, string dados)
        {
            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var fileName = Path.Combine(documents, "Write.txt");
            //string fileName = string.Format("{0}/{1}.CSV", this.assets, nome); // remember case-sensitive
            //File.WriteAllText(fileName, dados);
            throw new System.NotImplementedException();
        }
    }
}