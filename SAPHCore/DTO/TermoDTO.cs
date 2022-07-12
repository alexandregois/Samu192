using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class TermoDTO
    {
        private Termo termo;

        internal Termo Termo { get { return termo; } }

        public TermoDTO()
        {
            termo = new Termo();
        }

        internal TermoDTO(Termo termo)
        {
            this.termo = termo;
        }
        public TermoDTO(bool aceite)
        {
            termo = new Termo(aceite);
        }

        public bool Aceite { get { return termo.aceite; } set { termo.aceite = value; } }

        public override string ToString()
        {
            return termo.ToString();
        }
    }
}
