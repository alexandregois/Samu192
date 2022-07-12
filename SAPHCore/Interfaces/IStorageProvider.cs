using SAMU192Core.DTO;

namespace SAMU192Core.Interfaces
{
    public interface IStorageProvider
    {
        bool Salvar<BaseDTO>(BaseDTO obj);

        BaseDTO Recuperar<BaseDTO>(string key);
    }
}
