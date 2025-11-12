namespace ExpenseManagement.Application.Common.Interfaces;

public interface IExcelService
{
    Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName, CancellationToken cancellationToken = default);
}
