using FORTUNE_8OS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Interfaces
{
    public interface IPaymentService
    {
        string ValidatePurchase(Product requiredProduct, int quantity);
        public void CompletePurchase(Ship ship, Product product, int quantity, decimal total);
    }
}
