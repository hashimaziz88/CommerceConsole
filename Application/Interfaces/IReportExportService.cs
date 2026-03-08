namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for report export orchestration workflows.
/// </summary>
public interface IReportExportService
{
    /// <summary>
    /// Generates and exports a PDF sales report and returns the file path.
    /// </summary>
    string ExportSalesReportPdf(string outputDirectory, int topCount, int lowStockThreshold);
}