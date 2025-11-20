using System.Globalization;
using CsvHelper;
using UploadData.Interfaces.Services;

namespace UploadData.Helpers;

public class ReadCsvHelper<T> : IReadCsvHelper<T>
{
    public List<T> GetItemsFromCsv(string locationOfCsv)
    {
        List<T> items = new List<T>();
        try
        {
            using (var reader = new StreamReader(locationOfCsv))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                items = csv.GetRecords<T>().ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return items;
    }
}