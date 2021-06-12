using PlacetoPay.Integrations.Library.CSharp.Entities;
using PlacetoPay.Integrations.Library.CSharp.Message;
using System;
using System.Threading.Tasks;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;
using vm_shopping_business.Interfaces;
using vm_shopping_models.Entities;
using vm_shopping_models.Enum;

namespace vm_shopping_business.Business
{
    public class OrderBusiness : BusinessBase, IOrderBusiness
    {
        public readonly IGatewaySession gatewaySession;

        public readonly IClientBusiness clientBusiness;

        public readonly IProductBusiness productBusiness;

        public OrderBusiness(IGatewaySession gatewaySession, IClientBusiness clientBusiness, IProductBusiness productBusiness, ShoppingDBContext shoppingDBContext) : base(shoppingDBContext)
        {
            this.gatewaySession = gatewaySession;
            this.clientBusiness = clientBusiness;
            this.productBusiness = productBusiness;
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest)
        {
            Amount amount = new Amount(orderRequest.Product.Price, orderRequest.Product.Currency);
            Payment payment = new Payment(orderRequest.Product.Name, orderRequest.Product.Description, amount);

            RedirectRequest request = new RedirectRequest(payment,
                "https://shopping.com/orders/20",//ToDo: Move returnUrl to config
                Utils.GetLocalIPAddress(),
                "API",//ToDo: Move user agent to config
                 DateTime.Now.AddMinutes(5).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));

            RedirectResponse response = gatewaySession.Session().Request(request);
            if (response.IsSuccessful())
            {
                return await SaveOrderAsync(orderRequest, response.ProcessUrl, response.RequestId);
            }

            return new OrderResponse();
        }

        internal async Task<OrderResponse> SaveOrderAsync(OrderRequest orderRequest, string GatewayUrlRedirection, string GatewayPaymentId)
        {
            var orderResponse = new OrderResponse();
            try
            {
                var client = await clientBusiness.SaveClient(orderRequest.Client);
                var product = await productBusiness.SaveProduct(orderRequest.Product);
                using (shoppingDBContext)
                {
                    var order = new Order
                    {
                        ProductId = product.ProductId,
                        CustomerId = client.ClientId,
                        GatewayPaymentId = GatewayPaymentId,
                        GatewayUrlRedirection = GatewayUrlRedirection,
                        StatusId = (int)OrderStatus.CREATED,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    shoppingDBContext.Add(order);
                    await shoppingDBContext.SaveChangesAsync();
                    orderResponse.ShoppingOrderId = order.Id;
                    orderResponse.URLRedirection = order.GatewayUrlRedirection;
                }
            }
            catch (Exception ex)
            {
                //ToDo: Log
            }
            return orderResponse;
        }
    }
}
