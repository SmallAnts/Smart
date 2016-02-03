using Smart.Core.Data;
using Smart.Samples.Domain.Entites;
using Smart.Core.Dependency;
using Smart.Samples.Domain.Context;

namespace Smart.Samples.Services
{
    internal class UserService : ServiceBase, IUserService, IDependency
    {
        IRepository<UserInfo> _userinfoRepository;

        public UserService(
            IRepository<UserInfo> userinfoRepository
            )
        {
            _userinfoRepository = userinfoRepository;
        }

        public UserInfo Get(int id)
        {
            return _userinfoRepository.Get(id);
        }

        public UserInfo SiginIn(string email, string password)
        {
            var user = _userinfoRepository.Get(u => u.Email == email && u.Password == password);
            return user;
        }

    }
}
