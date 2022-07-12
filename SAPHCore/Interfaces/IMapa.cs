using System.Threading.Tasks;
using SAMU192Core.DTO;

namespace SAMU192Core.Interfaces
{
    public interface IMapa
    {
        void Carrega(object args, CoordenadaDTO coordenadaDefault, bool overrideDelegate);

        object ConfigurarMapa(object view);

        CoordenadaDTO PosicionaMapa(CoordenadaDTO coordenada, float? zoom = null);

        CoordenadaDTO PosicionaImagemNoMapa(CoordenadaDTO coordenada, string imagemPath, object tela, int index, string nomeEquipe);

        void AjustaCentralizacaoMapa(CoordenadaDTO coordenada, CoordenadaDTO coordenadaAmbulancia, double pFator, bool alterouDestino);

        void LimparMapa();

        Task<EnderecoDTO> ReverterCoordenada(CoordenadaDTO coordenada, object args);

        Task<EnderecoDTO> ReverterEndereco(string enderecoLegivel, object args);

    }
}
