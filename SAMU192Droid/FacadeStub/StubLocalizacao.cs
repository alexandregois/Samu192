using System.Collections.Generic;
using Android.Content.Res;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192Droid.Implementaitons;

namespace SAMU192Droid.FacadeStub
{
    internal static class StubLocalizacao
    {
        internal static void Carrega(AssetManager assets)
        {
            LeitorCSVImpl leitor = new LeitorCSVImpl(assets);
            FacadeLocalizacao.Carregar(leitor);
        }

        internal static List<ServidorDTO> Localizar(CoordenadaDTO coordenada)
        {
            return FacadeLocalizacao.Localizar(coordenada);
        }

        internal static bool ValidarEndereco(EnderecoDTO endereco)
        {
            return FacadeLocalizacao.ValidarEndereco(endereco);
        }
    }
}