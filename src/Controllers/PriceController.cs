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

  /// <summary>Get prices by SKU (CatalogEntryCode).</summary>
  /// <param name="sku">The SKU (CatalogEntryCode)</param>
  /// <returns>List of prices.</returns>
  /// <remarks>Path: /Price/:sku</remarks>
  [HttpGet("{sku}")]
  public async Task<ActionResult<IEnumerable<List<Price>>>> GetPrice(string sku) {
    var prices = await _context.Price.Where(p => p.CatalogEntryCode == sku).ToListAsync();
    if (prices.Count == 0) {
      return NotFound();
    }

    return
      prices.GroupBy(p => new { p.MarketId, p.CurrencyCode }).ToList()
        .Select(priceGroup => GetOptimizedPrices(priceGroup.ToList())).ToList();
  }

  /// <summary>Get the optimized prices for a list of prices.</summary>
  /// <param name="prices">The list of prices to optimize</param>
  /// <returns>The optimized list of prices</returns>
  private List<Price> GetOptimizedPrices(List<Price> prices) {
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

      previous = optimizedPrices.Last();
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
      var temp = entry.Clone();
      temp.ValidFrom = optimizedPrices.Last().ValidUntil;
      optimizedPrices.Add(temp);
      sortedPrices.Insert(0, current);
      return temp;
    }
    optimizedPrices.Add(current.Clone());
    return current;
  }
}