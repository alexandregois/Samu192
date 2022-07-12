using System.Collections.Generic;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192iOS.Implementaions;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubCadastro
    {
        static StorageProviderImpl myStorageProviderImpl;
        internal static bool ExisteCadastro()
        {
            var cadastro = RecuperaCadastro();
            return (cadastro != null && cadastro.Telefones != null && cadastro.Telefones.Length > 0 &&
                    !string.IsNullOrEmpty(cadastro.Telefones[0].Ddd) && !string.IsNullOrEmpty(cadastro.Telefones[0].Numero));
        }

        internal static bool ExisteCadastro(CadastroDTO cadastro)
        {
            return (cadastro != null && cadastro.Telefones != null && cadastro.Telefones.Length > 0 &&
                    !string.IsNullOrEmpty(cadastro.Telefones[0].Ddd) && !string.IsNullOrEmpty(cadastro.Telefones[0].Numero));
        }

        internal static bool ExisteEnderecos()
        {
            var enderecos = RecuperaEnderecos();
            return (enderecos != null && enderecos.Count > 0);
        }

        internal static void Carrega()
        {
            if (myStorageProviderImpl == null)
                myStorageProviderImpl = new StorageProviderImpl();
            FacadeCadastro.Carrega(myStorageProviderImpl);
        }

        internal static bool SalvaEnderecos(List<EnderecoDTO> collection)
        {
            return FacadeCadastro.SalvarColection<List<EnderecoDTO>>(collection);
        }

        internal static bool AdicionarEndereco(EnderecoDTO endereco)
        {
            return FacadeCadastro.AdicionarEndereco(endereco);
        }

        internal static bool SalvaCadastro(CadastroDTO cadastro)
        {
            return FacadeCadastro.Salvar<CadastroDTO>(cadastro);
        }

        internal static bool SalvaAceiteTermo(TermoDTO aceitouTermo)
        {
            return FacadeCadastro.Salvar<TermoDTO>(aceitouTermo);
        }

        internal static List<EnderecoDTO> RecuperaEnderecos()
        {
            return FacadeCadastro.Recuperar<List<EnderecoDTO>>();
        }

        internal static CadastroDTO RecuperaCadastro()
        {
            return FacadeCadastro.Recuperar<CadastroDTO>();
        }
        internal static bool ValidaCadastro(CadastroDTO cadastro)
        {
            return FacadeCadastro.ValidarCadastro<CadastroDTO>(cadastro);
        }

        internal static bool ValidaTelefone(CadastroDTO cad)
        {
            return FacadeCadastro.ValidarTelefone(cad);
        }

        internal static TermoDTO RecuperaAceiteTermo()
        {
            return FacadeCadastro.Recuperar<TermoDTO>();
        }
    }
}