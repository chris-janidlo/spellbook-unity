using UnityEngine;

public static class StringExtensions
{
    public static string WrapInTMProColorTag(this string input, Color color)
    {
        string rgba = ColorUtility.ToHtmlStringRGBA(color);
        return $"<#{rgba}>{input}</color>";
    }
}
