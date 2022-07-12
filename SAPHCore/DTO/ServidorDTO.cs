using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class ServidorDTO
    {
        private Servidor servidor;

        internal Servidor Servidor { get { return servidor; } }

        public ServidorDTO()
        {
            servidor = new Servidor();
        }

        internal ServidorDTO(Servidor servidor)
        {
            this.servidor = servidor;
        }

        public string Nome { get { return servidor.Nome; } set { servidor.Nome = value; } }
        public string Endereco { get { return servidor.endereco; } set { servidor.endereco = value; } }
        public string Usuario { get { return servidor.usuario; } set { servidor.usuario = value; } }
        public string Senha { get { return servidor.senha; } set { servidor.senha = value; } }
        public int Cod { get { return servidor.cod; } set { servidor.cod = value; } }

        public override string ToString()
        {
            return servidor.ToString();
        }

    }
}
