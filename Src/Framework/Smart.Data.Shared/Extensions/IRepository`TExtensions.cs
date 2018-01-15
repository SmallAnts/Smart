using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Data.Extensions
{
    public static class IRepositoryExtensions
    {
        public static void Insert<T>(this Core.Data.IRepository<T> irepository, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var ret = irepository.Insert(entity);
            }
        }

        public static void Delete<T>(this Core.Data.IRepository<T> irepository, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var ret = irepository.Delete(entity);
            }
        }

        public static void Update<T>(this Core.Data.IRepository<T> irepository, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var ret = irepository.Update(entity);
            }
        }
    }
}
