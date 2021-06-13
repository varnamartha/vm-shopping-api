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
    public class ProductBusinessTest
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
        public void CreateProductSuccededTest()
        {
            InitializeBusinessClass();

            var product = new ProductRequest
            {
                Currency = "USD",
                Name = "TV3",
                Description = "TV3 90'",
                Price = 50,
                ProductId = 6
            };
           

            var result = productBusiness.SaveProduct(product)?.Result;
            Assert.AreEqual(6, result.ProductId);
        }

        [Test]
        public void CreateProductFailedTest()
        {
            InitializeBusinessClass();

            var product = new ProductRequest
            {
                Currency = "USD",
                Name = "TV34",
                Description = "TV34 90'",
                Price = 50,
                ProductId = 78
            };


            var result = productBusiness.SaveProduct(product)?.Result;
            Assert.AreNotEqual(78,result.ProductId);
        }
    }
}
