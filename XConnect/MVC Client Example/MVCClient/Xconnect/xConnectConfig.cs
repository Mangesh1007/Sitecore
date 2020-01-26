using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Xconnect
{
    public static class xConnectConfig
    {
        public static XConnectClientConfiguration GetXconnectConfigObject()
        {
            CertificateHttpClientHandlerModifierOptions options =

            CertificateHttpClientHandlerModifierOptions.Parse(MVCClient.Xconnect.Contants.XconnectString);

            var certificateModifier = new CertificateHttpClientHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            var collectionClient = new CollectionWebApiClient(new Uri(Contants.Xconnectconfiguration), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri(Contants.Xconnectconfiguration), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri(Contants.XconfigurationClient), clientModifiers, new[] { certificateModifier });

            var cfg = new XConnectClientConfiguration(
                new XdbRuntimeModel(CollectionModel.Model), collectionClient, searchClient, configurationClient);

            return cfg;
        }
    }
}
