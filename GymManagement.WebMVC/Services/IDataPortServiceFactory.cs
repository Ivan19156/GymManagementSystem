namespace GymManagement.WebMVC.Services;

public interface IDataPortServiceFactory<T>
{
    IExportService<T> GetExportService(string contentType);
    IImportService<T> GetImportService(string contentType);
}
