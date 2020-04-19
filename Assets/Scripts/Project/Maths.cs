using Godot;
using System;

public class Maths
{
    public static float PI = 3.14159265359f;
    static Random random;
    public static float EaseInOut(float from, float to, float factor)
    {
        int a = 2;
        float result = Mathf.Pow(factor,a) / (Mathf.Pow(factor,a) + Mathf.Pow(1 - factor,a));
        float slope = (to - from) / (1 - 0);
        return result * slope;
    }

    public static float QubicInterpolation(float from, float to, float factor)
    {
        float a = 3;
        float result = Mathf.Pow(factor, a);
        float slope = (to - from) / (1 - 0);
        return result * slope;
    }

    public static float Truncate(float value, int precision)
    {
        float truncated = (float)System.Math.Truncate(value);
        GD.Print(truncated);
        return truncated;
    }

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

    public static float LerpAngle(float from, float to, float factor)
    {
        float difference = System.Math.Abs(to - from);
        if (difference > 180)
        {
            if (to > from)
                from += 360;
            else
                to += 360;
        }
        float value = Lerp(from,to,factor);
        return RepeatAngle(value);
    }

    public static float DeltaAngle(float from, float to)
    {
        from = RepeatAngle(from);
        to = RepeatAngle(to);
        float difference = System.Math.Abs(from - to);

        if (difference < 180)
        {
            return difference;
        }
        else
        {
            return 360f + from - to;
        }
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

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = RepeatAngle(angle);
        min = RepeatAngle(min);
        max = RepeatAngle(max);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if(min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }
        if(angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if(!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if(angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        if(max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }
    
        result = !inverse ? tangle < tmax : tangle > tmax;
        if(!result)
            angle = max;
        return angle;
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
