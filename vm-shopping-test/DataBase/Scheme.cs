using Microsoft.EntityFrameworkCore;
using vm_shopping_data_access;

namespace vs_shopping_test.DataBase
{
    public static class Scheme
    {
        public static ShoppingDBContext CreateContextMock()
        {
            var options = new DbContextOptionsBuilder<ShoppingDBContext>()
                .UseInMemoryDatabase(databaseName: "Status")
                .UseInMemoryDatabase(databaseName: "Order")
                .UseInMemoryDatabase(databaseName: "Product")
                .Options;
            var context = new ShoppingDBContext(options);
           
            context = ProductDB.CreateOrderDB(context);
            context = StatusDB.CreateOrderDB(context);
            context = OrderDB.CreateOrderDB(context);
            return context;
        }
    }
}
