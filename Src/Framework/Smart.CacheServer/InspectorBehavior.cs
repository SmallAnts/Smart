using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Etrack.Wcf
{
    /// <summary>
    /// 
    /// </summary>
    public class InspectorBehavior : BehaviorExtensionElement, IEndpointBehavior, IServiceBehavior, IOperationBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(InspectorBehavior); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new InspectorBehavior();
        }
        
        #region IEndpointBehavior
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new MessageInspector());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new MessageInspector());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion

        #region IServiceBehavior
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //foreach (var endPoint in serviceHostBase.Description.Endpoints)
            //{
            //    endPoint.Behaviors.Add(this);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            
        }
        #endregion

        #region IOperationBehavior
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="clientOperation"></param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Parent.MessageInspectors.Add(new MessageInspector());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="dispatchOperation"></param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Parent.MessageInspectors.Add(new MessageInspector());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationDescription"></param>
        public void Validate(OperationDescription operationDescription)
        {
            
        }
        #endregion
    }
}
