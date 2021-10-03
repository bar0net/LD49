using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public const string PLAYER_TAG = "Player";
    public const string BANNER_KEY = "bannertype_";
    public const string BACKGROUND_KEY = "background_color";
    public const string FOREGROUND_KEY = "foreground_color";
    public const string BACKGROUND_DEFAULT = "33-33-33";
    public const string FOREGROUND_DEFAULT = "ff-ff-ff";


    public static int ManhattanDistance(int a, int b)
    {
        return (a > 0 ? a : -a) + (b > 0 ? b : -b);
    }

    public static string BannerKey(Banner.Type type)
    {
        return Utils.BANNER_KEY + ((int)type).ToString();
    }

    public static Color String2Color(string value, float alpha=1.0f)
    {
        string[] slices = value.Split('-');
        int r = int.Parse(slices[0], System.Globalization.NumberStyles.HexNumber);
        int g = int.Parse(slices[1], System.Globalization.NumberStyles.HexNumber);
        int b = int.Parse(slices[2], System.Globalization.NumberStyles.HexNumber);

        return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f, alpha); ;
    }

    public static string Color2String(Color color)
    {
        int r = (int)(color.r * 255.0f);
        int g = (int)(color.g * 255.0f);
        int b = (int)(color.b * 255.0f);

        return r.ToString("X") + "-" + g.ToString("X") + "-" + b.ToString("X");
    }

    public static string DefaultBannerColor(Banner.Type type)
    {
        switch (type)
        {
            case Banner.Type.None:
                return "33-33-33";

            case Banner.Type.Happy:
                return "FF-DD-88";

            case Banner.Type.Sad:
                return "88-99-FF";

            case Banner.Type.Wrath:
                return "FF-6F-6F";

            case Banner.Type.Melancoly:
                return "AA-66-BB";

            default:
                return "33-33-33";
        }
    }
}
