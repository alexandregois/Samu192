using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SAMU192Droid.FacadeStub;
using SAMU192Droid.Interface.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.App.ActivityManager;

namespace SAMU192Droid.Servicos
{
    [Service]
    public class LerMensagensService : Service
    {
        private const string Tag = "[LerMensagensService]";

        private bool _isRunning;
        private Context _context;
        private Task _task;

        #region overrides

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            _context = this;
            _isRunning = false;
            _task = new Task(DoWork);
        }

        public override void OnDestroy()
        {
            _isRunning = false;

            if (_task != null && _task.Status == TaskStatus.RanToCompletion)
            {
                _task.Dispose();
            }

        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _task.Start();
            }
            return StartCommandResult.Sticky;
        }

        #endregion


        public void OpenChat()
        {

            string[] dados = new string[22];
            dados[0] = "DCConsultarParametrizacaoV1";

            string result = StubWebService.RetornaConsultaParametrizacao(dados);

            var resultChat = StubWebService.bus


            Intent i = new Intent(this, typeof(ChatActivity));
            i.AddFlags(ActivityFlags.NewTask);
            this.ApplicationContext.StartActivity(i);

        }

        private void DoWork()
        {
            try
            {
                //Start

                OpenChat();

            }
            catch (Exception e)
            {

            }
            finally
            {
                StopSelf();
            }
        }

    }
}