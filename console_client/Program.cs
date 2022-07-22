using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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

        public SportShopDb(string serverName, string dbName)
        {
            string connStr = @$"Data Source={serverName};Initial Catalog={dbName};Integrated Security=True;Connect Timeout=2;";
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
                                VALUES (@name, @type, @quantity, @costPrice, @producer, @price)";

            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("name", product.Name);
            command.Parameters.AddWithValue("type", product.Type);
            command.Parameters.AddWithValue("quantity", product.Quantity);
            command.Parameters.AddWithValue("costPrice", product.CostPrice);
            command.Parameters.AddWithValue("producer", product.Producer);
            command.Parameters.AddWithValue("price", product.Price);

            command.ExecuteNonQuery();
        }

        public List<Product> GetAll()
        {
            string cmdText = @"select * from Products";

            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            return this.GetProductsFromDataReader(reader);
        }
        public List<Product> GetAllByName(string name)
        {
            // SQL injection: name = Ball'; drop database SportShop; --
            string cmdText = @$"select * from Products where Name = @name";

            SqlCommand command = new SqlCommand(cmdText, connection);
            // add parameters
            //SqlParameter param1 = new SqlParameter()
            //{
            //    ParameterName = "name",
            //    SqlDbType = System.Data.SqlDbType.NVarChar,
            //    Value = name
            //};
            //command.Parameters.Add(param1);
            // shorten form
            command.Parameters.Add("name", System.Data.SqlDbType.NVarChar).Value = name;

            SqlDataReader reader = command.ExecuteReader();

            return this.GetProductsFromDataReader(reader);
        }
        public Product Get(int id)
        {
            string cmdText = $@"select TOP 1 * from Products where Id = {id}";

            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();

            return this.GetProductsFromDataReader(reader).FirstOrDefault();
        }

        public void Update(Product product)
        {
            string cmdText = $@"UPDATE Products
                                SET Name = @name,
                                    Type = @type, 
                                    Quantity = @quan, 
                                    CostPrice = @cPrice, 
                                    Producer = @prod, 
                                    Price = @price
                                WHERE Id = {product.Id}";

            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.AddWithValue("name", product.Name);
            command.Parameters.AddWithValue("type", product.Type);
            command.Parameters.AddWithValue("quan", product.Quantity);
            command.Parameters.AddWithValue("cPrice", product.CostPrice);
            command.Parameters.AddWithValue("prod", product.Producer);
            command.Parameters.AddWithValue("price", product.Price);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            string cmdText = $@"DELETE Products WHERE Id = {id}";

            SqlCommand command = new SqlCommand(cmdText, connection);

            command.ExecuteNonQuery();
        }

        // private methods
        private List<Product> GetProductsFromDataReader(SqlDataReader reader)
        {
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            SportShopDb db = new SportShopDb(@"DESKTOP-O0M8V28\SQLEXPRESS", "SportShop");

            // -=-=-=-=-=-=-=-=- Create -=-=-=-=-=-=-=-=-
            var pr = new Product()
            {
                Name = "Football T-Shirt",
                Type = "Sport Clothes",
                Quantity = 12,
                CostPrice = 950,
                Producer = "Turkey",
                Price = 1200
            };

            db.Create(pr);

            // -=-=-=-=-=-=-=-=- Read -=-=-=-=-=-=-=-=-
            Console.WriteLine("Enter product name to search: ");
            string name = Console.ReadLine();
            List<Product> products = db.GetAllByName(name);

            foreach (var p in products)
            {
                Console.WriteLine($"[{p.Id}]\t{p.Name}\t{p.Price}\t{p.Producer}");
            }

            // get by id
            Product pr2 = db.Get(8);
            Console.WriteLine("Product: " + pr2.Name);

            // -=-=-=-=-=-=-=-=- Update -=-=-=-=-=-=-=-=-

            pr2.Price += 120;
            pr2.CostPrice += 90;

            db.Update(pr2);

            // -=-=-=-=-=-=-=-=- Delete -=-=-=-=-=-=-=-=-
            //db.Delete(10);
        }
    }
}
