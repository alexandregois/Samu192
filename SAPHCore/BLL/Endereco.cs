using SAMU192Core.DTO;

namespace SAMU192Core.BLL
{
    internal class Endereco
    {
        private string logradouro;
        private string numero;
        private string bairro;
        private string cidade;
        private string estado;
        private string complemento;
        private Coordenada coordenada;
        private string nome;
        private string referencia;
        private string enderecoJaPesquisado;
        private bool emCache;

        internal string Logradouro { get => logradouro; set => logradouro = value; }
        internal string Numero { get => numero; set => numero = value; }
        internal string Bairro { get => bairro; set => bairro = value; }
        internal string Cidade { get => cidade; set => cidade = value; }
        internal string Estado { get => estado; set => estado = value; }
        internal string Complemento { get => complemento; set => complemento = value; }
        internal string Nome { get => nome; set => nome = value; }
        internal string Referencia { get => referencia; set => referencia = value; }
        internal string EnderecoJaPesquisado { get => enderecoJaPesquisado; set => enderecoJaPesquisado = value; }
        internal bool EmCache { get => emCache; set => emCache = value; }

        internal Coordenada Coordenada { get => coordenada; set => coordenada = value; }

        internal Endereco() { }

        internal Endereco(string logradouro, string numero, string complemento, string bairro, string cidade, string estado, Coordenada coordenada, string nome, string referencia, string enderecoJaPesquisado, bool emCache)
        {
            this.logradouro = logradouro;
            this.numero = numero;
            this.complemento = complemento;
            this.bairro = bairro;
            this.cidade = cidade;
            this.estado = estado;
            this.coordenada = coordenada;
            this.nome = nome;
            this.referencia = referencia;
            this.enderecoJaPesquisado = enderecoJaPesquisado;
            this.emCache = emCache;
        }

        internal Endereco(EnderecoDTO endereco)
        {
            this.logradouro = endereco.Logradouro;
            this.numero = endereco.Numero;
            this.complemento = endereco.Complemento;
            this.bairro = endereco.Bairro;
            this.cidade = endereco.Cidade;
            this.estado = endereco.Estado;
            this.coordenada = new Coordenada(endereco.Coordenada);
            this.nome = endereco.Nome;
            this.referencia = endereco.Referencia;
            this.enderecoJaPesquisado = endereco.EnderecoJaPesquisado;
            this.emCache = endereco.EmCache;
        }
    }
}
