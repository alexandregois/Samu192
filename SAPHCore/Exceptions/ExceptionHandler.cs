using System;
using System.Net;

namespace SAMU192Core.Exceptions
{
    internal static class ExceptionHandler
    {
        internal static void LancarExcecoesConexao(Exception ex)
        {
            if (ex is TimeoutException || ex is WebException || ex.InnerException is WebException || ex.InnerException is TimeoutException)
            {
                if (ex.Message.Contains("RequestEntityTooLarge"))
                    throw new ValidationException("Tamanho do arquivo de mídia muito grande. Verifique!");
                else
                    throw new ValidationException("Serviço indisponível! Verifique sua internet ou tente novamente mais tarde.");
            }
        }
    }
}
