using SAMU192Core.DTO;
using System;
using System.Threading;

namespace SAMU192Core.Servicos
{
    public class Local
    {
        Timer timer;
        ServidorDTO lastObject;
        const int timerPeriod = Utils.Constantes.REFRESH_ACOMOPANHAMENTOS_SECONDS * 1000;
        Action<ServidorDTO> callback;
        Func<ServidorDTO> executar;
        bool retornoPendente = false;
        private object _sync = new object();

        public bool Liga(Action<ServidorDTO> _callback, Func<ServidorDTO> _executar)
        {
            this.callback = _callback;
            this.executar = _executar;
            timer = new Timer(TimerCallback);
            retornoPendente = false;
            return timer.Change(0, timerPeriod);
        }

        private void TimerCallback(object state)
        {
            try
            {
                lock (_sync)
                {
                    if (!retornoPendente)
                    {
                        retornoPendente = true;
                        lastObject = Executar();
                        retornoPendente = false;
                    }
                }
                this.callback?.Invoke(lastObject);
            }
            catch
            {
                retornoPendente = false;
                return;
            }
        }

        private ServidorDTO Executar()
        {
            return this.executar?.Invoke();
        }

        public bool Desliga()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
            timer = null;
            this.callback = null;
            this.executar = null;
            return true;
        }

        public object GetLastObject()
        {
            return lastObject;
        }
    }
}
