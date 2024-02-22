using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Entities
{
    public class Scrap
    {
        public Scrap(int idItem, int quantity, DateTime scrapDate, decimal credits, Item item)
        {
            IdItem = idItem;
            Quantity = quantity;
            ScrapDate = scrapDate;
            Credits = credits;
            Item = item;
        }

        public Scrap(int id, int idItem, int quantity, DateTime scrapDate, decimal credits, Item item)
        {
            Id = id;
            IdItem = idItem;
            Quantity = quantity;
            ScrapDate = scrapDate;
            Credits = credits;
            Item = item;
        }

        public int Id { get; set; }
        public int IdItem { get; set; }
        public int Quantity { get; set; }
        public DateTime ScrapDate { get; set; }
        public decimal Credits { get; set; }
        public Item Item { get; set; }
    }
}
