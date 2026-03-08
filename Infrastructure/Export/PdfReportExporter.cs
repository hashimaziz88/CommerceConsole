using System.Globalization;
using System.Text;
using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Infrastructure.Export;

/// <summary>
/// Exports sales report snapshots to a simple one-page PDF document.
/// </summary>
public sealed class PdfReportExporter : IReportExporter
{
    private const int MaxLinesPerPage = 40;
    private const string FilePrefix = "sales-report";

    /// <inheritdoc />
    public string Export(SalesReportSnapshot snapshot, string outputDirectory)
    {
        if (snapshot is null)
        {
            throw new ValidationException("Report snapshot is required.");
        }

        if (string.IsNullOrWhiteSpace(outputDirectory))
        {
            throw new ValidationException("Output directory is required.");
        }

        string normalizedDirectory = outputDirectory.Trim();
        Directory.CreateDirectory(normalizedDirectory);

        string fileName = $"{FilePrefix}-{snapshot.GeneratedAtUtc:yyyyMMdd-HHmmss}.pdf";
        string filePath = Path.Combine(normalizedDirectory, fileName);

        List<string> lines = BuildReportLines(snapshot);
        byte[] pdfBytes = BuildPdfDocument(lines);

        File.WriteAllBytes(filePath, pdfBytes);
        return filePath;
    }

    private static List<string> BuildReportLines(SalesReportSnapshot snapshot)
    {
        List<string> lines =
        [
            "CommerceConsole Sales Report",
            $"Generated (UTC): {snapshot.GeneratedAtUtc:yyyy-MM-dd HH:mm:ss}",
            $"Total Revenue: {snapshot.TotalRevenue:C}",
            string.Empty,
            "Orders by Status:"
        ];

        foreach (OrderStatus status in Enum.GetValues<OrderStatus>())
        {
            int count = snapshot.OrdersByStatus.TryGetValue(status, out int value) ? value : 0;
            lines.Add($"- {status}: {count}");
        }

        lines.Add(string.Empty);
        lines.Add("Best-Selling Products:");

        if (snapshot.BestSellingProducts.Count == 0)
        {
            lines.Add("- No sales data available.");
        }
        else
        {
            for (int index = 0; index < snapshot.BestSellingProducts.Count; index++)
            {
                ProductSalesReportItem item = snapshot.BestSellingProducts[index];
                lines.Add($"{index + 1}. {item.ProductName} | Qty: {item.TotalQuantitySold} | Revenue: {item.TotalRevenue:C}");
            }
        }

        lines.Add(string.Empty);
        lines.Add("Low-Stock Products:");

        if (snapshot.LowStockProducts.Count == 0)
        {
            lines.Add("- No products at or below threshold.");
        }
        else
        {
            foreach (LowStockReportItem item in snapshot.LowStockProducts)
            {
                string statusTag = item.IsActive ? "ACTIVE" : "INACTIVE";
                lines.Add($"- {item.ProductName} ({item.Category}) | Stock: {item.StockQuantity} | Price: {item.UnitPrice:C} | {statusTag}");
            }
        }

        if (lines.Count > MaxLinesPerPage)
        {
            int hiddenLineCount = lines.Count - (MaxLinesPerPage - 1);
            lines = lines.Take(MaxLinesPerPage - 1).ToList();
            lines.Add($"... {hiddenLineCount} additional line(s) omitted in one-page PDF mode.");
        }

        return lines;
    }

    private static byte[] BuildPdfDocument(IReadOnlyList<string> lines)
    {
        string contentStream = BuildContentStream(lines);
        int contentLength = Encoding.ASCII.GetByteCount(contentStream);

        List<long> objectOffsets = new();

        using MemoryStream stream = new();
        using StreamWriter writer = new(stream, new UTF8Encoding(false), 1024, true);

        writer.WriteLine("%PDF-1.4");
        writer.Flush();

        WriteObject(writer, stream, objectOffsets, 1, "<< /Type /Catalog /Pages 2 0 R >>");
        WriteObject(writer, stream, objectOffsets, 2, "<< /Type /Pages /Kids [3 0 R] /Count 1 >>");
        WriteObject(writer, stream, objectOffsets, 3,
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>");
        WriteObject(writer, stream, objectOffsets, 4, "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");

        string contentObject = $"<< /Length {contentLength} >>\nstream\n{contentStream}\nendstream";
        WriteObject(writer, stream, objectOffsets, 5, contentObject);

        long xrefOffset = stream.Position;

        writer.WriteLine("xref");
        writer.WriteLine($"0 {objectOffsets.Count + 1}");
        writer.WriteLine("0000000000 65535 f ");

        foreach (long offset in objectOffsets)
        {
            writer.WriteLine($"{offset:0000000000} 00000 n ");
        }

        writer.WriteLine("trailer");
        writer.WriteLine($"<< /Size {objectOffsets.Count + 1} /Root 1 0 R >>");
        writer.WriteLine("startxref");
        writer.WriteLine(xrefOffset.ToString(CultureInfo.InvariantCulture));
        writer.Write("%%EOF");
        writer.Flush();

        return stream.ToArray();
    }

    private static string BuildContentStream(IReadOnlyList<string> lines)
    {
        StringBuilder builder = new();

        builder.AppendLine("BT");
        builder.AppendLine("/F1 11 Tf");
        builder.AppendLine("50 760 Td");

        for (int index = 0; index < lines.Count; index++)
        {
            if (index > 0)
            {
                builder.AppendLine("0 -16 Td");
            }

            builder.Append('(')
                .Append(EscapePdfText(lines[index]))
                .AppendLine(") Tj");
        }

        builder.AppendLine("ET");
        return builder.ToString();
    }

    private static string EscapePdfText(string value)
    {
        return value
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("(", "\\(", StringComparison.Ordinal)
            .Replace(")", "\\)", StringComparison.Ordinal);
    }

    private static void WriteObject(
        StreamWriter writer,
        MemoryStream stream,
        ICollection<long> objectOffsets,
        int objectNumber,
        string body)
    {
        objectOffsets.Add(stream.Position);

        writer.WriteLine($"{objectNumber} 0 obj");
        writer.WriteLine(body);
        writer.WriteLine("endobj");
        writer.Flush();
    }
}
