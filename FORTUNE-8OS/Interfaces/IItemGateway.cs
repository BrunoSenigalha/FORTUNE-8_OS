using FORTUNE_8OS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Interfaces
{
    public interface IItemGateway
    {
        public void PostItem(Item item);
        public IEnumerable<Item> GetItemList();

        void UpdateItem(Item item);
    }
}
