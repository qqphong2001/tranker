using ExpenseManagement.Application.Common.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpenseManagement.Infrastructure.Services;

public class PdfService : IPdfService
{
    public async Task<byte[]> GenerateReportPdfAsync<T>(T data, string templateName, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() =>
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Expense Management Report - {templateName}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);
                            x.Item().Text($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm}");
                            x.Item().Text(data?.ToString() ?? "No data available");
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }, cancellationToken);
    }
}
