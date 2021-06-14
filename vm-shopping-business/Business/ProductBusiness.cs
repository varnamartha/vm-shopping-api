using System;
using System.Linq;
using System.Threading.Tasks;
using vm_shopping_business.AutoMapper;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;
using vm_shopping_models.Entities;

namespace vm_shopping_business.Business
{
    public class ProductBusiness : BusinessBase, IProductBusiness
    {
        public readonly IAutoMapperConfig autoMapperConfig;

        public ProductBusiness(IAutoMapperConfig autoMapperConfig, ShoppingDBContext shoppingDBContext) : base(shoppingDBContext)
        {
            this.autoMapperConfig = autoMapperConfig;

        }

        public async Task<ProductResponse> SaveProduct(ProductRequest product)
        {
            var productResponse = new ProductResponse();
            try
            {
                var currentProduct = shoppingDBContext.Product.Where(t => t.Id == product.ProductId).FirstOrDefault();
                if (currentProduct == null)
                {
                    Product newProduct = autoMapperConfig.GetMapper().Map<ProductRequest, Product>(product);

                    shoppingDBContext.Add(newProduct);
                    await shoppingDBContext.SaveChangesAsync();
                    currentProduct = newProduct;
                }

                productResponse = autoMapperConfig.GetMapper().Map<Product, ProductResponse>(currentProduct);
            }
            catch (Exception ex)
            {
                LogError("SaveProduct", ex);
            }
            return productResponse;
        }
    }
}
