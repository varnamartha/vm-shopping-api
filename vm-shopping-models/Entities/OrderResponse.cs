
namespace vm_shopping_models.Entities
{
    public class OrderResponse
    {
        public string URLRedirection { get; set; }
        public int ShoppingOrderId { get; set; }
        public StatusResponse Status { get; set; }
        public ProductResponse Product { get; set; }
    }
}