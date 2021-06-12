using System;
using System.Collections.Generic;
using System.Text;
using P2P = PlacetoPay.Integrations.Library.CSharp.PlacetoPay;

namespace vm_shopping_business.Interfaces
{
   public interface IGatewaySession
    {
        P2P Session();
    }
}
