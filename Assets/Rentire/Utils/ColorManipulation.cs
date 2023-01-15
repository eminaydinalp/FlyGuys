using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RColorExtensions
{
	private static Color ChangeColorBrightness(Color color, float correctionFactor)
	{
		float red = color.r;
		float green = color.g;
		float blue = color.b;

		if (correctionFactor < 0)
		{
			correctionFactor = 1 + correctionFactor;
			red *= correctionFactor;
			green *= correctionFactor;
			blue *= correctionFactor;
		}
		else
		{
			red = (255 - red) * correctionFactor + red;
			green = (255 - green) * correctionFactor + green;
			blue = (255 - blue) * correctionFactor + blue;
		}

		return new Color(color.a, (int)red, (int)green, (int)blue);
	}

    public static Color DarkenSoftenColor(this Color color, float percentage, bool isDarker)
    {
        if(isDarker)
            return Color.Lerp(color, Color.black, percentage);
        else
            return Color.Lerp(color, Color.white, percentage);
    }

    public static Color LightenBy(this Color color, int percent)
    {
        return ChangeColorBrightness(color, percent / 100f);
    }

    public static Color DarkenBy(this Color color, int percent)
    {
        return ChangeColorBrightness(color, -1 * percent / 100f);
    }

}
