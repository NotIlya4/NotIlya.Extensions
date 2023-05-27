namespace NotIlya.Extensions;

public static class StringExtensions
{
    public static string JoinStrings(this IEnumerable<string> strings, string separator = ", ")
    {
        return string.Join(separator, strings);
    }

    public static bool EqualsIgnoreCase(this string thisString, string? other)
    {
        if (other is null)
        {
            return false;
        }

        return thisString.Equals(other, StringComparison.OrdinalIgnoreCase);
    }
}