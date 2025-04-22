using FluentValidation.TestHelper;
using Identity.Application.Features.User.GetAll;
using Shared.Application.Errors;

namespace Identity.Application.Tests.Validators;

public class GetAllUsersQueryValidatorTests
{
    private readonly GetAllUsersQuery _validQuery = new(PageNumber: 1, PageSize: 5);
    private readonly GetAllUsersQueryValidator _sut = new();
    
    [Fact]
    public void Validate_ValidQuery_ReturnsValidResult()
    {
        // Arrange

        // Act
        var result = _sut.TestValidate(_validQuery);

        // Assert
        Assert.True(result.IsValid);
    }

    #region PageNumber

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_PageNumberNonPositive_ReturnsInvalidResult(int pageNumber)
    {
        // Arrange
        var query = _validQuery with { PageNumber = pageNumber };

        // Act
        var result = _sut.TestValidate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(ApplicationErrors.PaginationErrors.PageNumberOutOfRange, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
    
    #region PageSize

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_PageSizeNonPositive_ReturnsInvalidResult(int pageSize)
    {
        // Arrange
        var query = _validQuery with { PageSize = pageSize };

        // Act
        var result = _sut.TestValidate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(ApplicationErrors.PaginationErrors.PageSizeOutOfRange, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
}