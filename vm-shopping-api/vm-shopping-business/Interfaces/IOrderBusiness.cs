using System.Threading.Tasks;
using vm_shopping_models.Entities;

namespace vm_shopping_business.Interfaces
{
    public interface IOrderBusiness
    {
        Task<OrderResponse> CreateOrder(OrderRequest orderRequest);
        Task<OrderResponse> GetOrder(int orderId);
    }
}