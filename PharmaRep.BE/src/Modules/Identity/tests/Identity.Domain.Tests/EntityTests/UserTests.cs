using Identity.Domain.Entities;
using Identity.Domain.Exceptions.UserExceptions;

namespace Identity.Domain.Tests.EntityTests;

public class UserTests
{
	private readonly User _validUser = User.Create(email: "test@email.com", firstName: "John", lastName: "Doe");

	[Fact]
	public void Create_ValidUser_ReturnsUser()
	{
		// Act
		var user = User.Create(email: _validUser.Email, firstName: _validUser.FirstName, lastName: _validUser.LastName);

		// Assert
		Assert.NotNull(user);
		Assert.Equal(_validUser.FirstName, user.FirstName);
		Assert.Equal(_validUser.LastName, user.LastName);
		Assert.Equal(_validUser.Email, user.Email);
	}

	#region Email

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("  ")]
	[InlineData("  hello")]
	[InlineData("world  ")]
	[InlineData("hello world")]
	[InlineData("123email")]
	[InlineData("123@ email")]
	public void Create_InvalidEmail_ThrowsInvalidUserEmailException(string email)
	{
		// Act
		void CreateUser() => User.Create(email: email, firstName: _validUser.FirstName, lastName: _validUser.LastName);

		// Assert
		Assert.Throws<UserArgumentException>(CreateUser);
	}

	#endregion

	#region First Name

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("   ")]
	[InlineData("123")]
	[InlineData("123abc")]
	public void Create_InvalidFirstName_ThrowsInvalidUserFirstNameException(string firstName)
	{
		// Act
		void CreateUser() => User.Create(email: _validUser.Email, firstName: firstName, lastName: _validUser.LastName);

		// Assert
		Assert.Throws<UserArgumentException>(CreateUser);
	}

	#endregion

	#region Last Name

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("   ")]
	[InlineData("123")]
	[InlineData("123abc")]
	public void Create_InvalidLastName_ThrowsInvalidUserLastNameException(string lastName)
	{
		// Act
		void CreateUser() => User.Create(email: _validUser.Email, firstName: _validUser.FirstName, lastName: lastName);

		// Assert
		Assert.Throws<UserArgumentException>(CreateUser);
	}

	#endregion

	#region Update First Name

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	[InlineData("1abc")]
	public void UpdateFirstName_InvalidFirstName_ThrowsException(string firstName)
	{
		// Act
		void UpdateFirstName() => _validUser.UpdateFirstName(firstName);

		// Assert
		Assert.Throws<UserArgumentException>(UpdateFirstName);
	}

	[Fact]
	public void UpdateFirstName_ValidFirstName_UpdatesFirstName()
	{
		// Arrange
		const string newFirstName = "Jane";

		// Act
		_validUser.UpdateFirstName(newFirstName);

		// Assert
		Assert.Equal(newFirstName, _validUser.FirstName);
	}

	#endregion

	#region Update Last Name

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	[InlineData("1abc")]
	public void UpdateFirstName_InvalidLastName_ThrowsException(string lastName)
	{
		// Act
		void UpdateLastName() => _validUser.UpdateLastName(lastName);

		// Assert
		Assert.Throws<UserArgumentException>(UpdateLastName);
	}

	[Fact]
	public void UpdateFirstName_ValidLastName_UpdatesLastName()
	{
		// Arrange
		const string newLastName = "Jane";

		// Act
		_validUser.UpdateLastName(newLastName);

		// Assert
		Assert.Equal(newLastName, _validUser.LastName);
	}

	#endregion
}
