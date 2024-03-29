﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;

namespace FORTUNE_8OS.Services
{
    public class ShipService(IShipGateway shipGateway)
    {
        private readonly IShipGateway _shipGateway = shipGateway;

        public void CreateShip()
        {
            if (GetShip() is null)
            {
                Ship shipOne = new("ShipOne", 60M);

                _shipGateway.PostShipDatabase(shipOne);
            }
        }

        public Ship? GetShip()
        {
            IEnumerable<Ship> shipDatabase = _shipGateway.GetShipList();
            var ship = shipDatabase.FirstOrDefault();

            return ship;
        }

        public void UpdateShip(Ship ship)
        {
            _shipGateway?.UpdateShip(ship);
        }

        public string PrintShipCredits()
        {
            var ship = GetShip();

            return ship != null ? $"\\{ship.Credits:F2}" : $"\\0";
        }
    }
}