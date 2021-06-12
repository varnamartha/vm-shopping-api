using System;
using System.Collections.Generic;
using System.Text;

namespace vm_shopping_data_access.Entities
{
    public class PaymentNotification
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public string Reference { get; set; }
        public string Signature { get; set; }
        public DateTime Date { get; set; }
        public Order Order { get; set; }
    }
}
