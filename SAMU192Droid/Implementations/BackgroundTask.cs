using System;
using System.Threading;
using Android.App;
using SAMU192Core.Exceptions;
using SAMU192Core.Interfaces;

namespace SAMU192Droid.Implementations
{
    internal class BackgroundTask : BackGroundTaskAbstract
    {
        public BackgroundTask(object screen, bool withOverlay, Action onPreExecute, Action<CancellationToken> onRunInBackGround, Action onPostExecute, Action onCancel, Action<Exception> onError, Action<ValidationException> onValidationException, string message = "", int cancelAfterMiliseconds = 60000, object scroll = null) : 
            base(screen, withOverlay, onPreExecute, onRunInBackGround, onPostExecute, onCancel, onError, onValidationException, message, cancelAfterMiliseconds, scroll)
        { }

        public override void StartOverlay()
        {
            if (WithOverlay)
            {
                Overlay = Interface.Utils.Mensagem.Dialogs.Progresso((Activity)Screen, base.Message, base.Interrupt, false, false);
            }
        }

        public override void HideOverlay()
        {
            if (WithOverlay)
            {
                ((Activity)Screen).RunOnUiThread(() =>
                {
                    ((Dialog)Overlay).Dismiss();
                });
            }
        }

        public override void OnPreExecute()
        {
            base.MyOnPreExecute();
        }

        public override void OnPostExecute()
        {
            HideOverlay();
            MyOnPostExecute();
        }

        public override void OnCancel()
        {
            HideOverlay();
            MyOnCancel();
        }

        public override void OnError(Exception ex)
        {
            HideOverlay();
            MyOnError(ex);
        }

        public override void OnValidationException(ValidationException vex)
        {
            HideOverlay();
            MyOnValidationException(vex);
        }
    }
}