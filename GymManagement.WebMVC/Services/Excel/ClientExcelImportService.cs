using ClosedXML.Excel;
using GymManagement.WebMVC.Models;

namespace GymManagement.WebMVC.Services.Excel;

public class ClientExcelImportService : IImportService<ClientRowDto>
{
    public IEnumerable<ClientRowDto> Import(Stream stream)
    {
        using var wb = new XLWorkbook(stream);
        var ws = wb.Worksheet(1);
        var rows = new List<ClientRowDto>();

        foreach (var row in ws.RowsUsed().Skip(1)) // skip header
        {
            var email = row.Cell(3).GetString().Trim();
            if (string.IsNullOrWhiteSpace(email)) continue;

            rows.Add(new ClientRowDto
            {
                FirstName    = row.Cell(1).GetString().Trim(),
                LastName     = row.Cell(2).GetString().Trim(),
                Email        = email,
                Phone        = row.Cell(4).GetString().Trim().NullIfEmpty(),
                MedicalNotes = row.Cell(5).GetString().Trim().NullIfEmpty(),
            });
        }

        return rows;
    }
}

file static class StringExtensions
{
    public static string? NullIfEmpty(this string s) =>
        string.IsNullOrWhiteSpace(s) ? null : s;
}
