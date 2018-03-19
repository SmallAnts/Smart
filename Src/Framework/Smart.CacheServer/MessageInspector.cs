using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Etrack.Wcf
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        #region IClientMessageInspector
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var contextHeader = new MessageHeader<WcfContext>(WcfContext.Current);
            request.Headers.Add(contextHeader.GetUntypedHeader(WcfContext.ContextHeaderLocalName, WcfContext.ContextHeaderNamespace));
            return null;
        }
        #endregion

        #region IDispatchMessageInspector
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var context = request.Headers.GetHeader<WcfContext>(WcfContext.ContextHeaderLocalName, WcfContext.ContextHeaderNamespace);
            WcfContext.Current = context;
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {

        }
        #endregion
    }
}
