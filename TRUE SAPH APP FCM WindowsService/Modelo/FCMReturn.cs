using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRUE_SAPH_APP_FCM_WindowsService.Modelo
{
    public class FCMReturn
    {
        public bool Error { get; set; }
        public string Message { get; set; }

        public FCMReturn() { }

        public FCMReturn(bool error, string message)
        {
            this.Error = error;
            this.Message = message;
        }
    }
}
