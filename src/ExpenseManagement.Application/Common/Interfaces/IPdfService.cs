namespace ExpenseManagement.Application.Common.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateReportPdfAsync<T>(T data, string templateName, CancellationToken cancellationToken = default);
}
