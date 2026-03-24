using GymManagement.WebMVC.Models;

namespace GymManagement.WebMVC.Services.Excel;

public class ClientDataPortServiceFactory : IDataPortServiceFactory<ClientRowDto>
{
    private const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public IExportService<ClientRowDto> GetExportService(string contentType) =>
        contentType == Xlsx
            ? new ClientExcelExportService()
            : throw new NotSupportedException(contentType);

    public IImportService<ClientRowDto> GetImportService(string contentType) =>
        contentType == Xlsx
            ? new ClientExcelImportService()
            : throw new NotSupportedException(contentType);
}
