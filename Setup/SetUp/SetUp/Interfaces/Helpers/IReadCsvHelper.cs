namespace SetUp.Interfaces.Helpers;

public interface IReadCsvHelper
{
    public IEnumerable<T> GetItemsFromCsv<T>(string locationOfCsv);
}