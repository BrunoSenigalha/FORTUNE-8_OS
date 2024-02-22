using Dapper;
using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using FORTUNE_8OS.Utilitaries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Gateways
{
    public class ItemGateway : IItemGateway
    {
        internal ItemService _itemService;

        public void PostItem(Item item)
        {
            using (SqlConnection connection = new(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "INSERT INTO Items (Name, Credits) " +
                    "VALUES (@Name, @Credits);";
                connection.Execute(query, item);
            }
        }

        public IEnumerable<Item> GetItemList()
        {
            using (SqlConnection connection = new(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "SELECT * FROM Items;";
                IEnumerable<Item> items = connection.Query<Item>(query);

                return items;
            }
        }

        public void UpdateItem(Item item)
        {
            using (SqlConnection connection = new(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "UPDATE Items SET Name=@Name, Credits=@Credits WHERE Id = @Id;";
                connection.Execute(query, item);
            }
        }

        public void DeleteItem(Item item)
        {
            using (SqlConnection connection = new(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "DELETE Items WHERE Id = @Id";
                connection.Execute(query, item);
            }
        }
    }
}
