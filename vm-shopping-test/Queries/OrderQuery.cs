using System.Threading.Tasks;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_models.Entities;

namespace vm_shopping_test.DataBase
{
    public class OrderQuery
    {
        private readonly ShoppingDBContext context;

        public readonly IOrderBusiness orderBusiness;

        public OrderQuery(IOrderBusiness orderBusiness, ShoppingDBContext context)
        {
            this.context = context;
            this.orderBusiness = orderBusiness;
        }      

        public Task<OrderResponse> GetOrderByIdExecute(int orderId)
        {
            return orderBusiness.GetOrder(orderId);

        }
    }
}
