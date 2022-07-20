using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace console_client
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Producer { get; set; }
        public int CostPrice { get; set; }
        public int Price { get; set; }
    }
    public class SportShopDb
    {
        private SqlConnection connection;

        public SportShopDb()
        {
            string connStr = @"Data Source={server_name};Initial Catalog={db_name};Integrated Security=True;Connect Timeout=2;";
            connection = new SqlConnection(connStr);

            connection.Open();
        }

        /*      CRUD Interface
         * [C]reate
         * [R]ead
         * [U]pdate
         * [D]elete
         */

        public void Create(Product product)
        {
            string cmdText = $@"INSERT INTO Products
                               VALUES ('{product.Name}',
                                       '{product.Type}', 
                                        {product.Quantity}, 
                                        {product.CostPrice}, 
                                       '{product.Producer}', 
                                        {product.Price})";

            SqlCommand command = new SqlCommand(cmdText, connection);

            command.ExecuteNonQuery();
        }
        public List<Product> GetAll()
        {
            string cmdText = @"select * from Products";

            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                products.Add(new Product()
                {
                    Id = (int)reader[0],
                    Name = (string)reader[1],
                    Type = (string)reader[2],
                    Quantity = (int)reader[3],
                    CostPrice = (int)reader[4],
                    Producer = (string)reader[5],
                    Price = (int)reader[6]
                });
            }
            reader.Close();

            return products;
        }
        public Product Get(int id)
        {
            string cmdText = $@"select TOP 1 * from Products where Id = {id}";

            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            Product product = new Product();

            while (reader.Read())
            {
                product.Id = (int)reader[0];
                product.Name = (string)reader[1];
                product.Type = (string)reader[2];
                product.Quantity = (int)reader[3];
                product.CostPrice = (int)reader[4];
                product.Producer = (string)reader[5];
                product.Price = (int)reader[6];
            }
            reader.Close();

            return product;
        }

        public void Update(Product product)
        {
            string cmdText = $@"UPDATE Products
                                SET Name = '{product.Name}',
                                    Type = '{product.Type}', 
                                    Quantity = {product.Quantity}, 
                                    CostPrice = {product.CostPrice}, 
                                    Producer = '{product.Producer}', 
                                    Price = {product.Price}
                                WHERE Id = {product.Id}";

            SqlCommand command = new SqlCommand(cmdText, connection);

            command.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            string cmdText = $@"DELETE Products WHERE Id = {id}";

            SqlCommand command = new SqlCommand(cmdText, connection);

            command.ExecuteNonQuery();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            SportShopDb db = new SportShopDb();

            // -=-=-=-=-=-=-=-=- Create -=-=-=-=-=-=-=-=-
            var pr = new Product()
            {
                Name = "Espander",
                Type = "Sport Equipment",
                Quantity = 4,
                CostPrice = 560,
                Producer = "China",
                Price = 780
            };

            db.Create(pr);

            // -=-=-=-=-=-=-=-=- Read -=-=-=-=-=-=-=-=-
            List<Product> products = db.GetAll();

            foreach (var p in products)
            {
                Console.WriteLine(p.Name);
            }

            Product pr2 = db.Get(8);

            Console.WriteLine("Product: " + pr2.Name);

            // -=-=-=-=-=-=-=-=- Update -=-=-=-=-=-=-=-=-

            pr2.Price += 120;
            pr2.CostPrice += 90;

            db.Update(pr2);

            // -=-=-=-=-=-=-=-=- Delete -=-=-=-=-=-=-=-=-
            db.Delete(10);
        }
    }
}
