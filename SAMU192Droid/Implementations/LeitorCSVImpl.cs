using System.IO;
using Android.Content.Res;
using SAMU192Core.Interfaces;

namespace SAMU192Droid.Implementaitons
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
            AssetManager assetsTmp = (AssetManager)assets;
            string dados = string.Empty;
            string[] arrayAssets = assetsTmp.List("CSV");
            if (arrayAssets.Length > 0)
            { 
                string item = nome + ".CSV";
                //Build Action "AndroidAsset"
                using (StreamReader sr = new StreamReader(assetsTmp.Open("CSV/" + item)))
                {
                    dados = sr.ReadToEnd();
                }
            }
            return dados;
        }

        public void Grava(string nome, string dados)
        {
            throw new System.NotImplementedException();
        }
    }
}