using data_access;
using data_access.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace console_client
{
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
