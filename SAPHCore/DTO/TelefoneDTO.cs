using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class TelefoneDTO
    {
        private Telefone telefone;

        internal Telefone Telefone { get { return telefone; } }

        public TelefoneDTO()
        {
            telefone = new Telefone();
        }

        public TelefoneDTO(string ddd, string numero, int slotId)
        {
            telefone = new Telefone(ddd, numero, slotId);
        }

        internal TelefoneDTO(Telefone telefone)
        {
            this.telefone = telefone;
        }

        public string Ddd { get { return telefone.Ddd; } set { telefone.Ddd = value; } }
        public string Numero { get { return telefone.Numero; } set { telefone.Numero = value; } }
        public int SlotId { get => telefone.SlotId; set => telefone.SlotId = value; }

        public override string ToString()
        {
            return telefone.ToString();
        }

    }
}
