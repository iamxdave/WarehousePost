using System;
using System.ComponentModel.DataAnnotations;

namespace cw5.Models
{
    public class Order
    {
        [Required]
        public int IdOrder { get; set; }
        [Required]
        public int IdProduct { get; set; }
        [Required]
        public int IdWarehouse { get; set; }
        [Required]
        [Range(0, Int16.MaxValue)]
        public int Amount { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }


        override
        public string ToString()
        {
            return "IdOrder=" + IdOrder + '\n'
                +  "IdProduct=" + IdProduct + '\n'
                +  "IdWarehouse= " + IdWarehouse + '\n'
                +  "Amount= " + Amount + '\n'
                +  "CreatedAt=" + CreatedAt; 
        }
    }
}