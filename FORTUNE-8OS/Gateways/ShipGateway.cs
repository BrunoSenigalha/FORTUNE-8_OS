using Dapper;
using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Utilitaries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Gateways
{
    internal class ShipGateway : IShipGateway
    {

        public void PostShipDatabase(Ship ship)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "INSERT INTO Ships (Name, Credits) " +
                    "VALUES (@Name, @Credits)";
                connection.Execute(query, ship);
            }
        }
        public IEnumerable<Ship> GetShipList()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "SELECT * FROM Ships";
                IEnumerable<Ship> ship = connection.Query<Ship>(query);

                return ship;
            }
        }

    }
}
