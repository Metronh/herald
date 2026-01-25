using System.Globalization;
using CsvHelper;
using SetUp.Interfaces.Helpers;

namespace SetUp.Helpers;

public class ReadCsvHelper : IReadCsvHelper
{
    public IEnumerable<T> GetItemsFromCsv<T>(string locationOfCsv)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), locationOfCsv);
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            foreach (var item in csv.GetRecords<T>())
            {
                yield return item;
            }
    }
}