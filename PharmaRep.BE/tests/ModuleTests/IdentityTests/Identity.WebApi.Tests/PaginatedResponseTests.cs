using Shared.WebApi;

namespace Identity.WebApi.Tests;

public class PaginatedResponseTests
{
    [Theory]
    [InlineData(0, 5, new[] {1, 2, 3, 4, 5})]
    [InlineData(-1, 5, new[] {1, 2, 3, 4, 5})]
    public void Create_PageNumberNonPositive_ThrowsException(int pageNumber, int pageSize, int[] items)
    {
        // Arrange
        PaginatedResponse<int> Create() => PaginatedResponse<int>.Create(pageNumber, pageSize, items);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(nameof(pageNumber), Create);
    }
    
    [Theory]
    [InlineData(1, 0, new[] {1, 2, 3, 4, 5})]
    [InlineData(1, -1, new[] {1, 2, 3, 4, 5})]
    public void Create_PageSizeNonPositive_ThrowsException(int pageNumber, int pageSize, int[] items)
    {
        // Arrange
        PaginatedResponse<int> Create() => PaginatedResponse<int>.Create(pageNumber, pageSize, items);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(nameof(pageSize), Create);
    }
    
    [Fact]
    public void Create_NullItems_ThrowsException()
    {
        // Arrange
        PaginatedResponse<int> Create() => PaginatedResponse<int>.Create<int>(pageNumber: 1, pageSize: 5, items: null);
        
        // Assert
        Assert.Throws<ArgumentNullException>(paramName: "items", Create);
    }

    [Fact]
    public void Create_FirstPage_HasNotPreviousPage()
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 3;
        var items = new[] {1, 2, 3, 4, 5};
        
        // Act
        var response = PaginatedResponse<int>.Create(pageNumber, pageSize, items);
        
        // Assert
        Assert.False(response.HasPrevious);
    }
    
    [Fact]
    public void Create_LastPage_HasNotNextPage()
    {
        // Arrange
        var items = new[] {1, 2, 3, 4, 5};
        const int pageSize = 3;
        var pageNumber = (int)Math.Ceiling((double)items.Length / pageSize);
        
        // Act
        var response = PaginatedResponse<int>.Create(pageNumber, pageSize, items);
        
        // Assert
        Assert.False(response.HasNext);
    }
    
    [Fact]
    public void Create_MiddlePage_HasPreviousAndNextPages()
    {
        // Arrange
        var items = new[] {1, 2, 3, 4, 5};
        const int pageSize = 2;
        var pageNumber = (int)Math.Floor((double)items.Length / pageSize);
        
        // Act
        var response = PaginatedResponse<int>.Create(pageNumber, pageSize, items);
        
        // Assert
        Assert.True(response.HasPrevious);
        Assert.True(response.HasNext);
    }
}