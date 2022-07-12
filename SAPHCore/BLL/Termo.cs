using SAMU192Core.DTO;

namespace SAMU192Core.BLL
{
    internal class Termo
    {
        internal bool aceite;

        internal bool Aceite
        {
            get
            {
                return aceite;
            }
            set
            {
                aceite = value;
            }
        }

        internal Termo()
        { }

        internal Termo(bool aceite)
        {
            this.aceite = aceite;
        }

        internal Termo(TermoDTO termo)
        {
            this.aceite = termo.Aceite;
        }

         public override string ToString()
        {
            return string.Format((aceite ? "Aceito" : "Não aceito"));
        }
    }
}
