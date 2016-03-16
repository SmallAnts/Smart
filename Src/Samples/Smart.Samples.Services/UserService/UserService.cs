using Smart.Samples.Domain.Entites;
using Smart.Core.Dependency;
using Smart.Data.EF;
using System.Transactions;

namespace Smart.Samples.Services
{
    internal class UserService : ServiceBase, IUserService, IDependency
    {
        IEFRepository<UserInfo> _userinfoRepository;

        public UserService(
            IEFRepository<UserInfo> userinfoRepository
            )
        {
            _userinfoRepository = userinfoRepository;
        }

        public UserInfo Get(int id)
        {
            return _userinfoRepository.GetById(id);
        }

        public UserInfo SiginIn(string email, string password)
        {
            var user = _userinfoRepository.Get(u => u.Email == email && u.Password == password);
            return user;
        }

    }
}
