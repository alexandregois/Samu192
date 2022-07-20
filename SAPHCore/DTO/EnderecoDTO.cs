using SAMU192Core.BLL;
using System.Collections.Generic;

namespace SAMU192Core.DTO
{
    public class EnderecoDTO
    {
        private Endereco endereco;

        internal Endereco Endereco { get { return endereco; } }

        public EnderecoDTO()
        {
            endereco = new Endereco();
        }

        public EnderecoDTO(string logradouro, string numero, string complemento, string bairro, string cidade, string estado, CoordenadaDTO coordenada, string nome = "", string referencia = "", string enderecoJaDigitado = "", bool emCache = false)
        {
            endereco = new Endereco(logradouro, numero, complemento, bairro, cidade, estado, coordenada.Coordenada, nome, referencia, enderecoJaDigitado, emCache);
        }

        internal EnderecoDTO(Endereco endereco)
        {
            this.endereco = endereco;
        }

        public string Logradouro { get { return endereco.Logradouro; } set { endereco.Logradouro = value; } }
        public string Numero { get { return endereco.Numero; } set { endereco.Numero = value; } }
        public string Bairro { get { return endereco.Bairro; } set { endereco.Bairro = value; } }
        public string Cidade { get { return endereco.Cidade; } set { endereco.Cidade = value; } }
        public string Estado { get { return endereco.Estado; } set { endereco.Estado = value; } }
        public string Complemento { get { return endereco.Complemento; } set { endereco.Complemento = value; } }
        public string Nome { get { return endereco.Nome; } set { endereco.Nome = value; } }
        public string Referencia { get { return endereco.Referencia; } set { endereco.Referencia = value; } }
        public string EnderecoJaPesquisado { get => endereco.EnderecoJaPesquisado; set => endereco.EnderecoJaPesquisado = value; }
        public bool EmCache { get => endereco.EmCache; set => endereco.EmCache = value; }

        public CoordenadaDTO Coordenada { get { return new CoordenadaDTO(endereco.Coordenada); } set { endereco.Coordenada = new Coordenada(value); } }

        public override string ToString()
        {
            if (endereco.Nome == null)
                endereco.Nome = "";
            if (endereco.Referencia == null)
                endereco.Referencia = "";

            if (endereco.Logradouro == null)
                endereco.Logradouro = "";
            if (endereco.Numero == null)
                endereco.Numero = "";
            if (endereco.Complemento == null)
                endereco.Complemento = "";
            if (endereco.Bairro == null)
                endereco.Bairro = "";
            if (endereco.Cidade == null)
                endereco.Cidade = "";
            if (endereco.Estado == null)
                endereco.Estado = "";

            return string.Format("{0}{1}{2}{3}{4}{5}",
                endereco.Logradouro,
                string.IsNullOrEmpty(endereco.Numero) ? "" : ", " + endereco.Numero,
                string.IsNullOrEmpty(endereco.Complemento) ? "" : " / " + endereco.Complemento,
                string.IsNullOrEmpty(endereco.Bairro) ? "" : " - " + endereco.Bairro,
                string.IsNullOrEmpty(endereco.Cidade) ? "" : ", " + endereco.Cidade,
                string.IsNullOrEmpty(endereco.Estado) ? "" : " - " + endereco.Estado);
        }
        public string ToString(bool complemento)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}",
                endereco.Logradouro,
                string.IsNullOrEmpty(endereco.Numero) ? "" : ", " + endereco.Numero,
                complemento ? string.IsNullOrEmpty(endereco.Complemento) ? "" : " / " + endereco.Complemento : "",
                string.IsNullOrEmpty(endereco.Bairro) ? "" : " - " + endereco.Bairro,
                string.IsNullOrEmpty(endereco.Cidade) ? "" : ", " + endereco.Cidade,
                string.IsNullOrEmpty(endereco.Estado) ? "" : " - " + endereco.Estado);
        }
    }

    public class EnderecoCollectionDTO
    {
        private List<EnderecoDTO> enderecos;

        public List<EnderecoDTO> Enderecos { get { return enderecos; } set { enderecos = value; } }

        public EnderecoCollectionDTO()
        {
            enderecos = new List<EnderecoDTO>();
        }
    }
}