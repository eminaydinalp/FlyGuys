using System;

public static class StringExtensions
{
    public static float ToFloat(this string input, float defaultValue = 0f)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }
        else return defaultValue;
    }

    public static bool ToBool(this string input, bool defaultValue = false)
    {
        if (input.Trim().Equals("0"))
            return false;
        else if (input.Trim().Equals("1"))
            return true;
        return defaultValue;
    }

    public static string RemoveResourcesIfExists(this string input)
    {
        if (input.Contains("Resources/"))
            return input.Substring(input.IndexOf("Resources/", StringComparison.Ordinal) + 10);
        return input;
    }
    public static string GetAfterLatestSlash(this string input)
    {
        if (input.Contains("/"))
            return input.Substring(input.LastIndexOf("/", StringComparison.Ordinal) + 1); 
        return input;
    }

    public static string GetDressCode(this string input)
    {
        input = GetAfterLatestSlash(input);
        if(input.Contains("_"))
            return input.Substring(0, input.IndexOf("_", StringComparison.Ordinal));
        return input;
    }

    public static string ReplaceWith(this string input, string replace, string with = "")
    {
        return input.Replace(replace, with);
    }

    public static string ReplaceWith(this string input, params string[] replace)
    {
        for (int i = 0; i < replace.Length; i++)
        {
            input = input.Replace(replace[i], "");
        }

        return input;
    }

    public static string MakeProperAnswer(this string input, bool removeSpace = false)
    {
        var result = input.ReplaceWith("-", "+", "/", "!", ".", "'", ":","*");
        return removeSpace ? result.ReplaceWith(" ") : result;
    }

    public static string GetAnswerVariant(this string input)
    {
        if (input.Contains("?"))
        {
            var variants = input.Split("?");
            input = variants[UnityEngine.Random.Range(0, variants.Length)];
        }

        return input;
    }

    public static string GetProperPath(this string input)
    {
        input = input.RemoveResourcesIfExists().ReplaceWith(".prefab");
        return input;
    }
}