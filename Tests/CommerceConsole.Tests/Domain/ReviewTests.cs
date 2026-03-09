using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Domain;

/// <summary>
/// Tests review invariants and persistence-friendly behavior.
/// </summary>
public sealed class ReviewTests
{
    /// <summary>
    /// Verifies review constructor enforces rating range.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Constructor_WithOutOfRangeRating_ThrowsValidationException(int rating)
    {
        Assert.Throws<ValidationException>(() =>
            _ = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), rating, "Comment"));
    }

    /// <summary>
    /// Verifies review comments are trimmed.
    /// </summary>
    [Fact]
    public void Constructor_WithComment_StoresTrimmedComment()
    {
        Review review = new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 5, "  Great product  ");

        Assert.Equal("Great product", review.Comment);
    }

    /// <summary>
    /// Verifies null comments are normalized to empty string.
    /// </summary>
    [Fact]
    public void Constructor_WithNullComment_StoresEmptyString()
    {
        Review review = new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 4, null!);

        Assert.Equal(string.Empty, review.Comment);
    }

    /// <summary>
    /// Verifies created timestamp is assigned at construction.
    /// </summary>
    [Fact]
    public void Constructor_AssignsCreatedAtTimestamp()
    {
        Review review = new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 4, "Solid");

        Assert.True(review.CreatedAt > DateTime.MinValue);
    }
}
