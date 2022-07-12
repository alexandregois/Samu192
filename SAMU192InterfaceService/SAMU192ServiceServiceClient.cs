using SAMU192InterfaceService.DataContracts;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SAMU192InterfaceService
{
    public partial class SAMU192ServiceServiceClient : ClientBase<ISAMU192ServiceWCF>, ISAMU192ServiceWCF
    {
        protected override ISAMU192ServiceWCF CreateChannel()
        {
            return new SAMU192ServiceWCFChannel(this);
        }

        private class SAMU192ServiceWCFChannel : ChannelBase<ISAMU192ServiceWCF>, ISAMU192ServiceWCF
        {
            public SAMU192ServiceWCFChannel(ClientBase<ISAMU192ServiceWCF> client) :
                base(client)
            { }

            public bool EnviarMidia(string[] dados, byte[] midia)
            {
                object[] _args = new object[2];
                _args[0] = dados;
                _args[1] = midia;
                bool result = (bool)base.EndInvoke("EnviarMidia", _args, base.BeginInvoke("EnviarMidia", _args, null, new object()));
                return result;
            }

            public bool EnviarDados(string[] dados)
            {
                object[] _args = new object[1];
                _args[0] = dados;
                return (bool)base.EndInvoke("EnviarDados", _args, base.BeginInvoke("EnviarDados", _args, null, new object()));
            }

            public string BuscarMensagens(string[] dados)
            {
                object[] _args = new object[1];
                _args[0] = dados;
                return (string)base.EndInvoke("BuscarMensagens", _args, base.BeginInvoke("BuscarMensagens", _args, null, new object()));
            }

            public string ConsultaParametrizacao(string[] dados)
            {
                object[] _args = new object[1];
                _args[0] = dados;
                return (string)base.EndInvoke("ConsultaParametrizacao", _args, base.BeginInvoke("ConsultaParametrizacao", _args, null, new object()));
            }
        }

        public SAMU192ServiceServiceClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        { }

        public bool EnviarDados(string[] dados)
        {
            return base.Channel.EnviarDados(dados);
        }

        public bool EnviarMidia(string[] dados, byte[] midia)
        {
            return base.Channel.EnviarMidia(dados, midia);
        }

        public string BuscarMensagens(string[] dados)
        {
            return base.Channel.BuscarMensagens(dados);
        }

        public string ConsultaParametrizacao(string[] dados)
        {
            return base.Channel.ConsultaParametrizacao(dados);
        }
    }
}