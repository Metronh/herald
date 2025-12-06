namespace SetUp.Interfaces.Helpers;

public interface IReadCsvHelper<T>
{
    public List<T> GetItemsFromCsv(string locationOfCsv);
}