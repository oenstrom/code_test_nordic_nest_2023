using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceController : ControllerBase {
  private readonly ApplicationDbContext _context;

  public PriceController(ApplicationDbContext context) {
    _context = context;
  }

  // GET: /Price
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Price>>> GetPrice() {
    // return NotFound();
    // if (_context.Price == null) {
    //   return NotFound();
    // }

    return await _context.Price.ToListAsync();
  }
  
  /// <summary>Get prices by SKU (CatalogEntryCode).</summary>
  /// <param name="sku">The SKU (CatalogEntryCode)</param>
  /// <returns>List of prices.</returns>
  /// <remarks>Path: /Price/:sku</remarks>
  [HttpGet("{sku}")]
  public async Task<ActionResult<IEnumerable<Price>>> GetPrice(string sku) {
    var prices = await _context.Price.Where(p => p.CatalogEntryCode == sku && p.MarketId == "sv").ToListAsync();
    prices.ForEach(p => p.ValidUntil ??= DateTime.MaxValue);
    var lowestPrices = prices.OrderBy(p => p.UnitPrice).ThenByDescending(p => p.ValidUntil).ToList();
    var sortedPrices = prices.OrderBy(p => p.ValidFrom).ThenBy(p => p.ValidUntil).ThenBy(p => p.UnitPrice).ToList();
    
    var optimizedPrices = new List<Price> { sortedPrices.First().Clone() };

    var done = false;
    var previous = sortedPrices.First();
    sortedPrices.RemoveAt(0);
    while (sortedPrices.Count > 0) {
      var current = sortedPrices.First();
      sortedPrices.RemoveAt(0);

      // If there is a gap between the current price and the previous price
      if (current.ValidFrom > previous.ValidUntil) {
        var didNotBreak = true;
        foreach (var entry in lowestPrices) {
          if (entry.ValidFrom <= previous.ValidUntil && entry.ValidUntil > previous.ValidUntil) {
            var temp = entry.Clone();
            optimizedPrices.Add(temp);
            previous = temp;
            sortedPrices.Insert(0, current);
            didNotBreak = false;
            break;
          }
        }
        if (didNotBreak) {
         optimizedPrices.Add(current.Clone());
         previous = current;
        }
        continue;
      }
      
      else if (current.UnitPrice < previous.UnitPrice) {
        optimizedPrices.Last().ValidUntil = current.ValidFrom;
        optimizedPrices.Add(current.Clone());
      }
      
      else if (done) {
        var temp = current.Clone();
        temp.ValidFrom = optimizedPrices.Last().ValidUntil;
        optimizedPrices.Add(temp);
      }
      
      previous = current;
      if (sortedPrices.Count == 0) {
        foreach (var entry in lowestPrices) {
          if (entry.ValidUntil > optimizedPrices.Last().ValidUntil) {
            sortedPrices.Add(entry);
          }
        }
        done = true;
      }
    }


    return optimizedPrices;
  }

  private bool PriceExists(int id) {
    return (_context.Price?.Any(e => e.PriceValueId == id)).GetValueOrDefault();
  }
}