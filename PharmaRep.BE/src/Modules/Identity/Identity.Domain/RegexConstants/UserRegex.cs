using System.Text.RegularExpressions;

namespace Identity.Domain.RegexConstants;

public static partial class UserRegex
{
	[GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
	public static partial Regex EmailFormat();

	[GeneratedRegex(@"^[\p{L}]+$")]
	public static partial Regex NameFormat();
}
