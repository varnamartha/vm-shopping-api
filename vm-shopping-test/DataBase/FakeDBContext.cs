using Microsoft.EntityFrameworkCore;
using vm_shopping_data_access;

namespace vs_shopping_test.DataBase
{
    public static class FakeDBContext
    {
        private static ShoppingDBContext context;

        public static ShoppingDBContext GetDBContext()
        {
            if(context==null)
            {
                context = CreateContextMock();
            }
            return context;
        }
        private static ShoppingDBContext CreateContextMock()
        {
            var options = new DbContextOptionsBuilder<ShoppingDBContext>()
                .UseInMemoryDatabase(databaseName: "Status")
                .UseInMemoryDatabase(databaseName: "Order")
                .UseInMemoryDatabase(databaseName: "Product")
                .Options;
            var context = new ShoppingDBContext(options);
           
            context = ProductDB.CreateProductDB(context);
            context = StatusDB.CreateStatusDB(context);
            context = OrderDB.CreateOrderDB(context);
            return context;
        }
    }
}
