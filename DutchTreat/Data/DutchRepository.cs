using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Data
{
  public class DutchRepository : IDutchRepository
  {
    private readonly DutchContext _context;

    public DutchRepository(DutchContext context, ILogger<DutchRepository>)
    {
      this._context = context;
    }
    public IEnumerable<Product> GetAllProducts()
    {
      return _context.Products.OrderBy(p => p.Title).ToList();
    }

    public IEnumerable<Product> GetProductsByCategory(string category)
    {
      return _context.Products.Where(p => p.Category == category).ToList();
    }

    public bool SaveAll()
    {
      return _context.SaveChanges() > 0;
    }
  }
}
