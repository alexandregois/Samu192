using System;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192Core.Interfaces;
using SAMU192InterfaceService;
using static SAMU192Core.Utils.Constantes;

namespace SAMU192Core.Servicos
{
    internal class WebService
    {
        INetworkConnection netConn;
        ServidorDTO servidor;

        public WebService(ServidorDTO _servidor, INetworkConnection _netConn)
        {
            servidor = _servidor;
            netConn = _netConn;
        }

        private SAMU192ConnectorWCF ServiceConnector
        {
            get
            {
                return new SAMU192ConnectorWCF(servidor.Endereco, servidor.Usuario, servidor.Senha, SERVICE_OPEN_TIMEOUT_SECONDS, SERVICE_CLOSE_TIMEOUT_SECONDS, SERVICE_SEND_TIMEOUT_SECONDS, SERVICE_RECEIVE_TIMEOUT_SECONDS); 
            }
        }

        string msgFalha = "Falha ao retornar dados do Serviço.";

        internal bool EnviarMidia(string[] dados, byte[] midia)
        {
            ValidaConexao();
            try
            {
                bool result = ServiceConnector.EnviarMidia(dados, midia);
                if (!result)
                {
                    throw new ValidationException(msgFalha);
                }

                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LancarExcecoesConexao(ex);
                throw ex;
            }
        }

        internal bool EnviarDados(string[] dados)
        {
            ValidaConexao();
            try
            {
                bool result = ServiceConnector.EnviarDados(dados);

                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LancarExcecoesConexao(ex);
                throw ex;
            }
        }

        internal string RetornaConsultaParametrizacao(string[] dados)
        {
            ValidaConexao();
            try
            {
                string result = ServiceConnector.RetornaConsultaParametrizacao(dados);

                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LancarExcecoesConexao(ex);
                throw ex;
            }
        }

        internal string BuscaMensagens(string[] dados)
        {
            ValidaConexao();
            try
            {
                string result = ServiceConnector.BuscaMensagens(dados);

                return result;
            }
            catch (Exception ex)
            {
                ExceptionHandler.LancarExcecoesConexao(ex);
                throw ex;
            }
        }


        private void ValidaConexao()
        {
            if (!netConn.CheckNetworkConnection())
                throw new ConnectionException("Esta funcionalidade necessita do uso de internet. Por favor, verifique seu sinal de internet.");
        }
    }
}