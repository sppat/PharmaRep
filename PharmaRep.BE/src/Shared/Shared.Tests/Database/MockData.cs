using Identity.Application.Dtos;
using Identity.Domain.Entities;

namespace Shared.Tests.Database;

public static class MockData
{
    public static IEnumerable<UserDto> Users => 
    [
        new(Id: Guid.Parse("23b42b97-972b-40e9-9f43-baa9b2d3d1ad"),
            FirstName: "User",
            LastName: "One",
            Email: "user@one.com",
            Roles: [Role.MedicalRepresentative.Name]),
        new(Id: Guid.Parse("525eed83-52ec-44f7-8f2b-0c42e9e8e112"),
            FirstName: "User",
            LastName: "Two",
            Email: "user@two.com",
            Roles: [Role.MedicalRepresentative.Name]),
        new(Id: Guid.Parse("18309ae7-c39e-4c9b-9cb3-deb592eb6980"),
            FirstName: "User",
            LastName: "Three",
            Email: "user@three.com",
            Roles: [Role.MedicalRepresentative.Name]),
        new(Id: Guid.Parse("a08c2300-6a09-46ea-83bf-c75b4ab6485d"),
            FirstName: "User",
            LastName: "Four",
            Email: "user@four.com",
            Roles: [Role.MedicalRepresentative.Name]),
        new(Id: Guid.Parse("3c1a09b3-1b46-4b3c-8c10-aec8296239b5"),
            FirstName: "User",
            LastName: "Five",
            Email: "user@five.com",
            Roles: [Role.MedicalRepresentative.Name])
    ];
}