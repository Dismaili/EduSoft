using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace WebApplicationIntern2.NBRMWebService
{
    [ServiceContract(Namespace = "http://www.nbrm.mk/")]
    public interface IKLService
    {
        [OperationContract]
        Task<string> GetExchangeRatesAsync();
        
        [OperationContract]
        Task<string> GetExchangeRatesDAsync(string datum);
    }

    public partial class KLServiceSoapClient : ClientBase<IKLService>, IKLService
    {
        public KLServiceSoapClient() : base(GetDefaultBinding(), GetDefaultEndpointAddress())
        {
        }

        public KLServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
        {
        }

        public async Task<string> GetExchangeRatesAsync()
        {
            return await Channel.GetExchangeRatesAsync();
        }

        public async Task<string> GetExchangeRatesDAsync(string datum)
        {
            return await Channel.GetExchangeRatesDAsync(datum);
        }

        public virtual async Task OpenAsync()
        {
            await Task.Factory.FromAsync(((ICommunicationObject)(this)).BeginOpen(null, null), new Action<IAsyncResult>(((ICommunicationObject)(this)).EndOpen));
        }

        public new virtual async Task CloseAsync()
        {
            await Task.Factory.FromAsync(((ICommunicationObject)(this)).BeginClose(null, null), new Action<IAsyncResult>(((ICommunicationObject)(this)).EndClose));
        }

        private static Binding GetDefaultBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.AllowCookies = true;
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            return binding;
        }

        private static EndpointAddress GetDefaultEndpointAddress()
        {
            return new EndpointAddress("https://www.nbrm.mk/KLServiceNOV/KLService.asmx");
        }

        private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.KLServiceSoap))
            {
                BasicHttpBinding result = new BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new InvalidOperationException($"Could not find endpoint with name \'{endpointConfiguration}\'.");
        }

        private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.KLServiceSoap))
            {
                return new EndpointAddress("https://www.nbrm.mk/KLServiceNOV/KLService.asmx");
            }
            throw new InvalidOperationException($"Could not find endpoint with name \'{endpointConfiguration}\'.");
        }

        public enum EndpointConfiguration
        {
            KLServiceSoap,
        }
    }
}
