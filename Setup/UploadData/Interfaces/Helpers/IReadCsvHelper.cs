namespace UploadData.Interfaces.Helpers;

public interface IReadCsvHelper <T>
{
    public List<T> GetItemsFromCsv(string locationOfCsv);
}