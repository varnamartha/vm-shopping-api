using System.Threading.Tasks;
using vm_shopping_models.Entities;

namespace vm_shopping_business.Interfaces
{
    public interface IProductBusiness
    {
        Task<ProductResponse> SaveProduct(ProductRequest product);
    }
}