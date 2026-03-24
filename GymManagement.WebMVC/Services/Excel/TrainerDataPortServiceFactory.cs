using GymManagement.WebMVC.Models;

namespace GymManagement.WebMVC.Services.Excel;

public class TrainerDataPortServiceFactory : IDataPortServiceFactory<TrainerRowDto>
{
    private const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public IExportService<TrainerRowDto> GetExportService(string contentType) =>
        contentType == Xlsx
            ? new TrainerExcelExportService()
            : throw new NotSupportedException(contentType);

    public IImportService<TrainerRowDto> GetImportService(string contentType) =>
        contentType == Xlsx
            ? new TrainerExcelImportService()
            : throw new NotSupportedException(contentType);
}
