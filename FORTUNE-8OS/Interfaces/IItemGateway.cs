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
        void PostItem(Item item);
        IEnumerable<Item> GetItemList();
        void UpdateItem(Item item);
        void DeleteItem(Item item);
    }
}
