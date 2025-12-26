using System.Globalization;
using CsvHelper;
using SetUp.Interfaces.Helpers;

namespace SetUp.Helpers;

public class ReadCsvHelper<T> : IReadCsvHelper<T>
{
    public IEnumerable<T> GetItemsFromCsv(string locationOfCsv)
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