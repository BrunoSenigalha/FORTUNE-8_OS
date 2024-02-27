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
    internal class ProductGateway : IProductGateway
    {
        public void PostProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "INSERT INTO Products (Name, Description, Quantity, Price, Category) " +
                      "VALUES (@Name, @Description, @Quantity, @Price, @Category);";

                connection.Execute(query, product);
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "SELECT * FROM Products";
                IEnumerable<Product> products = connection.Query<Product>(query);

                return products;
            }
        }

        public void UpdateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "UPDATE Products SET Name = @Name, Credits = @Credits WHERE Id = @Id";
                connection.Execute(query, product);
            }
        }

        public void DeleteProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDatabase.ConnectionString()))
            {
                connection.Open();

                string query = "DELETE Products Where Id = @Id";
                connection.Execute(query, product);
            }
        }
    }
}
