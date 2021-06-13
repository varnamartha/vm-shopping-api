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
using System.Collections.Generic;
using vm_shopping_business.AutoMapper;
using AutoMapper;

namespace vm_shopping_business.Business
{
    public class OrderBusiness : BusinessBase, IOrderBusiness
    {
        public readonly IGatewaySession gatewaySession;

        public readonly IClientBusiness clientBusiness;

        public readonly IProductBusiness productBusiness;

        public readonly IAutoMapperConfig autoMapperConfig;


        public OrderBusiness(IGatewaySession gatewaySession, IClientBusiness clientBusiness, IProductBusiness productBusiness, IAutoMapperConfig autoMapperConfig, ShoppingDBContext shoppingDBContext) : base(shoppingDBContext)
        {
            this.gatewaySession = gatewaySession;
            this.clientBusiness = clientBusiness;
            this.productBusiness = productBusiness;
            this.autoMapperConfig = autoMapperConfig;
        }

        public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest)
        {
            try
            {
                Amount amount = new Amount(orderRequest.Product.Price, orderRequest.Product.Currency);
                Payment payment = new Payment(orderRequest.Product.Name, orderRequest.Product.Description, amount);

                RedirectRequest request = new RedirectRequest(payment,
                    Environment.GetEnvironmentVariable("PlacetoUrlWebhook"),
                    Utils.GetLocalIPAddress(),
                    Environment.GetEnvironmentVariable("PlacetoUserAgent"),
                    DateTime.Now.AddMinutes(5).ToString(Utils.dateIso8601Format));

                RedirectResponse response = gatewaySession.Session().Request(request);
                if (response.IsSuccessful())
                {
                    return await SaveOrder(orderRequest, response.ProcessUrl, response.RequestId);
                }
            }
            catch (Exception ex)
            {
                LogError("CreateOrder", ex);
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
                    orderResponse = autoMapperConfig.GetMapper().Map<Order, OrderResponse>(order);   
                }
            }
            catch (Exception ex)
            {
                LogError("GetOrder", ex);
            }
            return orderResponse;
        }

        public async Task<List<OrderResponse>> GetClientOrders(string email)
        {
            List<OrderResponse> orders = new List<OrderResponse>();
            try
            {
                var clientOrders = shoppingDBContext.Order
                        .Include(x => x.Customer)
                        .Include(x => x.Status)
                        .Include(x => x.Product)
                        .Where(o => o.Customer.Mail == email).ToList();

                if (clientOrders.Any())
                {
                    foreach (var clientOrder in clientOrders)
                    {
                        var orderResponse = autoMapperConfig.GetMapper().Map<Order, OrderResponse>(clientOrder);
                        orders.Add(orderResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("GetClientOrders", ex);
            }
            return orders;
        }

        internal async Task<OrderResponse> SaveOrder(OrderRequest orderRequest, string GatewayUrlRedirection, string GatewayPaymentId)
        {
            var orderResponse = new OrderResponse();
            try
            {
                var client = await clientBusiness.SaveClient(orderRequest.Client);
                var product = await productBusiness.SaveProduct(orderRequest.Product);

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

                orderResponse = autoMapperConfig.GetMapper().Map<Order, OrderResponse>(order);                
                orderResponse.Status = new StatusResponse
                {
                    Id = (int)OrderStatus.CREATED,
                    Status = Enum.GetName(typeof(OrderStatus), OrderStatus.CREATED)
                };
                orderResponse.Product = product;
            }
            catch (Exception ex)
            {
                LogError("SaveOrder", ex);
            }
            return orderResponse;
        }
    }
}
