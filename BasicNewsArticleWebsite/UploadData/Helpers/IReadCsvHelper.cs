using UploadData.Models;

namespace UploadData.Interfaces.Services;

public interface IReadCsvHelper <T>
{
    public List<T> GetItemsFromCsv(string locationOfCsv);
}