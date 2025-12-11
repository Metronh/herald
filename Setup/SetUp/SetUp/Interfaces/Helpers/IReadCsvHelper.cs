namespace SetUp.Interfaces.Helpers;

public interface IReadCsvHelper<T>
{
    public IEnumerable<T> GetItemsFromCsv(string locationOfCsv);
}