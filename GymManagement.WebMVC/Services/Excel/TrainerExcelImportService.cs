using ClosedXML.Excel;
using GymManagement.WebMVC.Models;

namespace GymManagement.WebMVC.Services.Excel;

public class TrainerExcelImportService : IImportService<TrainerRowDto>
{
    public IEnumerable<TrainerRowDto> Import(Stream stream)
    {
        using var wb = new XLWorkbook(stream);
        var ws = wb.Worksheet(1);
        var rows = new List<TrainerRowDto>();

        foreach (var row in ws.RowsUsed().Skip(1)) // skip header
        {
            var email = row.Cell(3).GetString().Trim();
            if (string.IsNullOrWhiteSpace(email)) continue;

            int?     exp  = int.TryParse(row.Cell(6).GetString(), out var e) ? e : null;
            decimal? rate = decimal.TryParse(row.Cell(7).GetString(), out var d) ? d : null;

            rows.Add(new TrainerRowDto
            {
                FirstName       = row.Cell(1).GetString().Trim(),
                LastName        = row.Cell(2).GetString().Trim(),
                Email           = email,
                Phone           = row.Cell(4).GetString().Trim().NullIfEmptyT(),
                Specialization  = row.Cell(5).GetString().Trim().NullIfEmptyT(),
                ExperienceYears = exp,
                HourlyRate      = rate,
            });
        }

        return rows;
    }
}

file static class StringExtT
{
    public static string? NullIfEmptyT(this string s) =>
        string.IsNullOrWhiteSpace(s) ? null : s;
}
