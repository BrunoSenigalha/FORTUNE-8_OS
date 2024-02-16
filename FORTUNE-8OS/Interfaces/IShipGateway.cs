using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FORTUNE_8OS.Entities;

namespace FORTUNE_8OS.Interfaces
{
    public interface IShipGateway
    {
        void PostShipDatabase(Ship ship);
        IEnumerable<Ship> GetShipList();
       // bool IsShipExistsOnDatabase();
    }
}
