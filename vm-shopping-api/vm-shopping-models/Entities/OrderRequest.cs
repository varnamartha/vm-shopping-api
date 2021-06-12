using System;
using System.Collections.Generic;
using System.Text;

namespace vm_shopping_models.Entities
{
    public class OrderRequest 
    {
        public ClientRequest Client { get; set; }
        public ProductRequest Product { get; set; }
    }
}
