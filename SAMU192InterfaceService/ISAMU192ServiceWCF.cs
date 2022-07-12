using SAMU192InterfaceService.DataContracts;
using System;
using System.ServiceModel;

namespace SAMU192InterfaceService
{
    [ServiceContract]
    public interface ISAMU192ServiceWCF
    {
        [OperationContract]
        bool EnviarDados(string[] dados);

        [OperationContract]
        bool EnviarMidia(string[] dados, byte[] midia);

        [OperationContract]
        string BuscarMensagens(string[] dados);

        [OperationContract]
        string ConsultaParametrizacao(string[] dados);
    }
}