using SAMU192Core.DTO;
using SAMU192Core.Exceptions;

namespace SAMU192Core.BLL
{
    internal class Telefone
    {
        private string ddd;
        private string numero;
        private int slotId;

        public Telefone()
        {
        }

        public Telefone(TelefoneDTO telefone)
        {
            this.ddd = telefone.Ddd;
            this.numero = telefone.Numero;
            this.slotId = telefone.SlotId;
        }

        public Telefone(string ddd, string numero, int slotId)
        {
            this.ddd = ddd;
            this.numero = numero;
            this.slotId = slotId;
        }

        public string Numero { get => numero; set => numero = value; }
        public string Ddd { get => ddd; set => ddd = value; }
        public int SlotId { get => slotId; set => slotId = value; }

        internal bool ValidarTelefone()
        {
            if (string.IsNullOrEmpty(this.Ddd))
                throw new ValidationException("DDD deve ser informado!");

            var dddEhNumero = int.TryParse(Ddd, out int n1);

            if (!dddEhNumero)
                throw new ValidationException("Informe apenas números para o DDD!");

            if (Ddd.Length < 2)
                throw new ValidationException("DDD deve possuir pelo menos 2 dígitos!");

            if (Ddd.Length > 3)
                throw new ValidationException("DDD deve possuir no máximo 3 dígitos!");

            if (Ddd.Length == 3 && Ddd.Substring(0, 1) == "0")
                Ddd = Ddd.Substring(1, 2);

            if (string.IsNullOrEmpty(Numero))
                throw new ValidationException("Número de Telefone deve ser informado!");

            var telefoneEhNumero = int.TryParse(Numero, out int n2);

            if (!telefoneEhNumero)
                throw new ValidationException("Informe apenas números para o Número de Telefone!");

            if (Numero.Length != 9)
                throw new ValidationException("Número de Telefone deve possuir 9 dígitos!");

            return true;
        }

        public override string ToString()
        {
            return string.Format("({0}) {1}", Ddd, Numero);
        }

    }
}
