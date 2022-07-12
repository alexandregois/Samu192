using System;
using SAMU192InterfaceService.DataContracts;
using SAMU192InterfaceService.Utils;

namespace SAMU192Core.DTO
{
    public class EnviarMidiaDTO
    {
        private DCEnviarMidiaV1 info;

        internal DCEnviarMidiaV1 Info { get { return info; } }

        public EnviarMidiaDTO() { 
            info = new DCEnviarMidiaV1();
        }

        public EnviarMidiaDTO(DCEnviarMidiaV1 info)
        {
            if (info == null) info = new DCEnviarMidiaV1();
            this.info = info;
        }

        public string Identificador { get { return info.Identificador; } set { info.Identificador = value; } }
        public string NomeArquivo { get { return info.NomeArquivo; } set { info.NomeArquivo = value; } }
        public Enums.TipoMidia TipoMidia { get { return info.TipoMidia; } set { info.TipoMidia = value; } }

    }
}
