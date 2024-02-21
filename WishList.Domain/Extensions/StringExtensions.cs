namespace System;

public static class StringExtensions
{
    public static string MarkForbiddenChar(this string text)
    {
        return text
            .Replace("_", "\\_")
            .Replace("*", "\\*")
            .Replace("[", "\\[")
            .Replace("]", "\\]")
            .Replace("(", "\\(")
            .Replace(")", "\\)")
            .Replace("~", "\\~")
            .Replace("`", "\\`")
            .Replace(">", "\\>")
            .Replace("#", "\\#")
            .Replace("+", "\\+")
            .Replace("-", "\\-")
            .Replace("=", "\\=")
            .Replace("|", "\\|")
            .Replace("{", "\\{")
            .Replace("}", "\\}")
            .Replace(".", "\\.")
            .Replace("!", "\\!");

    }
}