using System;
using System.ServiceModel;

namespace SAMU192InterfaceService
{
    public class SAMU192ConnectorWCF
    {
        private string serviceURI;
        private string userName;
        private string password;

        private TimeSpan openTimeout;
        private TimeSpan closeTimeout;
        private TimeSpan sendTimeout;
        private TimeSpan receiveTimeout;

        public SAMU192ConnectorWCF(string serviceURI, string userName, string password, int openTimeoutSec, int closeTimeoutSec, int sendTimeoutSec, int receiveTimeoutSec)
        {
            this.serviceURI = serviceURI;
            this.userName = userName;
            this.password = password;

            this.openTimeout = new TimeSpan(0, 0, openTimeoutSec);
            this.closeTimeout = new TimeSpan(0, 0, closeTimeoutSec);
            this.sendTimeout = new TimeSpan(0, 0, sendTimeoutSec);
            this.receiveTimeout = new TimeSpan(0, 0, receiveTimeoutSec);
        }

        public bool EnviarDados(string[] dados)
        {
            var ts = GetTransactionService();
            bool result = ts.EnviarDados(dados);

            return result;
        }

        public string RetornaConsultaParametrizacao(string[] dados)
        {
            var ts = GetTransactionService();
            string result = ts.ConsultaParametrizacao(dados);

            return result;
        }

        public string BuscaMensagens(string[] dados)
        {
            var ts = GetTransactionService();
            string result = ts.BuscarMensagens(dados);

            return result;
        }

        public bool EnviarMidia(string[] dados, byte[] midia)
        {
            var ts = GetTransactionService();
            bool result = ts.EnviarMidia(dados, midia);

            return result;
        }

        private ISAMU192ServiceWCF GetTransactionService()
        {
            if (serviceURI.ToLower().StartsWith("https"))
            {
                BasicHttpsBinding binding = new BasicHttpsBinding();
                binding.OpenTimeout = openTimeout;
                binding.CloseTimeout = closeTimeout;
                binding.SendTimeout = sendTimeout;
                binding.ReceiveTimeout = receiveTimeout;
                binding.Security.Mode = BasicHttpsSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                EndpointAddress endpoint = new EndpointAddress(serviceURI);
                SAMU192ServiceServiceClient ts = new SAMU192ServiceServiceClient(binding, endpoint);

                ts.ClientCredentials.UserName.UserName = userName;
                ts.ClientCredentials.UserName.Password = password;

                return ts;
            }
            else
            {
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.OpenTimeout = openTimeout;
                binding.CloseTimeout = closeTimeout;
                binding.SendTimeout = sendTimeout;
                binding.ReceiveTimeout = receiveTimeout;

                EndpointAddress endpoint = new EndpointAddress(serviceURI);
                SAMU192ServiceServiceClient ts = new SAMU192ServiceServiceClient(binding, endpoint);

                return ts;
            }
        }
    }
}