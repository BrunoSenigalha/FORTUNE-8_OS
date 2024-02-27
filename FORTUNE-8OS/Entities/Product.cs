using FORTUNE_8OS.Entities.Enums;
using FORTUNE_8OS.Exceptions;
using System.Xml.Linq;

namespace FORTUNE_8OS.Entities
{
    public class Product
    {
        public Product(string name, string description, int quantity, decimal price, CategoryEnum category)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(name), "The field name can't be empty.");
            DomainExceptionValidation.When(name.Length < 3, "The name must have a minimum of three characters.");
            DomainExceptionValidation.When(string.IsNullOrEmpty(description), "The field description can't be empty.");
            DomainExceptionValidation.When(quantity <= 0, "The quantity can't be lass than or equal to 0");
            DomainExceptionValidation.When(price <= 0, "The quantity can't be lass than or equal to 0");

            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            Category = category;
        }

        public Product(int id, string name, string description, int quantity, decimal price, CategoryEnum category)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            Category = category;
        }

        public int Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public CategoryEnum Category { get; set; }

        public void UpdateQuantity(int quantity)
        {
            DomainExceptionValidation.When(quantity < 0, "The quantity can't be less than 0");
            this.Quantity += quantity;
        }
    }
}
