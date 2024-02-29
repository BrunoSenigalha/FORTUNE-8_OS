using FORTUNE_8OS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Gateways
{
    public interface IShoppingServices
    {
        Dictionary<int, decimal> PromotionGenerator(int vetLenght);
        void CompletePurchase();
    }
}
