using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            this._ctx = ctx;
            this._hosting = hosting;
            this._userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("kenneth.laforteza@pointwest.com.ph");

            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Kenneth",
                    LastName = "Laforteza",
                    Email = "kenneth.laforteza@pointwest.com.ph",
                    UserName = "kenneth.laforteza@pointwest.com.ph"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create a new user in seeder");
                }
            }

            if (!_ctx.Products.Any())
            {
                // Add Products
                // Need to create sample data
                string file = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                string jsonData = File.ReadAllText(file);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonData);
                _ctx.Products.AddRange(products);

                // Add Order
                if (!_ctx.Orders.Any())
                {
                    Order order = new Order()
                    {
                        OrderDate = DateTime.Now,
                        OrderNumber = "1N98VJAF8",
                        User = user,
                        Items = new List<OrderItem>()
                        {
                            new OrderItem()
                            {
                                Product = products.First(),
                                Quantity = 5,
                                UnitPrice = products.First().Price
                            }
                        }
                    };
                }

                _ctx.SaveChanges();
            }
        }
    }
}
