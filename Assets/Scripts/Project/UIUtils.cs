using Godot;

public class UIUtils : Node
{
    public static Color ColorRed = new Color(1,0,0);
    public static Color ColorGreen = new Color(0,0,1);

    public static string ColorizeText(string text, Color color)
    {
        return "[color=#" + ColorToHex(color) + "]" + text + "[/color]";
    }

    public static string ColorToHex(Color color)
    {
        return  color.r8.ToString("X2") + 
                color.b8.ToString("X2") + 
                color.g8.ToString("X2");
    }
    
    public static string CenterText(string text)
    {  
        return "[center]" + text + "[/center]";
    }

    public static void ColorizeButton(Button button, Color color)
    {
        button.SelfModulate = color;
    }
}