using NUnit.Framework;
using vm_shopping_business;
using vm_shopping_business.AutoMapper;
using vm_shopping_business.Business;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_models.Entities;
using vm_shopping_test.Mocks;
using vs_shopping_test.DataBase;

namespace vm_shopping_test.Test
{
    public class OrderBusinessTest
    {
        private IAutoMapperConfig autoMapperConfig;

        private IGatewaySession gatewaySession;

        private IClientBusiness clientBusiness;

        private IProductBusiness productBusiness;

        private IOrderBusiness orderBusiness;

        private ShoppingDBContext context;

        private void InitializeBusinessClass(bool didFailed = false)
        {
            if (context == null)
            {
                context = FakeDBContext.GetDBContext();
            }

            gatewaySession = new GatewaySessionMock
            {
                DidFailed = didFailed
            };

            autoMapperConfig = new AutoMapperConfig();

            clientBusiness = new ClientBusiness(autoMapperConfig, context);

            productBusiness = new ProductBusiness(autoMapperConfig, context);

            orderBusiness = new OrderBusiness(gatewaySession, clientBusiness, productBusiness, autoMapperConfig, context);
        }

        [Test]
        public void GetOrderByIdSuccessTest()
        {
            InitializeBusinessClass();
            var result =  orderBusiness.GetOrder(5)?.Result;

            Assert.AreEqual(5, result?.ShoppingOrderId);
        }

        [Test]
        public void GetOrderByIdFailedTest()
        {
            InitializeBusinessClass();
            Assert.IsNull(orderBusiness.GetOrder(10)?.Result);
        }

        [Test]
        public void CreateOrderSuccededTest()
        {
            InitializeBusinessClass(true);

            var order = new OrderRequest
            {
                Client = new ClientRequest
                {
                    Name = "Juan3",
                    Mail = "juan3@gmail.com",
                    Phone = "091234567"
                },
                Product = new ProductRequest
                {
                    Currency = "USD",
                    Name = "TV3",
                    Description = "TV3 90'",
                    Price = 50,
                    ProductId = 6
                }
            };

            var result = orderBusiness.CreateOrder(order)?.Result;
            Assert.AreNotEqual(0, result.ShoppingOrderId);
        }

        [Test]
        public void CreateOrderFailedTest()
        {
            InitializeBusinessClass();

            var order = new OrderRequest
            {
                Client= new ClientRequest
                {
                    Name= "Juan3",
                    Mail= "juan3@gmail.com",
                    Phone= "091234567"
                },
                Product = new ProductRequest
                {
                    Currency="USD",
                    Name="TV3",
                    Description="TV3 90'",
                    Price=50,
                    ProductId=6
                }
            };

            var result = orderBusiness.CreateOrder(order)?.Result;
            Assert.AreEqual(0,result.ShoppingOrderId);
        }
    }
}
