using System;
using System.Collections.Generic;
using System.Linq;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;

namespace SAMU192Core.BLL
{
    internal class Cadastro
    {
        private string nome;
        private Telefone[] telefones;
        private DateTime? dtNasc;
        private char? sexo;
        private string historico;

        internal string Nome
        {
            get
            {
                return nome;
            }
            set
            {
                nome = value;
            }
        }
        internal Telefone[] Telefones
        {
            get
            {
                return telefones;
            }
            set
            {
                telefones = value;
            }
        }
        internal DateTime? DtNasc
        {
            get
            {
                return dtNasc;
            }
            set
            {
                dtNasc = value;
            }
        }
        internal char? Sexo
        {
            get
            {
                return sexo;
            }
            set
            {
                sexo = value;
            }
        }
        internal string Historico
        {
            get
            {
                return historico;
            }
            set
            {
                historico = value;
            }
        }
        internal Cadastro()
        {
            this.telefones = new List<Telefone>().ToArray();
        }

        internal Cadastro(string nome, DateTime? dtNasc, char? sexo, string historico, Telefone[] telefones)
        {
            this.nome = nome;
            this.dtNasc = dtNasc;
            this.sexo = sexo;
            this.historico = historico;
            this.telefones = telefones;
        }

        internal Cadastro(CadastroDTO cadastro)
        {
            this.nome = cadastro.Nome;
            this.dtNasc = cadastro.DtNasc;
            this.sexo = cadastro.Sexo;
            this.historico = cadastro.Historico;
            this.telefones = cadastro.Telefones.Select(x => new Telefone(x)).ToArray();
        }

        internal int CalcularIdade()
        {
            if (!this.DtNasc.HasValue)
                return 0;

            var today = DateTime.Today;
            var dtNasc = this.DtNasc.Value;
            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dtNasc.Year * 100 + dtNasc.Month) * 100 + dtNasc.Day;
            return (a - b) / 10000;
        }

        internal bool Validar()
        {
            ValidarTelefones();

            if (string.IsNullOrEmpty(this.Nome))
                throw new ValidationException("Nome da Pessoa deve ser informado!");

            if (!this.DtNasc.HasValue)
                throw new ValidationException("Data de Nascimento deve ser informada!");

            if (this.DtNasc.Value > DateTime.Now)
                throw new ValidationException("Data de Nascimento inválida!");

            if (!this.Sexo.HasValue)
                throw new ValidationException("Sexo deve ser informado!");

            /*if (string.IsNullOrEmpty(this.Historico))
                throw new ValidationException("Historico deve ser informado!");*/

            return true;
        }

        internal bool ValidarTelefones()
        {
            if (telefones == null || telefones.Length == 0)
                throw new ValidationException("Pelo menos um Telefone com DDD deve ser informado!");

            foreach (Telefone t in telefones)
                    t.ValidarTelefone();
            
            return true;
        }
    }
}
