using FORTUNE_8OS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Entities
{
    public class Item
    {
        public Item(string name, decimal credits)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(name), "The field name can't be empty.");
            DomainExceptionValidation.When(credits <= 0, "The field credits can't be less than or iqual zero.");

            Name = name;
            Credits = credits;
        }

        public Item(int id, string name, decimal credits)
        {
            Id = id;
            Name = name;
            Credits = credits;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Credits { get; set; }

        public override string ToString()
        {
            return $"{Name} \\ {Credits}";
        }
    }
}
