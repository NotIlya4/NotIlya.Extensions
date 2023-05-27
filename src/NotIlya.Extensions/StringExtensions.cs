namespace NotIlya.Extensions;

public static class StringExtensions
{
    public static string JoinStrings(this IEnumerable<string> strings, string separator = ", ")
    {
        return string.Join(separator, strings);
    }
}