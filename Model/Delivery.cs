using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }
        public string? SalesOrderNo { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? DeliveryNo { get; set; }
        public string? ItemCode { get; set; }
        public string? DocumentType { get; set; }
        public decimal Quantity { get; set; }
    }
}
