using System.Globalization;
using CsvHelper;
using SetUp.Interfaces.Helpers;

namespace SetUp.Helpers;

public class ReadCsvHelper<T> : IReadCsvHelper<T>
{
    public IEnumerable<T> GetItemsFromCsv(string locationOfCsv)
    {
        using (var reader = new StreamReader(locationOfCsv))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            foreach (var item in csv.GetRecords<T>())
            {
                yield return item;
            }
    }
}