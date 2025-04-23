using Shared.WebApi.Responses;

namespace Identity.WebApi.Tests;

public class PaginatedResponseTests
{
    [Theory]
    [InlineData(0, 5, 5, new[] {1, 2, 3, 4, 5})]
    [InlineData(-1, 5, 5, new[] {1, 2, 3, 4, 5})]
    public void Create_PageNumberNonPositive_ThrowsException(int pageNumber, int pageSize, int total, int[] items)
    {
        // Arrange
        PaginatedResponse<int> Create() => new(pageNumber, pageSize, total, items);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(nameof(pageNumber), Create);
    }
    
    [Theory]
    [InlineData(1, 0, 5, new[] {1, 2, 3, 4, 5})]
    [InlineData(1, -1, 5, new[] {1, 2, 3, 4, 5})]
    public void Create_PageSizeNonPositive_ThrowsException(int pageNumber, int pageSize, int total, int[] items)
    {
        // Arrange
        PaginatedResponse<int> Create() => new(pageNumber, pageSize, total, items);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(nameof(pageSize), Create);
    }

    [Theory] 
    [InlineData(1, 5, -1, new[] { 1, 2, 3, 4, 5 })]
    public void Create_TotalNonPositive_ThrowsException(int pageNumber, int pageSize, int total, int[] items)
    {
        // Arrange
        PaginatedResponse<int> Create() => new(pageNumber, pageSize, total, items);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(nameof(total), Create);
    }
    
    [Fact]
    public void Create_NullItems_ThrowsException()
    {
        // Arrange
        PaginatedResponse<int> Create() => new(pageNumber: 1, pageSize: 5, total: 5, items: null);
        
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
        var total = items.Length;
        
        // Act
        var response = new PaginatedResponse<int>(pageNumber, pageSize, total, items);
        
        // Assert
        Assert.False(response.HasPrevious);
    }
    
    [Fact]
    public void Create_LastPage_HasNotNextPage()
    {
        // Arrange
        var items = new[] {1, 2, 3, 4, 5};
        var total = items.Length;
        const int pageSize = 3;
        var pageNumber = (int)Math.Ceiling((double)items.Length / pageSize);
        
        // Act
        var response = new PaginatedResponse<int>(pageNumber, pageSize, total, items);
        
        // Assert
        Assert.False(response.HasNext);
    }
    
    [Fact]
    public void Create_MiddlePage_HasPreviousAndNextPages()
    {
        // Arrange
        var items = new[] {1, 2, 3, 4, 5};
        var total = items.Length;
        const int pageSize = 2;
        var pageNumber = (int)Math.Floor((double)items.Length / pageSize);
        
        // Act
        var response = new PaginatedResponse<int>(pageNumber, pageSize, total, items);
        
        // Assert
        Assert.True(response.HasPrevious);
        Assert.True(response.HasNext);
    }
}