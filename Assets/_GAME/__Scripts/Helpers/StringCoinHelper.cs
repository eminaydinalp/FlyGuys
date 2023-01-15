using UnityEngine;

public static class StringCoinHelper
{
    public static string ReturnCoin(this double coin, int format = 2)
    {
        string result;
        string[] ScoreNames = new string[]
        {
            "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an",
            "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf",
            "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx",
            "by", "bz",
        };
        int i;

        for (i = 0; i < ScoreNames.Length; i++)
            if (coin < 1000)
                break;
            else coin = coin / 1000f;

        double c = System.Math.Floor((double) coin * 100) / 100;
        if (c % System.Math.Floor(c) == 0)
            format = 0;
        result = c.ToString("N" + format) + ScoreNames[i];
        return result;
    }

    public static string ReturnCoin(this float coin, int format = 2)
    {
        string result;
        string[] ScoreNames = new string[]
        {
            "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an",
            "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf",
            "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx",
            "by", "bz",
        };
        int i;

        for (i = 0; i < ScoreNames.Length; i++)
            if (coin < 1000)
                break;
            else coin = coin / 1000f;

        double c = System.Math.Floor((double) coin * 100) / 100;
        if (c % System.Math.Floor(c) == 0)
            format = 0;
        result = c.ToString("N" + format) + ScoreNames[i];
        return result;
    }
}