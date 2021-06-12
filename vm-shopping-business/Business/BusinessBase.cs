using vm_shopping_data_access;

namespace vm_shopping_business.Business
{
    public class BusinessBase
    {
        public readonly ShoppingDBContext shoppingDBContext;

        public BusinessBase(ShoppingDBContext shoppingDBContext)
        {
            this.shoppingDBContext = shoppingDBContext;
        }
    }
}
