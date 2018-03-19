using Smart.Core.Extensions;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Smart.Core.WCF
{
    public class WcfClient<TService> : IDisposable
    {
        public string Address { get; set; }
        public Binding Binding { get; set; }
        public Type CallbackType { get; set; }

        ChannelFactory<TService> _ChannelFactory;
        protected ChannelFactory<TService> ChannelFactory
        {
            get
            {
                return _ChannelFactory ?? (_ChannelFactory = new ChannelFactory<TService>(Binding, Address));
            }
        }

        DuplexChannelFactory<TService> _DuplexChannelFactory;
        protected DuplexChannelFactory<TService> DuplexChannelFactory
        {
            get
            {
                return _DuplexChannelFactory ?? (_DuplexChannelFactory = new DuplexChannelFactory<TService>(CallbackType, Binding, Address));
            }
        }

        public WcfClient(string basicAddress, Binding binding = null, Type callbackType = null)
        {
            this.Address = $"{basicAddress.TrimEnd('/')}/{typeof(TService).Name.Substring(1)}";
            this.Binding = binding ?? CreateNetTcpBinding();
            this.CallbackType = callbackType;
        }

        public TService CreateService()
        {
            return ChannelFactory.CreateChannel();
        }

        public TService CreateDuplexService(object callback)
        {
            var context = new InstanceContext(callback);
            return DuplexChannelFactory.CreateChannel(context);
        }

        NetTcpBinding CreateNetTcpBinding()
        {
            var tcpBinding = new NetTcpBinding(SecurityMode.None);
            tcpBinding.MaxBufferPoolSize = int.MaxValue;
            tcpBinding.MaxReceivedMessageSize = int.MaxValue;
            tcpBinding.MaxBufferSize = int.MaxValue;
            return tcpBinding;
        }

        public void Dispose()
        {
            _ChannelFactory?.Close();
            _DuplexChannelFactory?.Close();
        }
    }
}
