using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AtirUz.DTOs;

namespace AtirUz.DTOs
{
    public class OrderCreateDTO
    {
        public int CustomerId { get; set; }
        public List<OrderItemCreateDTO> OrderItems { get; set; }  // For creating orders
        public string PaymentStatus { get; set; }
    }
}
