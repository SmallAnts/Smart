using Smart.Core;
using Smart.Core.Caching;
using Smart.Sample.Core.Context;

namespace Smart.Sample.Services
{
    internal abstract class ServiceBase
    {
        private SampleDbContext _db;
        internal SampleDbContext db
        {
            get
            {
                return _db ?? (_db = SmartContext.Current.Resolve<SampleDbContext>());

                 
            }
            set
            {
                _db = value;
            }
        }
        internal ICache cache
        {
            get
            {
                return SmartContext.Current.Resolve<ICache>("ServiceCache");
            }
        }

    }
}
