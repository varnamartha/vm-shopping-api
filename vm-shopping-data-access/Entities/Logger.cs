using System;
using System.Collections.Generic;
using System.Text;

namespace vm_shopping_data_access.Entities
{
    public class Logger
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string LogLevel { get; set; }
        public DateTime LogDate { get; set; }
    }
}
