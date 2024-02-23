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
    public class ScrapGateway : IScrapGateway
    {
        public void PostScrap(Scrap scrap)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "INSERT INTO Scrap (IdItem, Quantity, ScrapDate, Credits)" +
                    "VALUES (@IdItem, @Quantity, @ScrapDate, @Credits)";
                connection.Execute(query, scrap);
            }
        }

        public IEnumerable<Scrap> GetScraps()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = @"SELECT s.*, i.* FROM Scrap s JOIN Items i ON s.IdItem = i.Id";
                var result = connection.Query<Scrap, Item, Scrap>(
                    query,
                    (scrap,item) =>
                    {
                        scrap.Item = item;
                        return scrap;
                    },
                    splitOn: "Id"
                    ).ToList();

                return result;
            }
        }
    }
}