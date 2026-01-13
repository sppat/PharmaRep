namespace Shared.Infrastructure.Constants;

public static class EfConstants
{
	public const string DefaultConnection = "DefaultConnection";
	public const string MigrationsHistoryTable = "__EFMigrationsHistory";

	public static class Schemas
	{
		public const string Identity = "identity";
		public const string Appointments = "appointments";
	}

	public static class OwnedTables
	{
		public static class Appointment
		{
			public const string Address = "Addresses";
		}
	}

	public static class ShadowProperties
	{
		public static class AppointmentAddress
		{
			public const string AppointmentId = "AppointmentId";
		}
	}
}
