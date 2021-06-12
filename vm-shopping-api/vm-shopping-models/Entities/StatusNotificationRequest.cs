using System;
using System.Collections.Generic;
using System.Text;

namespace vm_shopping_models.Entities
{
    public class StatusNotificationRequest
    {
        public string status { get; set; }
        public string message { get; set; }
        public string reason { get; set; }
        public string date { get; set; }
    }
}
