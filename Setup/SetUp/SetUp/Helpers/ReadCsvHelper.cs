using System.Globalization;
using CsvHelper;
using SetUp.Interfaces.Helpers;

namespace SetUp.Helpers;

public class ReadCsvHelper<T> : IReadCsvHelper<T>
{
    public List<T> GetItemsFromCsv(string locationOfCsv)
    {
        List<T> items = new List<T>();
        using (var reader = new StreamReader(locationOfCsv))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            items = csv.GetRecords<T>().ToList();
        }

        return items;
    }
}