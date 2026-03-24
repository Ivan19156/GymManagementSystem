namespace GymManagement.WebMVC.Services;

public interface IImportService<T>
{
    IEnumerable<T> Import(Stream stream);
}
