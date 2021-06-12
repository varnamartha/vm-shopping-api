using System;

namespace vm_shopping_data_access.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string GatewayPaymentId { get; set; }
        public string GatewayUrlRedirection { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public Status Status { get; set; }
    }
}