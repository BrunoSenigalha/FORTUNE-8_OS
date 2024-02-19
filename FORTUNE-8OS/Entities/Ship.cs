using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Entities
{
    public class Ship
    {
        public Ship(string name, decimal credits)
        {
            Name = name;
            Credits = credits;
        }
        public Ship(int id, string name, decimal credits)
        {
            Id = id;
            Name = name;
            Credits = credits;

        }
        public int Id { get; }
        public string Name { get; private set; }
        public decimal Credits { get; set; }

    }
}
