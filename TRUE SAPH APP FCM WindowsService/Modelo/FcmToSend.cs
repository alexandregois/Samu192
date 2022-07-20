using System;

namespace TRUE_SAPH_APP_FCM_WindowsService.Modelo
{
    public class FcmToSend
    {
        public string FcmRegistration { get; set; }

        public FcmPayLoad PayLoad { get; set; }

        public class FcmPayLoad
        {
            public Guid CodConversaMensagem { get; set; }
            public DateTime HorarioRegistro { get; set; }
            public string Mensagem { get; set; }
            public int Tstamp { get; set; }
        }
    }
}