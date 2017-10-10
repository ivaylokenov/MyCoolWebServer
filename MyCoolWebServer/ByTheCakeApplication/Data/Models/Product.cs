namespace MyCoolWebServer.ByTheCakeApplication.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ImageUrl { get; set; }

        public List<OrderProduct> Orders { get; set; } = new List<OrderProduct>();
    }
}
