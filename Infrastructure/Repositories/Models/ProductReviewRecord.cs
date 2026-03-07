namespace CommerceConsole.Infrastructure.Repositories.Models;

/// <summary>
/// JSON persistence model for product reviews.
/// </summary>
internal sealed class ProductReviewRecord
{
    /// <summary>
    /// Gets or sets review identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets product identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets customer identifier.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets rating value.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets review comment.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}
