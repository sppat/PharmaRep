using System.Globalization;
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
                                    INSERT INTO [identity].[AspNetUsers] (Id,
                                                                          FirstName, 
                                                                          LastName, 
                                                                          UserName, 
                                                                          Email,
                                                                          EmailConfirmed,
                                                                          SecurityStamp, 
                                                                          PhoneNumberConfirmed,
                                                                          TwoFactorEnabled,
                                                                          LockoutEnabled,
                                                                          AccessFailedCount)
                                    VALUES ('{user.Id}',
                                        '{user.FirstName}',
                                        '{user.LastName}',
                                        '{user.Email}',
                                        '{user.Email}',
                                        1, 
                                        '{Guid.NewGuid()}',
                                        0,
                                        0,
                                        0,
                                        0);
                                    INSERT INTO [identity].[AspNetUserRoles] ("UserId", "RoleId")
                                    VALUES ('{user.Id}', '{Role.MedicalRepresentative.Id}');
                                    """;
				queryBuilder.Append(addUserQuery);
				queryBuilder.AppendLine();
			}

			return queryBuilder.ToString();
		}
	}

	public static class AppointmentsModuleSeeder
	{
		public static string SeedAppointment()
		{
			var queryBuilder = new StringBuilder();
			foreach (var appointment in MockData.Appointments)
			{
				var addAppointmentQuery = $"""
                                    INSERT INTO [appointments].[Appointments] (Id,
                                                                               StartDate, 
                                                                               EndDate,
                                                                               CreatedBy,
                                                                               CreatedAt,
                                                                               UpdatedBy,
                                                                               UpdatedAt)
                                    VALUES ('{appointment.Id}',
                                            '{appointment.Start.ToString("O", CultureInfo.InvariantCulture)}',
                                            '{appointment.End.ToString("O", CultureInfo.InvariantCulture)}',
                                            '{appointment.Organizer.Id}',
                                            '{DateTimeOffset.UtcNow.ToString("O", CultureInfo.InvariantCulture)}',
                                            null,
                                            null);
                                    """;
				queryBuilder.Append(addAppointmentQuery);
				queryBuilder.AppendLine();

				var addAddressQuery = $"""
                                        INSERT INTO [appointments].[Addresses] (AppointmentId,
                                                                                Street,
                                                                                Number,
                                                                                ZipCode)
                                        VALUES ('{appointment.Id}',
                                                '{appointment.Address.Street}',
                                                {appointment.Address.Number},
                                                {appointment.Address.ZipCode});
                                        """;

				queryBuilder.Append(addAddressQuery);
				queryBuilder.AppendLine();

				foreach (var attendee in appointment.Attendees)
				{
					var addAttendeeQuery = $"""
                                            INSERT INTO [appointments].[Attendees] (UserId,
                                                                                    AppointmentId)
                                            VALUES ('{attendee.Id}',
                                                    '{appointment.Id}');
                                            """;
					queryBuilder.Append(addAttendeeQuery);
					queryBuilder.AppendLine();
				}
			}

			return queryBuilder.ToString();
		}
	}
}
