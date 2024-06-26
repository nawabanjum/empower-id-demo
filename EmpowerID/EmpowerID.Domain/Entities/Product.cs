﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpowerID.Domain.Entities
{
    public class Product(string name, string description, string imageUrl, decimal price)
    {
        [Column("product_id")]
        public int Id { get; private set; }

        [Column("product_name")]
        public string Name { get; private set; } = name;

        [Column("description")]
        public string Description { get; private set; } = description;
        [Column("image_url")]
        public string ImageUrl { get; private set; } = imageUrl;
        [Column("price")]
        [Precision(18, 2)]
        public decimal Price { get; private set; } = price;
        [Column("date_added")]
        public DateTime DateAdded { get; private set; }
        [Column("category_id")]
        public int CategoryId { get; private set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}