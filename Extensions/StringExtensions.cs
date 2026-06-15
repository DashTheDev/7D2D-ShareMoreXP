namespace ShareMoreXP;

public static class StringExtensions
{
    /// <summary>
    /// Returns the estimated byte length of a string when written to a net package.
    /// Includes 1 byte for the length prefix. Assumes ASCII encoding.
    /// </summary>
    public static int ToPackageLength(this string value)
    {
        return string.IsNullOrEmpty(value) ? 1 : value.Length + 1;
    }
}