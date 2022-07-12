using SAMU192Core.Utils;

namespace SAMU192Core.Interfaces
{
    public abstract class TelefoniaAbstract
    {
        object phoneManager;
        public object PhoneManager { get => phoneManager; private set => phoneManager = value; }

        public TelefoniaAbstract(LigacaoConectada LigacaoConectada, LigacaoDesconectada LigacaoDesconectada, object phoneManager)
        {
            LigacaoConectada_Event += LigacaoConectada;
            LigacaoDesconectada_Event += LigacaoDesconectada;

            this.PhoneManager = phoneManager;
        }

        public abstract bool MakeCall(string number, object args = null);

        public abstract bool VerificaPermissao(object activity);

        public abstract void StartPhoneManager();

        public virtual void DisposePhoneManager()
        {
            this.PhoneManager = null;
            LigacaoConectada_Event -= null;
            LigacaoDesconectada_Event -= null;
        }

        public virtual void OnCallStateChanged(Enums.PhoneCallState state)
        {
            if (state == Enums.PhoneCallState.Connected)
                LigacaoConectada_Event();
            else //Enums.PhoneCallState.Disconnected)
                LigacaoDesconectada_Event();
        }

        public delegate void LigacaoConectada();
        LigacaoConectada LigacaoConectada_Event;

        public delegate void LigacaoDesconectada();
        LigacaoDesconectada LigacaoDesconectada_Event;
    }
}
