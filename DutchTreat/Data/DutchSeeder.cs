﻿using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
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

    public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting)
    {
      this._ctx = ctx;
      this._hosting = hosting;
    }

    public void Seed()
    {
      _ctx.Database.EnsureCreated();

      if (!_ctx.Products.Any())
      {
        // Need to create sample data
        var file = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
        var jsonData = File.ReadAllText(file);
        var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonData);
        _ctx.Products.AddRange(products);

        var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();

        if (order != null)
        {
          order.Items = new List<OrderItem>()
          {
            new OrderItem()
            {
              Product = products.First(),
              Quantity = 5,
              UnitPrice = products.First().Price
            }
          };
        }

        _ctx.SaveChanges();
      }
    }
  }
}