using SAMU192InterfaceService;
using SAMU192InterfaceService.DataContracts;
using System;
using System.IO;

namespace SAMU192Service
{
    public class SAMU192ServiceWCF : ISAMU192ServiceWCF
    {
        public SAMU192ServiceWCF()
        {

        }

        public bool EnviarDados(string callerID, string dados)
        {
            return true;
        }

        public bool EnviarDados(string[] dados)
        {
            throw new NotImplementedException();
        }

        public bool EnviarFoto(string callerID, byte[] foto)
        {
            string path = string.Format(@"D:\TEMP\{0}{1}", Guid.NewGuid().ToString("N"), ".jpg");
            File.WriteAllBytes(path, foto);
            return true;
        }

        public bool EnviarMidia(string[] dados, byte[] midia)
        {
            throw new NotImplementedException();
        }

        public string BuscarMensagens(string[] dados)
        {
            throw new NotImplementedException();
        }

        public string EnviarMensagens(string[] dados)
        {
            throw new NotImplementedException();
        }

        public string ConsultarParametrizacao(string[] dados)
        {
            throw new NotImplementedException();
        }
    }
}