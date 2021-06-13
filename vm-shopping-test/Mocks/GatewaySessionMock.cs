using vm_shopping_business.Interfaces;

namespace vm_shopping_test.Mocks
{
    public class GatewaySessionMock : IGatewaySession
    {
        public bool DidFailed { get; set; }

        PlacetoPay.Integrations.Library.CSharp.PlacetoPay IGatewaySession.Session()
        {
            var placetoPayMock = new PlacetoPayMock
            {
                didFailed = DidFailed
            };
            return placetoPayMock;
        }
    }
}
