using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MWRepo.Models;
using System.Data.SqlClient;

namespace MWRepo.Factories
{
    public class ProductFac:AutoFac<Product>
    {
        public List<Product>Search(string searchInput, params string[] parameters)
        {
            string sqlQuery = "";

            sqlQuery += "SELECT * FROM Product WHERE ";

            for(int i =0; i <parameters.Length; i++)
            {
                sqlQuery += parameters[i] + " LIKE '%" + searchInput +"%'";
                if(i +1 < parameters.Length)
                {
                    sqlQuery += " OR ";
                }
            }
            
            SqlCommand cmd = new SqlCommand(sqlQuery, Conn.CreateConnection());
            SqlDataReader reader = cmd.ExecuteReader();

            List<Product> searchResult = new List<Product>();
            Product product;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    product = new Product();
                    product.ID = (int)reader["ID"];
                    product.Name = (string)reader["Name"];
                    product.Description = (String)reader["Description"];
                    product.DateAdded = (DateTime)reader["DateAdded"];
                    product.Active = (bool)reader["Active"];
                    searchResult.Add(product);

                }
            }

            cmd.Connection.Close();
            cmd.Dispose();

            return searchResult;
        }
    }
}
