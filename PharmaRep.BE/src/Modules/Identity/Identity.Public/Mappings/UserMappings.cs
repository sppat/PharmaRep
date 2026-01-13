using Identity.Domain.Entities;
using Identity.Public.Contracts;

namespace Identity.Public.Mappings;

public static class UserMappings
{
	public static UserBasicInfo ToUserBasicInfo(this User user) => new(Id: user.Id,
		FirstName: user.FirstName,
		LastName: user.LastName,
		Email: user.Email);
}
