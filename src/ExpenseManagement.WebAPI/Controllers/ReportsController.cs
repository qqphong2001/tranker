using ExpenseManagement.Application.Reports.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ExportExpensesToPdfQueryHandler _exportToPdfHandler;
    private readonly ExportExpensesToExcelQueryHandler _exportToExcelHandler;

    public ReportsController(
        ExportExpensesToPdfQueryHandler exportToPdfHandler,
        ExportExpensesToExcelQueryHandler exportToExcelHandler)
    {
        _exportToPdfHandler = exportToPdfHandler;
        _exportToExcelHandler = exportToExcelHandler;
    }

    [HttpGet("expenses/pdf")]
    public async Task<IActionResult> ExportExpensesToPdf(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var query = new ExportExpensesToPdfQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            CategoryId = categoryId
        };

        var pdfBytes = await _exportToPdfHandler.Handle(query, cancellationToken);

        var fileName = $"Expenses_{DateTime.UtcNow:yyyyMMdd}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }

    [HttpGet("expenses/excel")]
    public async Task<IActionResult> ExportExpensesToExcel(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var query = new ExportExpensesToExcelQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            CategoryId = categoryId
        };

        var excelBytes = await _exportToExcelHandler.Handle(query, cancellationToken);

        var fileName = $"Expenses_{DateTime.UtcNow:yyyyMMdd}.xlsx";
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
