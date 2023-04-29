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
    if (prices.Count == 0) {
      return NotFound();
    }
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

      // If there is a gap between the current Price and the Previous price
      if (current.ValidFrom > previous.ValidUntil) {
        previous = FillGap(previous, current, lowestPrices, sortedPrices, optimizedPrices);
        continue;
      }
      
      // If the current unitPrice is lower than the previous unitPrice
      if (current.UnitPrice < previous.UnitPrice) {
        optimizedPrices.Last().ValidUntil = current.ValidFrom;
        optimizedPrices.Add(current.Clone());
      }
      
      // If we are at the end of the list, go through the remainders
      else if (done) {
        var temp = current.Clone();
        temp.ValidFrom = optimizedPrices.Last().ValidUntil;
        optimizedPrices.Add(temp);
      }
      
      previous = current;
      if (sortedPrices.Count != 0) continue;
      sortedPrices.AddRange(lowestPrices.Where(entry => entry.ValidUntil > optimizedPrices.Last().ValidUntil));
      done = true;
    }


    optimizedPrices.ForEach(p => p.ValidUntil = p.ValidUntil == DateTime.MaxValue ?  null : p.ValidUntil);
    return optimizedPrices;
  }

  /// <summary>Find a Price to fill the gap with. If none is found, add the current one.</summary>
  /// <param name="previous">The previous Price</param>
  /// <param name="current">The current Price</param>
  /// <param name="lowestPrices">The list of prices sorted by unitPrice</param>
  /// <param name="sortedPrices">The list of prices sorted by ValidFrom, ValidUntil and unitPrice</param>
  /// <param name="optimizedPrices">The list of optimized prices to add to</param>
  /// <returns>The added Price object</returns>
  private Price FillGap(Price previous, Price current , List<Price> lowestPrices, List<Price> sortedPrices, List<Price> optimizedPrices) {
    foreach (var entry in lowestPrices) {
      if (!(entry.ValidFrom <= previous.ValidUntil) || !(entry.ValidUntil > previous.ValidUntil)) continue;
      optimizedPrices.Add(entry.Clone());
      sortedPrices.Insert(0, current);
      return entry.Clone();
    }
    optimizedPrices.Add(current.Clone());
    return current;
  }

  private bool PriceExists(int id) {
    return (_context.Price?.Any(e => e.PriceValueId == id)).GetValueOrDefault();
  }
}