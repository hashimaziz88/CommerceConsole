using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Orchestrates report export workflows using report data and exporter strategy.
/// </summary>
public sealed class ReportExportService : IReportExportService
{
    private readonly IReportService _reportService;
    private readonly IReportExporter _reportExporter;

    /// <summary>
    /// Initializes report export service dependencies.
    /// </summary>
    public ReportExportService(IReportService reportService, IReportExporter reportExporter)
    {
        _reportService = reportService;
        _reportExporter = reportExporter;
    }

    /// <inheritdoc />
    public string ExportSalesReportPdf(string outputDirectory, int topCount, int lowStockThreshold)
    {
        if (string.IsNullOrWhiteSpace(outputDirectory))
        {
            throw new ValidationException("Output directory is required.");
        }

        SalesReportSnapshot snapshot = new(
            DateTime.UtcNow,
            _reportService.GetTotalRevenue(),
            _reportService.GetOrdersByStatus(),
            _reportService.GetBestSellingProducts(topCount),
            _reportService.GetLowStockProducts(lowStockThreshold));

        return _reportExporter.Export(snapshot, outputDirectory.Trim());
    }
}