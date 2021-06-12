using PlacetoPay.Integrations.Library.CSharp.Contracts;
using System;
using vm_shopping_business.Interfaces;

namespace vm_shopping_business.Business
{
    public class GatewaySession : IGatewaySession
    {
        PlacetoPay.Integrations.Library.CSharp.PlacetoPay shoppingGatewaySession;
        public PlacetoPay.Integrations.Library.CSharp.PlacetoPay Session()
        {
            if (shoppingGatewaySession == null)
            {
                //ToDo: Take these data from Configuration
                shoppingGatewaySession = new PlacetoPay.Integrations.Library.CSharp.PlacetoPay("6dd490faf9cb87a9862245da41170ff2",
                                      "024h1IlD",
                                      new Uri("https://test.placetopay.com/redirection/"),
                                      Gateway.TP_REST);
            }
            return shoppingGatewaySession;
        }
    }
}
