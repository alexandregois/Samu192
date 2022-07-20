using SAPHBO;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using TRUE_SAPH_APP_FCM_WindowsService.Util;

namespace TRUE_SAPH_APP_FCM_WindowsService
{
    public partial class AppFcmSenderService : ServiceBase
    {
        private Schedule _server = null;
        private string _serviceName = "TRUE SAPH APP FCM WindowsService";
        private FCMSender _fcmSender;

        public AppFcmSenderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            Debugger.Launch();
#endif
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (string.IsNullOrEmpty(this.ServiceName))
            {
                this.ServiceName = _serviceName;
            }

            BOUtil.InicializaBancos(true);

            string senderID = string.Empty;
            string serverKey = string.Empty;

            BOParametro oPar = new BOParametro();

            if (oPar.CarregaAK1(eParametros.APP192FCMSenderID, null) && !string.IsNullOrWhiteSpace(oPar.ValorString))
            {
                senderID = oPar.ValorString;
            }
            else
                throw new Exception("Parâmentro 'APP SAMU 192 - FCM Sender ID' não configurado no SAPH Cliente");

            if (oPar.CarregaAK1(eParametros.APP192FCMServerKey, null) && !string.IsNullOrWhiteSpace(oPar.ValorString))
            {
                serverKey = oPar.ValorString;
            }
            else
                throw new Exception("Parâmentro 'APP SAMU 192 - FCM Server Key' não configurado no SAPH Cliente");

            _fcmSender = new FCMSender(BOUtil.CarregaProxy(Environment.MachineName.ToString()), senderID, serverKey);

            _server = new Schedule(this, new Logger(_serviceName), _fcmSender);
            _server.StartServer();
        }

        protected override void OnStop()
        {
            _server.StopServer();
            _server = null;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
        }
    }
}