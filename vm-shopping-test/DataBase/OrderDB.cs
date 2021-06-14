using System.Linq;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;

namespace vs_shopping_test.DataBase
{
    public static class OrderDB
    {
        public static ShoppingDBContext CreateOrderDB(ShoppingDBContext context)
        {           
            Seed(context);
            return context;
        }

        private static void Seed(ShoppingDBContext context)
        {
           var product= context.Product.Where(x => x.Id == 4).FirstOrDefault();
           var status= context.Status.Where(x => x.Id == 4).FirstOrDefault();

            var orders = new[]
            {
                new Order{Id=4,ProductId=40, Product=product,Status= status },
                new Order{Id=5,ProductId=50,Product=product,Status= status }
            };

            context.Order.AddRange(orders);
            context.SaveChanges();
        }
    }
}
