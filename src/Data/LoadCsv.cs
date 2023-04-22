using System.Globalization;
using CsvHelper;
using src.Models;

namespace src.Data;

public class LoadCsv {
  private readonly ApplicationDbContext _context;

  public LoadCsv(ApplicationDbContext context) {
    _context = context;
  }

  /**
   * Load the CSV file into the database if it doesn't exist.
   */
  public void Load() {
    const string csvPath = @"price_detail.csv";

    using var reader = new StreamReader(csvPath);
    using var csv = new CsvReader(
      reader,
      new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "\t" }
    );
    var records = csv.GetRecords<Price>();
    if (!_context.Database.EnsureCreated()) return;
    _context.Price.AddRange(records);
    _context.SaveChanges();
  }
}