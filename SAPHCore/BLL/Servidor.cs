
using SAMU192Core.Utils;

namespace SAMU192Core.BLL
{
    internal class Servidor
    {

        const string CHAVE = "AY1Wa4B5YW17sloYa!";

        /// <summary>
        /// Identificador único do servidor
        /// </summary>
        internal int cod;
        /// <summary>
        /// Nome do servidor (utilizado apenas para fins de visualização no configurador e possivelmente para facilitar testes/debug 
        /// </summary>
        private string nome;
        /// <summary>
        /// URL do WCF para onde os dados deverão ser enviados
        /// </summary>
        internal string endereco;
        /// <summary>
        /// Nome de usuário para autenticação no serviço
        /// </summary>
        internal string usuario;
        /// <summary>
        /// Senha para autenticação no serviço
        /// </summary>
        internal string senha;

        /// <summary>
        /// Peso da Central para GPS
        /// </summary>
        internal string peso;

        internal string Nome { get => nome; set => nome = value; }

        internal string GeraStringArquivo()
        {
            return string.Format("{0};{1};{2};{3};{4};{5}", cod.ToString(), Nome, peso, endereco, Criptografa(usuario), Criptografa(senha));
        }

        internal void LeStringArquivo(string dados)
        {
            //string[] array = dados.Split(';');
            //cod = int.Parse(array[0]);
            //Nome = array[1];
            //endereco = array[2];
            //if (array.Length > 3)
            //{
            //    usuario = Descriptografa(array[3]);
            //    senha = Descriptografa(array[4]);
            //}
            //else
            //{
            //    usuario = string.Empty;
            //    senha = string.Empty;
            //}

            string[] array = dados.Split(';');
            cod = int.Parse(array[0]);
            Nome = array[1];
            peso = array[2];
            endereco = array[3];
            if (array.Length > 4)
            {
                usuario = Descriptografa(array[4]);
                senha = Descriptografa(array[5]);
            }
            else
            {
                usuario = string.Empty;
                senha = string.Empty;
            }
        }

        internal Servidor FromString(string dados)
        {
            this.LeStringArquivo(dados);
            return this;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", cod.ToString(), nome);
        }

        private string Criptografa(string valor)
        {
            Cryptography c = new Cryptography();
            c.Password = CHAVE;
            return c.EncryptString(valor);
        }

        private string Descriptografa(string valor)
        {
            Cryptography c = new Cryptography();
            c.Password = CHAVE;
            return c.DecryptString(valor);
        }

    }
}
