using System;
using System.Collections.Generic;
using SAMU192Core.DTO;
using SAMU192Core.Servicos;
using SAMU192Core.Exceptions;
using System.Threading.Tasks;
using System.Threading;

namespace SAMU192Core.Facades
{
    public static class FacadeWebService
    {
        public static Action solicitarAtendimentoCallBack;
        public static Action<ServidorDTO> solicitarAutorizacaoMidiaCallBack;
        static bool aguardandoRetornoSolicitacaoMidia = false;
        static List<RequisicaoMidia> Requisicoes = new List<RequisicaoMidia>();

        public static void Carrega(Action _solicitarAtendimentoCallBack, Action<ServidorDTO> _solicitarAutorizacaoMidiaCallBack)
        {
            solicitarAtendimentoCallBack = _solicitarAtendimentoCallBack;
            solicitarAutorizacaoMidiaCallBack = _solicitarAutorizacaoMidiaCallBack;
        }

        public static string RetornaConsultaParametrizacao(string[] dados)
        {
            string result = String.Empty;

            if (dados != null)
            {
                List<ServidorDTO> servidores = null;
                var coord = FacadeGPS.GetLastLocation();


#if DEBUG
                CoordenadaDTO Coordenadas = new CoordenadaDTO();
                Coordenadas.Latitude = -29.1668707;
                Coordenadas.Longitude = -51.1801015;
                coord = Coordenadas;
#endif

                if (coord != null)
                {
                    servidores = FacadeLocalizacao.Localizar(coord);

                    //TODO: Servidores devem estar em CACHE?
                    foreach (var servidor in servidores)
                    {
                        result = new WebService(servidor, FacadeConexao.GetNetworkConnection()).RetornaConsultaParametrizacao(dados);
                    }

                }
            }
            //solicitarAtendimentoCallBack?.Invoke();

            return result;
        }

        public static string EnviarMensagens(string[] dados)
        {
            string result = String.Empty;

            if (dados != null)
            {
                List<ServidorDTO> servidores = null;
                var coord = FacadeGPS.GetLastLocation();
                if (coord != null)
                {
                    servidores = FacadeLocalizacao.Localizar(coord);

                    //TODO: Servidores devem estar em CACHE?
                    foreach (var servidor in servidores)
                    {
                        result = new WebService(servidor, FacadeConexao.GetNetworkConnection()).EnviarMensagens(dados);
                    }
                }
            }
            //solicitarAtendimentoCallBack?.Invoke();

            return result;
        }

        public static string BuscarMensagens(string[] dados)
        {
            string result = String.Empty;

            if (dados != null)
            {
                List<ServidorDTO> servidores = null;
                var coord = FacadeGPS.GetLastLocation();

#if DEBUG
                CoordenadaDTO Coordenadas = new CoordenadaDTO();
                Coordenadas.Latitude = -29.1668707;
                Coordenadas.Longitude = -51.1801015;
                coord = Coordenadas;
#endif

                if (coord != null)
                {
                    servidores = FacadeLocalizacao.Localizar(coord);

                    //TODO: Servidores devem estar em CACHE?
                    foreach (var servidor in servidores)
                    {
                        result = new WebService(servidor, FacadeConexao.GetNetworkConnection()).BuscarMensagens(dados);
                    }
                }
            }

            return result;
        }

        public static void SolicitarAtendimento(SolicitarAtendimentoDTO solicitacao)
        {
            if (solicitacao != null)
            {
                List<ServidorDTO> servidores = null;
                var coord = FacadeGPS.GetLastLocation();
                if (coord != null)
                {
                    servidores = FacadeLocalizacao.Localizar(coord);
                    
                    //TODO: Servidores devem estar em CACHE?
                    foreach (var servidor in servidores)
                    {
                        new WebService(servidor, FacadeConexao.GetNetworkConnection()).EnviarDados(solicitacao.Solicitacao.Dados);
                    }
                }
            }
            solicitarAtendimentoCallBack?.Invoke();
        }

        public static bool SolicitarAtendimentoChat(SolicitarAtendimentoChatDTO solicitacao)
        {
            bool validaRecebido = false;

            if (solicitacao != null)
            {
                List<ServidorDTO> servidores = null;
                var coord = FacadeGPS.GetLastLocation();
                if (coord != null)
                {
                    servidores = FacadeLocalizacao.Localizar(coord);

                    //TODO: Servidores devem estar em CACHE?
                    foreach (var servidor in servidores)
                    {
                        validaRecebido = new WebService(servidor, FacadeConexao.GetNetworkConnection()).EnviarDados(solicitacao.Solicitacao.Dados);
                    }
                }
            }
            solicitarAtendimentoCallBack?.Invoke();

            return validaRecebido;
        }


        public static void SolicitarAutorizacaoMidia(SolicitarAutorizacaoMidiaDTO solicitacao)
        {
            ServidorDTO result = null;
            if (solicitacao != null)
            {
                List<ServidorDTO> servidores = null;
                var coord = FacadeGPS.GetLastLocation();
                //TODO: Servidores devem estar em CACHE?
                if (coord != null)
                {
                    servidores = FacadeLocalizacao.Localizar(coord);
                    //TODO: Servidores devem estar em CACHE?
                    foreach (var srv in servidores)
                    {
                        if (!Requisicoes.Exists(o => o.servidor.Cod == srv.Cod && o.solicitacao.Identificador == solicitacao.Identificador))
                        {
                            var req = new RequisicaoMidia(solicitacao, srv);
                            Requisicoes.Add(req);
                            Thread th = new Thread(ExecutaSolicitacao);
                            th.Start(req);
                        }
                    }
                }
            }
        }

        public static bool EnviarMidia(ServidorDTO servidor, EnviarMidiaDTO dados, byte[] midia, Action envioDeMidiaCallBack)
        {
            if (midia == null)
                throw new ValidationException("Nenhuma mídia selecionada. Selecione uma mídia para Enviar.");

            if (servidor == null)
                throw new ValidationException("Nenhum servidor disponível para envio de mídia. Aguarde.");

            bool result = new WebService(servidor, FacadeConexao.GetNetworkConnection()).EnviarMidia(dados.Info.Dados, midia);
            envioDeMidiaCallBack?.Invoke();
            return result;
        }

        class RequisicaoMidia
        {
            public SolicitarAutorizacaoMidiaDTO solicitacao;
            public ServidorDTO servidor;
            public RequisicaoMidia(SolicitarAutorizacaoMidiaDTO _sol, ServidorDTO _ser)
            {
                solicitacao = _sol;
                servidor = _ser;
            }
        }

        private static void ExecutaSolicitacao(object requisicao)
        {
            try
            {
                if (aguardandoRetornoSolicitacaoMidia)
                    return;

                aguardandoRetornoSolicitacaoMidia = true;
                RequisicaoMidia req = (RequisicaoMidia)requisicao;

                bool result = new WebService(req.servidor, FacadeConexao.GetNetworkConnection()).EnviarDados(req.solicitacao.Solicitacao.Dados);
                if (result)
                {
                    ServidorDTO serv = new ServidorDTO();
                    serv.Cod = req.servidor.Cod;
                    serv.Endereco = req.servidor.Endereco;
                    serv.Nome = req.servidor.Nome;
                    serv.Senha = req.servidor.Senha;
                    serv.Usuario = req.servidor.Usuario;
                    solicitarAutorizacaoMidiaCallBack?.Invoke(serv);
                }

                aguardandoRetornoSolicitacaoMidia = false;
            }
            catch (Exception ex)
            {
                //dummy
                aguardandoRetornoSolicitacaoMidia = false;
            }
            finally
            {
                Requisicoes.Remove((RequisicaoMidia)requisicao);
            }
        }

        public static SolicitarAtendimentoDTO MontaPacote(int sistema, bool outraPessoa, string queixa, CadastroDTO cadastro, EnderecoDTO endereco)
        {
            //Endereço
            var solicitarAtendimento = new SolicitarAtendimentoDTO();
            if (endereco != null)
            {
                solicitarAtendimento.Logradouro = endereco.Logradouro;
                solicitarAtendimento.Numero = endereco.Numero;
                solicitarAtendimento.Complemento = endereco.Complemento;
                solicitarAtendimento.Bairro = endereco.Bairro;
                solicitarAtendimento.Cidade = endereco.Cidade;
                solicitarAtendimento.UF = endereco.Estado;
                solicitarAtendimento.Referencia = endereco.Referencia;
                solicitarAtendimento.Latitude = endereco.Coordenada.Latitude;
                solicitarAtendimento.Longitude = endereco.Coordenada.Longitude;
            }

            //Coordenada Real do Smartphone
            CoordenadaDTO coordenada = FacadeGPS.GetLastLocation();
            if (coordenada != null)
            {
                solicitarAtendimento.LatitudeApp = coordenada.Latitude;
                solicitarAtendimento.LongitudeApp = coordenada.Longitude;
            }

            //Cadastro Pessoa
            solicitarAtendimento.Nome = cadastro.Nome;
            solicitarAtendimento.DataNascimento = cadastro.DtNasc;
            solicitarAtendimento.Sexo = cadastro.Sexo.ToString();

            //Dados complementares
            solicitarAtendimento.OutraPessoa = outraPessoa;
            solicitarAtendimento.Queixa = queixa;

            //Telefone(s)
            solicitarAtendimento.Telefone1 = cadastro.Telefones.Length > 0 ? cadastro.Telefones[0].Ddd + cadastro.Telefones[0].Numero : string.Empty;
            solicitarAtendimento.Telefone2 = cadastro.Telefones.Length > 1 ? cadastro.Telefones[1].Ddd + cadastro.Telefones[1].Numero : string.Empty;

            //Identificação do Smartphone
            string identificador = FacadeUtilidades.RecuperaInstanceID();
            solicitarAtendimento.Sistema = sistema;
            solicitarAtendimento.Identificador = identificador;

            //FCM
            solicitarAtendimento.FCMRegistration = FacadePushNotifications.Token();

            return solicitarAtendimento;
        }

        public static SolicitarAtendimentoChatDTO MontaPacoteChat(int sistema, bool outraPessoa, string queixa, CadastroDTO cadastro, EnderecoDTO endereco)
        {
            //Endereço
            var solicitarAtendimento = new SolicitarAtendimentoChatDTO();
            if (endereco != null)
            {
                solicitarAtendimento.Logradouro = endereco.Logradouro;
                solicitarAtendimento.Numero = endereco.Numero;
                solicitarAtendimento.Complemento = endereco.Complemento;
                solicitarAtendimento.Bairro = endereco.Bairro;
                solicitarAtendimento.Cidade = endereco.Cidade;
                solicitarAtendimento.UF = endereco.Estado;
                solicitarAtendimento.Referencia = endereco.Referencia;
                solicitarAtendimento.Latitude = endereco.Coordenada.Latitude;
                solicitarAtendimento.Longitude = endereco.Coordenada.Longitude;
            }

            //Coordenada Real do Smartphone
            CoordenadaDTO coordenada = FacadeGPS.GetLastLocation();


//#if DEBUG
//            coordenada.Latitude = -29.1668707;
//            coordenada.Longitude = -51.1801015;
//#endif



            if (coordenada != null)
            {
                solicitarAtendimento.LatitudeApp = coordenada.Latitude;
                solicitarAtendimento.LongitudeApp = coordenada.Longitude;
            }

            //Cadastro Pessoa
            solicitarAtendimento.Nome = cadastro.Nome;
            solicitarAtendimento.DataNascimento = cadastro.DtNasc;
            solicitarAtendimento.Sexo = cadastro.Sexo.ToString();

            //Dados complementares
            solicitarAtendimento.OutraPessoa = outraPessoa;
            solicitarAtendimento.Queixa = queixa;

            //Telefone(s)
            solicitarAtendimento.Telefone1 = cadastro.Telefones.Length > 0 ? cadastro.Telefones[0].Ddd + cadastro.Telefones[0].Numero : string.Empty;
            solicitarAtendimento.Telefone2 = cadastro.Telefones.Length > 1 ? cadastro.Telefones[1].Ddd + cadastro.Telefones[1].Numero : string.Empty;

            //Identificação do Smartphone
            string identificador = FacadeUtilidades.RecuperaInstanceID();
            solicitarAtendimento.Sistema = sistema;
            solicitarAtendimento.Identificador = identificador;

            //FCM
            solicitarAtendimento.FCMRegistration = FacadePushNotifications.Token();

            return solicitarAtendimento;
        }


        public static SolicitarAutorizacaoMidiaDTO MontaSolicitacaoMidia()
        {
            var solicitarAutorizacao = new SolicitarAutorizacaoMidiaDTO();

            //Identificação do Smartphone
            string identificador = FacadeUtilidades.RecuperaInstanceID();
            solicitarAutorizacao.Identificador = identificador ?? "Simulador-Milton-7-Plus";

            return solicitarAutorizacao;
        }

    }
}
