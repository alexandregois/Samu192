namespace SAMU192Core.Interfaces
{
    public interface ILeitorDados
    {
        string Carrega(string nome);

        void Grava(string nome, string dados);
    }
}
