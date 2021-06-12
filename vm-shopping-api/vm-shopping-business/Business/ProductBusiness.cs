using System;
using System.Linq;
using System.Threading.Tasks;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;
using vm_shopping_models.Entities;

namespace vm_shopping_business.Business
{
    public class ProductBusiness : BusinessBase, IProductBusiness
    {
        public ProductBusiness(ShoppingDBContext shoppingDBContext) : base(shoppingDBContext)
        {
        }

        public async Task<ProductResponse> SaveProduct(ProductRequest product)
        {
            var productResponse = new ProductResponse();
            try
            {
                using (shoppingDBContext)
                {
                    var currentProduct = shoppingDBContext.Product.Where(t => t.Id == product.ProductId).FirstOrDefault();
                    if (currentProduct == null)
                    {
                        Product newProduct = new Product
                        {
                            Name = product.Name,
                            Price = product.Price,
                            Description = product.Description
                        };
                        shoppingDBContext.Add(newProduct);
                        await shoppingDBContext.SaveChangesAsync();
                        currentProduct = newProduct;
                    }

                    productResponse.ProductId = currentProduct.Id;
                    productResponse.Name = currentProduct.Name;
                    productResponse.Description = currentProduct.Description;
                    productResponse.Price = currentProduct.Price;
                }
            }
            catch (Exception ex)
            {
                //ToDo: Log
            }
            return productResponse;
        }
    }
}
