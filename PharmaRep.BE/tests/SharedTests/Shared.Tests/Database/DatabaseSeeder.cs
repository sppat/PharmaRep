using System.Text;
using Identity.Domain.Entities;

namespace Shared.Tests.Database;

public static class DatabaseSeeder
{
    public static class IdentityModuleSeeder
    {
        public static string SeedTestUsersQuery()
        {
            var queryBuilder = new StringBuilder();
            foreach (var user in MockData.Users)
            {
                var addUserQuery = $"""
                                    INSERT INTO identity."AspNetUsers" ("Id", "FirstName", "LastName", "Email", "EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount")
                                    VALUES ('{user.Id}', '{user.FirstName}', '{user.LastName}', '{user.Email}', true, false, false, false, 0);
                                    INSERT INTO identity."AspNetUserRoles" ("UserId", "RoleId")
                                    VALUES ('{user.Id}', '{Role.MedicalRepresentative.Id}');
                                    """;
                queryBuilder.Append(addUserQuery);
                queryBuilder.AppendLine();
            }
            
            return queryBuilder.ToString();
        }   
    }
}