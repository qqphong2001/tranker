using ClosedXML.Excel;
using ExpenseManagement.Application.Common.Interfaces;

namespace ExpenseManagement.Infrastructure.Services;

public class ExcelService : IExcelService
{
    public async Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() =>
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Get properties
            var properties = typeof(T).GetProperties();

            // Add headers
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
            }

            // Add data
            int row = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cell(row, i + 1).Value = value?.ToString() ?? string.Empty;
                }
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Save to memory stream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }, cancellationToken);
    }
}
