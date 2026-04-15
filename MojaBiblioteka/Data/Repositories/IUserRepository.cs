using MojaBiblioteka.Utility;

namespace MojaBiblioteka.Data.Repositories
{
    public interface IUserRepository
    {
        bool Exists(string login);
        UserModel GetByCredentials(string login, string passwordHash);
        UserModel Add(string login, string passwordHash);
    }
}
