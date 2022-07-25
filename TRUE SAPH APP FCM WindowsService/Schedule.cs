using Newtonsoft.Json;
using SAPHBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TRUE_SAPH_APP_FCM_WindowsService.Modelo;
using TRUE_SAPH_APP_FCM_WindowsService.Util;
using static TRUE_SAPH_APP_FCM_WindowsService.Modelo.FCMRequest;

namespace TRUE_SAPH_APP_FCM_WindowsService
{
    public class Schedule
    {
        private AppFcmSenderService _service = null;
        private Logger _logger;
        private HttpClient _httpClient = new HttpClient();
        private FCMSender _fcmSender;
        private object _lock = new object();

        public Schedule(AppFcmSenderService service, Logger logger, FCMSender fcmSender)
        {
            _service = service;
            _logger = logger;
            _fcmSender = fcmSender;
        }

        ~Schedule()
        {

        }

        public void StartServer()
        {
            Logger.LogInformation(string.Format("Serviço Iniciado em: {0}", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")));

            int timerBuscarMensagensEmSegundos = 5;

            if (!Properties.Settings.Default.TimerBuscarMensagensEmSegundos.Equals(0))
                timerBuscarMensagensEmSegundos = Properties.Settings.Default.TimerBuscarMensagensEmSegundos;

            System.Timers.Timer timerBuscarMensagens = new System.Timers.Timer(timerBuscarMensagensEmSegundos * 1000);
            timerBuscarMensagens.Elapsed += (s, ea) => { BuscaMensagens(); };
            timerBuscarMensagens.AutoReset = true;
            timerBuscarMensagens.Start();
        }

        public void StopServer()
        {
            Logger.LogInformation(string.Format("Serviço Finalizado em: {0}", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
        }

        private void BuscaMensagens()
        {
            lock (_lock)
            {
#if DEBUG
                //Logger.LogInformation(string.Format("Serviço Executado em: {0}", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")));
#endif

                try
                {
                    List<SAPHBO.APP192.BOConversaMensagem> conversaMensagemList = SAPHBO.APP192.BOConversaMensagem.CarregaArray(new SAPHBO.APP192.FiltroConversaMensagem()
                    {
                        HorarioEnviadaNula = true,
                        HorarioRecebidaNula = true,
                        HorarioLidaNula = true,
                        Sentido = 2,
                        ErrosF = 2
                    }, 0, true)?
                    .OrderBy(o => o.CodChamado)
                    .ToList();

                    if (conversaMensagemList != null && conversaMensagemList.Any())
                    {
                        int codChamado = 0;
                        string fcmRegistration = string.Empty;

                        foreach (SAPHBO.APP192.BOConversaMensagem conversaMensagem in conversaMensagemList)
                        {
                            if (!conversaMensagem.CodChamado.Equals(codChamado)) // Só carrega a AppChamado quando altera o codChamado
                            {
                                fcmRegistration = string.Empty;

                                SAPHBO.APP192.BOAPPChamado appChamado = SAPHBO.APP192.BOAPPChamado.CarregaArray(new SAPHBO.APP192.FiltroAPPChamado() { CodChamado = conversaMensagem.CodChamado.Value })?.FirstOrDefault();

                                if (appChamado != null && appChamado.CodAppConversaCompletada.HasValue)
                                {
                                    codChamado = appChamado.CodChamado.Value;

                                    SAPHBO.APP192.BOAPPConversaCompletada appConversaCompletada = new SAPHBO.APP192.BOAPPConversaCompletada();

                                    if (appConversaCompletada.Carrega(appChamado.CodAppConversaCompletada.Value))
                                    {
                                        fcmRegistration = appConversaCompletada.FcmRegistration;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(fcmRegistration))
                            {
                                FCMRequest fcmRequest = new FCMRequest()
                                {
                                    FcmTokenOrTopic = fcmRegistration,
                                    IsHighPriority = true,
                                    NotificationBody = new Notification("Nova mensagem", conversaMensagem.Mensagem),
                                    DataBody = new
                                    {
                                        CodConversaMensagem = conversaMensagem.CodConversaMensagem.Value,
                                        HorarioRegistro = conversaMensagem.HorarioRegistro.Value,
                                        Mensagem = conversaMensagem.Mensagem,
                                        Tstamp = conversaMensagem.TimeStamp.Value
                                    }
                                };

                                System.Timers.Timer timerRunOnceEnviaFCM = new System.Timers.Timer(1);
                                timerRunOnceEnviaFCM.Elapsed += (s, ea) => { EnviaFCM(conversaMensagem.CodConversaMensagem.Value, fcmRequest); };
                                timerRunOnceEnviaFCM.AutoReset = false;
                                timerRunOnceEnviaFCM.Start();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                finally
                {

                }
            }
        }

        public async void EnviaFCM(Guid codConversaMensagem, FCMRequest fcmRequest)
        {
            try
            {
                FCMReturn result = await _fcmSender.SendData(fcmRequest);

                if (result != null)
                {
                    SAPHBO.APP192.BOConversaMensagem conversaMensagem = new SAPHBO.APP192.BOConversaMensagem();
                    conversaMensagem.Carrega(codConversaMensagem);

                    if (conversaMensagem != null)
                    {
                        conversaMensagem.Acao = conversaMensagem.Acao.Value + 1;

                        if (result.Error)
                        {
                            conversaMensagem.Erros = conversaMensagem.Erros.Value + 1;
                            conversaMensagem.MensagemUltimoErro = result.Message;
                        }
                        else
                        {
                            conversaMensagem.HorarioEnviada = DateTime.Now;
                        }

                        conversaMensagem.Salva();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}