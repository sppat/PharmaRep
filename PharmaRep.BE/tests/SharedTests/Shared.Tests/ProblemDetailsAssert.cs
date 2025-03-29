using Xunit;

namespace Shared.Tests;

public static class ProblemDetailsAssert
{
    public static void HasErrors(IEnumerable<string> expected, IEnumerable<string> actual) => Assert.Equivalent(expected, actual);
}