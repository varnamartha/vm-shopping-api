using vm_shopping_data_access;
using vm_shopping_data_access.Entities;

namespace vs_shopping_test.DataBase
{
    public static class StatusDB
    {
        public static ShoppingDBContext CreateOrderDB(ShoppingDBContext context)
        {
            Seed(context);
            return context;
        }

        private static void Seed(ShoppingDBContext context)
        {
            var status = new[]
            {
                new Status{Id=4,Description="status1"},
                new Status{Id=5,Description="status2"}

            };

            context.Status.AddRange(status);
            context.SaveChanges();
        }
    }
}
