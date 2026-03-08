using CommerceConsole.Application.Models;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for report export strategies.
/// </summary>
public interface IReportExporter
{
    /// <summary>
    /// Exports a sales report snapshot and returns the created file path.
    /// </summary>
    string Export(SalesReportSnapshot snapshot, string outputDirectory);
}