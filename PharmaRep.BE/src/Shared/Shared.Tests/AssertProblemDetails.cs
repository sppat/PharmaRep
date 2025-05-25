using Xunit;

namespace Shared.Tests;

public static class AssertProblemDetails
{
    public static void HasErrors(IEnumerable<string> expected, IEnumerable<string> actual) => Assert.Equivalent(expected, actual);
}