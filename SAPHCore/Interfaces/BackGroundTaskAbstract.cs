using SAMU192Core.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAMU192Core.Interfaces
{
    public abstract class BackGroundTaskAbstract
    {
        public Action MyOnPreExecute;
        public Action<CancellationToken> MyRunInBackGround;
        public Action MyOnPostExecute;
        public Action MyOnCancel;
        public Action<Exception> MyOnError;
        public Action<ValidationException> MyOnValidationException;
        public bool WithOverlay;
        public string Message;
        public object Overlay;
        public object Screen;
        public object ScrollView;
        public int CancelAfterMiliseconds;
        public CancellationTokenSource MyCancellationTokenSource;
        public bool IsActive;

        public BackGroundTaskAbstract(object screen, bool withOverlay, Action onPreExecute, Action<CancellationToken> onRunInBackGround, Action onPostExecute, Action onCancel, Action<Exception> onError, Action<ValidationException> onValidationException, string message, int cancelAfterMiliseconds, object scrollView)
        {
            Screen = screen;
            ScrollView = scrollView;
            WithOverlay = withOverlay;
            MyOnPreExecute = onPreExecute;
            MyRunInBackGround = onRunInBackGround;
            MyOnPostExecute = onPostExecute;
            MyOnCancel = onCancel;
            MyOnError = onError;
            MyOnValidationException = onValidationException;
            Message = message;
            CancelAfterMiliseconds = cancelAfterMiliseconds;
            IsActive = false;
        }

        public abstract void StartOverlay();

        public abstract void HideOverlay();

        public abstract void OnPreExecute();        

        public abstract void OnPostExecute();

        public abstract void OnCancel();

        public abstract void OnError(Exception ex);

        public abstract void OnValidationException(ValidationException vex);

        public void Interrupt()
        {
            MyCancellationTokenSource.Cancel(true);
        }

        public void OnRunInBackground(CancellationToken ct)
        {
            MyRunInBackGround(ct);
        }

        public async Task AsyncTask(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            OnPreExecute();
            ct.ThrowIfCancellationRequested();

            OnRunInBackground(ct);
            ct.ThrowIfCancellationRequested();

            OnPostExecute();
            ct.ThrowIfCancellationRequested();
        }

        public void Execute()
        {
            IsActive = true;
            StartOverlay();

            MyCancellationTokenSource = new CancellationTokenSource();
            var ct = MyCancellationTokenSource.Token;

            Task.Run(async () =>
            {
                try
                {
                    await AsyncTask(ct);
                }
                catch (System.OperationCanceledException)
                {
                    OnCancel();
                }
                catch (ValidationException vex)
                {
                    OnValidationException(vex);
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
                IsActive = false;
            }, ct);

            MyCancellationTokenSource.CancelAfter(CancelAfterMiliseconds);
        }
    }
}
