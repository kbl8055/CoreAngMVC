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
    private readonly ILogger<DutchRepository> _logger;

    public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
    {
      this._context = context;
      this._logger = logger;
    }
    public IEnumerable<Product> GetAllProducts()
    {
      _logger.LogInformation("GetAllProducts called");
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
