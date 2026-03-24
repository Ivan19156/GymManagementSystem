namespace GymManagement.WebMVC.Services;

public interface IExportService<T>
{
    byte[] Export(IEnumerable<T> items);
}
