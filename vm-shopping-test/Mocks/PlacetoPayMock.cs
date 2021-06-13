using System;
using System.Collections.Generic;
using System.Text;
using PlacetoPay.Integrations.Library.CSharp;
using PlacetoPay.Integrations.Library.CSharp.Message;

namespace vm_shopping_test.Mocks
{
    public class PlacetoPayMock : PlacetoPay.Integrations.Library.CSharp.PlacetoPay
    {
        public bool didFailed { get; set; }

        public PlacetoPayMock() : base("", "", new Uri("http://www.placetomock.com"))
        {

        }

        public override RedirectResponse Request(RedirectRequest redirectRequest)
        {
            return didFailed ? SuccesfulRedirectResponse() : FailedRedirectResponse();
        }

        private RedirectResponse SuccesfulRedirectResponse()
        {
            var status = new PlacetoPay.Integrations.Library.CSharp.Entities.Status("OK", "success", "", "2021-06-13T15:43:41-05:00");
            return new RedirectResponse("80", "http://www.placeto.com/pay", status);
        }

        private RedirectResponse FailedRedirectResponse()
        {
            var status = new PlacetoPay.Integrations.Library.CSharp.Entities.Status("FAILED", "fail", "", "2021-06-13T15:43:41-05:00");
            return new RedirectResponse("502", "", status);
        }
    }
}
