using PlacetoPay.Integrations.Library.CSharp.Entities;
using PlacetoPay.Integrations.Library.CSharp.Message;
using System;
using System.Threading.Tasks;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;
using vm_shopping_business.Interfaces;
using vm_shopping_models.Entities;
using vm_shopping_models.Enum;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
        {
            Amount amount = new Amount(orderRequest.Product.Price, orderRequest.Product.Currency);
            Payment payment = new Payment(orderRequest.Product.Name, orderRequest.Product.Description, amount);

            RedirectRequest request = new RedirectRequest(payment,
                Environment.GetEnvironmentVariable("PlacetoUrlWebhook"),
                Utils.GetLocalIPAddress(),
                Environment.GetEnvironmentVariable("PlacetoUserAgent"),
                DateTime.Now.AddMinutes(5).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));

            RedirectResponse response = gatewaySession.Session().Request(request);
            if (response.IsSuccessful())
            {
                return await SaveOrder(orderRequest, response.ProcessUrl, response.RequestId);
            }

            return new OrderResponse();
        }

        public async Task<OrderResponse> GetOrder(int orderId)
        {
            OrderResponse orderResponse = null;
            try
            {
                var order = shoppingDBContext.Order
                        .Include(x => x.Status)
                        .Include(x => x.Product)
                        .Where(o => o.Id == orderId).FirstOrDefault();

                if (order != null)
                {
                    orderResponse = new OrderResponse();
                    orderResponse.ShoppingOrderId = order.Id;
                    orderResponse.URLRedirection = order.GatewayUrlRedirection;
                    orderResponse.Status = new StatusResponse
                    {
                        Id = order.Status.Id,
                        Status = order.Status.Description
                    };
                    orderResponse.Product = new ProductResponse
                    {
                        ProductId = order.Product.Id,
                        Name = order.Product.Name,
                        Description = order.Product.Description,
                        Price = order.Product.Price
                    };
                }
            }
            catch (Exception ex)
            {
                //ToDo: Log
            }
            return orderResponse;
        }

        internal async Task<OrderResponse> SaveOrder(OrderRequest orderRequest, string GatewayUrlRedirection, string GatewayPaymentId)
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
                    orderResponse.Status = new StatusResponse
                    {
                        Id = (int)OrderStatus.CREATED,
                        Status = Enum.GetName(typeof(OrderStatus), OrderStatus.CREATED)
                    };
                    orderResponse.Product = new ProductResponse
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price
                    };
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
