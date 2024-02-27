using FORTUNE_8OS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Interfaces
{
    public interface IProductGateway
    {
        void PostProduct(Product product);
        IEnumerable<Product> GetProducts();
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
