using vm_shopping_data_access;
using vm_shopping_data_access.Entities;

namespace vs_shopping_test.DataBase
{
    public static class ProductDB
    {
        public static ShoppingDBContext CreateOrderDB(ShoppingDBContext context)
        {          
            Seed(context);
            return context;
        }

        private static void Seed(ShoppingDBContext context)
        {
            var products = new[]
            {
                new Product{Id=4,Name="product1",Price=40,Description="product1"},
                new Product{Id=6,Name="product2",Price=50,Description="product2"}
            };

            context.Product.AddRange(products);
            context.SaveChanges();
        }
    }
}
