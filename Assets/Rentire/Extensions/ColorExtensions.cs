using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ColorExtensions
{
    public static Color Color(this int n)
    {
        return n.Color32();
    }

    public static Color32 Color32(this int n)
    {
        return new Color32((byte) ((n >> 16) & 0xff), (byte) ((n >> 8) & 0xff), (byte) ((n >> 0) & 0xff), 0xff);
    }

    public static Color R(this Color c, float r)
    {
        c.r = r;
        return c;
    }

    public static Color G(this Color c, float g)
    {
        c.g = g;
        return c;
    }

    public static Color B(this Color c, float b)
    {
        c.b = b;
        return c;
    }

    public static Color A(this Color c, float a)
    {
        c.a = a;
        return c;
    }

    public static Color R(this Color c, Func<float, float> f)
    {
        c.r = f(c.r);
        return c;
    }

    public static Color G(this Color c, Func<float, float> f)
    {
        c.g = f(c.g);
        return c;
    }

    public static Color B(this Color c, Func<float, float> f)
    {
        c.b = f(c.b);
        return c;
    }

    public static Color A(this Color c, Func<float, float> f)
    {
        c.a = f(c.a);
        return c;
    }

    public static RHSV HSV(this Color c)
    {
        return (RHSV) c;
    }

    public static Color Color(this RHSV hsv)
    {
        return (Color) hsv;
    }

    public static RHSV H(this RHSV hsv, float h)
    {
        hsv.H = h;
        return hsv;
    }

    public static RHSV S(this RHSV hsv, float s)
    {
        hsv.S = s;
        return hsv;
    }

    public static RHSV V(this RHSV hsv, float v)
    {
        hsv.V = v;
        return hsv;
    }

    public static RHSV A(this RHSV hsv, float a)
    {
        hsv.A = a;
        return hsv;
    }

    public static RHSV H(this RHSV hsv, Func<float, float> f)
    {
        hsv.H = f(hsv.H);
        return hsv;
    }

    public static RHSV S(this RHSV hsv, Func<float, float> f)
    {
        hsv.S = f(hsv.S);
        return hsv;
    }

    public static RHSV V(this RHSV hsv, Func<float, float> f)
    {
        hsv.V = f(hsv.V);
        return hsv;
    }

    public static RHSV A(this RHSV hsv, Func<float, float> f)
    {
        hsv.A = f(hsv.A);
        return hsv;
    }

    public static Color GetRandomColor()
    {
        var randomR = UnityEngine.Random.Range(0, 1f);
        var randomB = UnityEngine.Random.Range(0, 1f);
        var randomG = UnityEngine.Random.Range(0, 1f);

        var color = new Color(randomR, randomG, randomB, 1);
        return color;
    }

    public static Color GetRandomHSVColor()
    {
        var color = GetRandomColor();
         var hsv = color.HSV();
         hsv.V = UnityEngine.Random.Range(0.2f, 0.6f);
         hsv.S = UnityEngine.Random.Range(0.65f, 1f);
         color = hsv.Color();
        return color;
    }
}

public struct RHSV
{
    public float H;
    public float S;
    public float V;
    public float A;

    public static explicit operator RHSV(Color c)
    {
        var hsv = new RHSV();
        Color.RGBToHSV(c, out hsv.H, out hsv.S, out hsv.V);
        hsv.A = c.a;
        return hsv;
    }

    public static explicit operator Color(RHSV hsv)
    {
        var c = Color.HSVToRGB(hsv.H, hsv.S, hsv.V);
        c.a = hsv.A;
        return c;
    }
}