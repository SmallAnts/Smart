using Smart.Core.Caching;
using Smart.Core.Utilites;
using System.ComponentModel;
using System.ServiceModel;

namespace Smart.CacheServer
{
    class WcfHost
    {
        ServiceHost serviceHost;

        [Category("选项")]
        [Description("主机IP地址")]
        public string Host { get; set; } = IPAddressUtility.GetLocalIPAddress();

        [Category("选项")]
        [Description("端口号")]
        [DefaultValue(9379)]
        public int Port { get; set; } = 9379;

        [Category("选项")]
        [Description("绑定信息")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public NetTcpBinding Binding { get; private set; }

        [Browsable(false)]
        public bool IsRuning { get; private set; }

        public WcfHost()
        {
            Binding = new NetTcpBinding(SecurityMode.None);
            Binding.MaxBufferPoolSize = int.MaxValue;
            Binding.MaxReceivedMessageSize = int.MaxValue;
            Binding.MaxBufferSize = int.MaxValue;
        }

        public void Start()
        {
            serviceHost = new ServiceHost(typeof(DefaultCacheService));
            serviceHost.AddServiceEndpoint(typeof(ICacheService),
                Binding,
                $"net.tcp://{Host}:{Port}/CacheService");
            serviceHost.Open();
            IsRuning = true;
        }

        public void Stop()
        {
            if (IsRuning)
            {
                serviceHost.Close();
                IsRuning = false;
            }
        }
    }
}
