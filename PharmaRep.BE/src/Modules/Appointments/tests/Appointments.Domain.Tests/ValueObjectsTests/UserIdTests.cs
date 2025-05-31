using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class UserIdTests
{
    [Fact]
    public void TryCreate_ValidId_CreatesUserId()
    {
        // Arrange
        var validId = Guid.NewGuid();

        // Act
        var userIdCreated = UserId.TryCreate(validId, out var userId);

        // Assert
        Assert.True(userIdCreated);
        Assert.Equal(validId, userId.Value);
    }
    
    [Fact]
    public void TryCreate_EmptyId_ReturnsFalse()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act
        var userIdCreated = UserId.TryCreate(emptyId, out var userId);

        // Assert
        Assert.False(userIdCreated);
        Assert.Null(userId);
    }
}