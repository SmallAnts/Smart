using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Samples.Services.Extensions
{
    public static class UserServiceExtensions
    {
        public static async Task<Domain.Entites.UserInfo> GetAsync(this IUserService service, int id)
        {
            return await Task.FromResult(service.Get(id));
        }

        public static async Task<Domain.Entites.UserInfo> SiginInAsync(this IUserService service, string email, string password)
        {
            return await Task.FromResult(service.SiginIn(email, password));
        }
    }
}
