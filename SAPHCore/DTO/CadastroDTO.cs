using System;
using System.Collections.Generic;
using System.Linq;
using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class CadastroDTO
    {
        private Cadastro cadastro;

        internal Cadastro Cadastro { get { return cadastro; } }

        public CadastroDTO()
        {
            cadastro = new Cadastro();
        }

        public CadastroDTO(string nome, DateTime? dtNasc, char? sexo, string historico, TelefoneDTO[] telefones)
        {
            Telefone[] telefonesTemp = new Telefone[0];
            if (telefones != null)
                telefonesTemp = telefones.Select(x => new Telefone(x)).ToArray();
            cadastro = new Cadastro(nome, dtNasc, sexo, historico, telefonesTemp);
        }

        internal CadastroDTO(Cadastro cadastro)
        {
            this.cadastro = cadastro;
        }

        public string Nome { get { return cadastro.Nome; } set { cadastro.Nome = value; } }
        public TelefoneDTO[] Telefones
        {
            get
            {
                if (cadastro.Telefones == null)
                    return new TelefoneDTO[0];

                return cadastro.Telefones.Select(x => new TelefoneDTO(x.Ddd, x.Numero, x.SlotId)).ToArray();
            }
            set
            {
                cadastro.Telefones = value.Select(x => new Telefone(x)).ToArray();
            }
            
        }
        public DateTime? DtNasc { get { return cadastro.DtNasc; } set { cadastro.DtNasc = value; } }
        public char? Sexo { get { return cadastro.Sexo; } set { cadastro.Sexo = value; } }
        public string Historico { get { return cadastro.Historico; } set { cadastro.Historico = value; } }

        public override string ToString()
        {
            string result = string.Format("{0}", string.IsNullOrEmpty(Nome) ? "Nome da Pessoa" : cadastro.Nome);
            return result;
        }

        public bool ValidarTelefones()
        {
            return Cadastro.ValidarTelefones();
        }

    }
}