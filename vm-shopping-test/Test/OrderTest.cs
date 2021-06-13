using NUnit.Framework;
using vm_shopping_business;
using vm_shopping_business.AutoMapper;
using vm_shopping_business.Business;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_test.DataBase;
using vs_shopping_test.DataBase;

namespace vm_shopping_test.Test
{
    public class OrderTest
    {
        private IAutoMapperConfig autoMapperConfig;

        private IGatewaySession gatewaySession;

        private IClientBusiness clientBusiness;

        private IProductBusiness productBusiness;

        private IOrderBusiness orderBusiness;

        private void InitializeBusinessClass(ShoppingDBContext context)
        {
            if (autoMapperConfig == null)
            {
                autoMapperConfig = new AutoMapperConfig();
                gatewaySession = new GatewaySession();

                clientBusiness = new ClientBusiness(autoMapperConfig, context);

                productBusiness = new ProductBusiness(autoMapperConfig, context);

                orderBusiness = new OrderBusiness(gatewaySession, clientBusiness, productBusiness, autoMapperConfig, context);
            }
        }

        [Test]
        public void GetOrderById()
        {
            var context = Scheme.CreateContextMock();
            InitializeBusinessClass(context);

            var query = new OrderQuery(orderBusiness, context);

            var result = query.GetOrderByIdExecute(5);

            Assert.AreEqual(5, result.Result.ShoppingOrderId);
        }
    }
}
