using System;
using System.Collections.Generic;
using System.Text;

namespace vm_shopping_models.Entities
{
    public class PaymentNotificationRequest
    {
        public StatusNotificationRequest status { get; set; }
        public int requestId { get; set; }
        public string reference { get; set; }
        public string signature { get; set; }
    }
}
