using System.Collections.Generic;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192iOS.Implementaitons;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubLocalizacao
    {
        internal static void Carrega(string assets)
        {
            LeitorCSVImpl leitor = new LeitorCSVImpl(assets);
            FacadeLocalizacao.Carregar(leitor);
        }

        internal static List<ServidorDTO> Localizar(CoordenadaDTO coordenada)
        {
            List<ServidorDTO> servidores = FacadeLocalizacao.Localizar(coordenada);
            //servidores.Clear();//simula fora de cobertura
            return servidores;
        }

        internal static bool ValidarEndereco(EnderecoDTO endereco)
        {
            return FacadeLocalizacao.ValidarEndereco(endereco);
        }
    }
}