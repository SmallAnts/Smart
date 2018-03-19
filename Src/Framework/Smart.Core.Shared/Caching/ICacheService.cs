using System;
using System.ServiceModel;

namespace Smart.Core.Caching
{
    [ServiceContract(Namespace = "http://schemas.smartants.com/cache")]
    public interface ICacheService
    {
        [OperationContract]
        string Get(string key);

        [OperationContract]
        void Set(string key, string cache, TimeSpan slidingExpiration);

        [OperationContract]
        void Remove(string key);

        [OperationContract]
        void RemoveAll(Predicate<string> match);
    }
}
