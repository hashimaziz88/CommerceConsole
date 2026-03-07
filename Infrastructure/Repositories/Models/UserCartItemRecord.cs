namespace CommerceConsole.Infrastructure.Repositories.Models;

/// <summary>
/// JSON persistence model for cart items embedded in user records.
/// </summary>
internal sealed class UserCartItemRecord
{
    /// <summary>
    /// Gets or sets product identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets product name snapshot.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets unit price snapshot.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets quantity snapshot.
    /// </summary>
    public int Quantity { get; set; }
}
