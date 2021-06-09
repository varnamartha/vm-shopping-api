using System.ComponentModel.DataAnnotations;

namespace vm_shopping_data_access.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}