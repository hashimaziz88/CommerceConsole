using System.Text;
using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Export;
using Xunit;

namespace CommerceConsole.Tests.Infrastructure;

/// <summary>
/// Tests PDF sales report export behavior.
/// </summary>
public sealed class PdfReportExporterTests
{
    /// <summary>
    /// Verifies exporting a snapshot creates a PDF file with an expected header.
    /// </summary>
    [Fact]
    public void Export_WritesPdfDocument()
    {
        string outputDirectory = CreateTempDataDirectory();

        try
        {
            PdfReportExporter exporter = new();
            SalesReportSnapshot snapshot = CreateSnapshot();

            string filePath = exporter.Export(snapshot, outputDirectory);

            Assert.True(File.Exists(filePath));
            Assert.Equal(".pdf", Path.GetExtension(filePath));

            byte[] bytes = File.ReadAllBytes(filePath);
            string header = Encoding.ASCII.GetString(bytes, 0, 8);
            string fullText = Encoding.ASCII.GetString(bytes);

            Assert.Equal("%PDF-1.4", header);
            Assert.Contains("CommerceConsole Sales Report", fullText);
            Assert.Contains("Total Revenue", fullText);
        }
        finally
        {
            DeleteDirectoryIfExists(outputDirectory);
        }
    }

    /// <summary>
    /// Verifies a missing output directory argument is rejected.
    /// </summary>
    [Fact]
    public void Export_WithBlankDirectory_ThrowsValidationException()
    {
        PdfReportExporter exporter = new();
        SalesReportSnapshot snapshot = CreateSnapshot();

        Assert.Throws<ValidationException>(() => exporter.Export(snapshot, "  "));
    }

    private static SalesReportSnapshot CreateSnapshot()
    {
        return new SalesReportSnapshot(
            new DateTime(2026, 3, 8, 8, 0, 0, DateTimeKind.Utc),
            1400m,
            new Dictionary<OrderStatus, int>
            {
                [OrderStatus.Paid] = 2,
                [OrderStatus.Delivered] = 4,
                [OrderStatus.Cancelled] = 1
            },
            new List<ProductSalesReportItem>
            {
                new("Laptop", 5, 1200m),
                new("Mouse", 4, 200m)
            },
            new List<LowStockReportItem>
            {
                new("Cable", "Accessories", 2, 20m, true)
            });
    }

    private static string CreateTempDataDirectory()
    {
        string path = Path.Combine(Path.GetTempPath(), "CommerceConsoleTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteDirectoryIfExists(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
}
