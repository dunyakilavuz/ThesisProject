using Godot;
using System;

public class Maths
{
    public static float PI = 3.14159265359f;
    public static float INF = Mathf.Inf;
    public static Random random;

    public static float Lerp(float from, float to, float factor)
    {                
        return (1.0f - factor) * from + factor * to;
    }

    public static Color LerpColor(Color from, Color to, float factor)
    {
        return new Color(
            Lerp(from.r,to.r,factor),
            Lerp(from.g,to.g,factor),
            Lerp(from.b,to.b,factor),
            Lerp(from.a,to.a,factor)
        );
    }

    public static float Angle(Vector3 A ,Vector3 B)
    {
        return Mathf.Acos(A.Normalized().Dot(B.Normalized())) * Rad2Deg;
    }

    public static float RandomFloat(float min, float max)
    {
        if(random == null)
            random = new Random();

        return (float)random.NextDouble() * (max - min) + min;
    }

    public static int RandomInt(int min, int max)
    {
        if(random == null)
            random = new Random();

        return random.Next(min, max + 1);
    }

    public static int RandomInt()
    {
        if(random == null)
            random = new Random();

        return random.Next();
    }

    public static float RepeatAngle(float angle)
    {
        return  angle - Mathf.Floor(angle / 360.0f) * 360.0f;
    }

    public static float Deg2Rad 
    { 
        get
        {
            return PI / 180f;
        }
    }
    public static float Rad2Deg 
    {
        get
        {
            return 180f / PI;;
        }
    }
}
