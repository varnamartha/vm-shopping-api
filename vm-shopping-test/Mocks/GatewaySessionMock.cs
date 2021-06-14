using vm_shopping_business.Interfaces;

namespace vm_shopping_test.Mocks
{
    public class GatewaySessionMock : IGatewaySession
    {
        public bool IsSuccessfullResponse { get; set; }

        PlacetoPay.Integrations.Library.CSharp.PlacetoPay IGatewaySession.Session()
        {
            var placetoPayMock = new PlacetoPayMock
            {
                IsSuccessfullResponse = IsSuccessfullResponse
            };
            return placetoPayMock;
        }
    }
}
