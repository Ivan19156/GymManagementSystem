using ClosedXML.Excel;
using GymManagement.WebMVC.Models;

namespace GymManagement.WebMVC.Services.Excel;

public class TrainerExcelExportService : IExportService<TrainerRowDto>
{
    private static readonly (string Header, Func<TrainerRowDto, object?> Value)[] Columns =
    [
        ("Ім'я",                r => r.FirstName),
        ("Прізвище",            r => r.LastName),
        ("Email",               r => r.Email),
        ("Телефон",             r => r.Phone),
        ("Спеціалізація",       r => r.Specialization),
        ("Досвід (років)",      r => r.ExperienceYears),
        ("Ставка (грн/год)",    r => r.HourlyRate),
    ];

    public byte[] Export(IEnumerable<TrainerRowDto> items)
    {
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Тренери");

        for (int c = 0; c < Columns.Length; c++)
        {
            var cell = ws.Cell(1, c + 1);
            cell.Value = Columns[c].Header;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
        }

        int row = 2;
        foreach (var item in items)
        {
            for (int c = 0; c < Columns.Length; c++)
                ws.Cell(row, c + 1).Value = Columns[c].Value(item)?.ToString() ?? "";
            row++;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
