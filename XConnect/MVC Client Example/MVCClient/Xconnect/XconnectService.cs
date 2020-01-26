using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Xconnect
{
    public static class XconnectService
    {
        public static Contact GetContact(XConnectClientConfiguration cfg, string source,string Identifier)
        {
            try
            {
                using (var client = new XConnectClient(cfg))
                {
                    IdentifiedContactReference reference = new IdentifiedContactReference(source, Identifier);
                     Contact existingContact = client.Get<Contact>(reference, new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey })
                    {
                        Interactions = new RelatedInteractionsExpandOptions()
                        {

                            StartDateTime = DateTime.MinValue,
                            EndDateTime = DateTime.MaxValue,
                            Limit = int.MaxValue
                        }
                    });

                    if (existingContact!=null && existingContact.Id != null)
                    {
                        return existingContact;
                    }
                    else
                    {
                        Contact newContact = CreateContact(cfg, source, Identifier);
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception in Xconnect Service (GetContact): " + e.Message);  
            }

        }

        public static Contact CreateContact(XConnectClientConfiguration cfg, string source, string CurrentIdentifier)
        {
            try
            {
                using (var client = new XConnectClient(cfg))
                {
                    var Identifier = new ContactIdentifier[]
                    {
                        new ContactIdentifier(source,CurrentIdentifier,ContactIdentifierType.Known)
                    };
                    Contact knownContact = new Contact(Identifier);
                    if (knownContact != null)
                    {
                        return knownContact;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception in Xconnect Service (CreateContact): " + e.Message);
            }
        }
    }
}
