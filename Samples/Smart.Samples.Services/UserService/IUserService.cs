using Smart.Samples.Domain.Entites;

namespace Smart.Samples.Services
{
    public interface IUserService
    {
        UserInfo Get(int id);

        UserInfo SiginIn(string email, string password);
    }
}
