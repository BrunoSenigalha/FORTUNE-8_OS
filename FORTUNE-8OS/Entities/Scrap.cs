using FORTUNE_8OS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Entities
{
    public class Scrap
    {
        public Scrap(int quantity, DateTime scrapDate, decimal credits, Item item)
        {
            DomainExceptionValidation.When(quantity <= 0, "The field quantity can't be lass than or equal zero");
            DomainExceptionValidation.When(credits < 0, "The field credits can't be less than zero.");

            IdItem = item.Id;
            Quantity = quantity;
            ScrapDate = scrapDate;
            Credits = credits;
            Item = item;
        }

        public Scrap(int id, int idItem, int quantity, DateTime scrapDate, decimal credits)
        {
            Id = id;
            IdItem = idItem;
            Quantity = quantity;
            ScrapDate = scrapDate;
            Credits = credits;
        }

        public int Id { get; set; }
        public int IdItem { get; set; }
        public int Quantity { get; set; }
        public DateTime ScrapDate { get; set; }
        public decimal Credits { get; set; }
        public Item Item { get; set; }
    }
}
