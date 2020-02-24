using Godot;
using System;

public class UIManager : Node
{
    static string UIPath = "/root/MainScene/";

    public override void _Ready()
    {
        ReadInput();
    }

    public override void _Process(float delta)
    {
        SetPositionText();
    }

    void ReadInput()
    {
        string seedText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/SeedLine")).Text;
        string chunkSizeText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ChSizeLine")).Text;
        string chunkAmountText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ChAmountLine")).Text;
        string steepnessText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/SteepnessLine")).Text;
        string slopeText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/SlopeLine")).Text;
        string forestText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ForestLine")).Text;
        string regionText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/RegionLine")).Text;

        int seed;
        int chunkSize;
        int chunkAmount;
        int steepness;
        int slope;
        int forestChance;
        int regions;


        if(seedText == "")
            seed = References.random.Next();
        else
            Int32.TryParse(seedText, out seed);

        Int32.TryParse(chunkSizeText, out chunkSize);
        Int32.TryParse(chunkAmountText, out chunkAmount);
        Int32.TryParse(steepnessText, out steepness);
        Int32.TryParse(slopeText, out slope);
        Int32.TryParse(forestText, out forestChance);
        Int32.TryParse(regionText, out regions);

        References.noise.Seed = seed;
        References.chunkSize = chunkSize;
        References.chunkAmount = chunkAmount;
        References.Steepness = steepness;
        References.walkableSlope = slope;
        References.forestChance = forestChance;
        References.regions = regions;
    }


    public void ToggleTerrain(bool enabled)
    {
        ReadInput();

        if(enabled)
            References.factory.GenerateTerrain();
        else
            References.factory.ClearTerrain();

        if(enabled)
            References.factory.GenerateForest();
        else
            References.factory.ClearForest();
    }

    public void TogglePoints(bool enabled)
    {
        ReadInput();

        if(enabled)
            References.factory.GeneratePoints();
        else
            References.factory.ClearPoints();
    }

    void SetPositionText()
    {
        string positionText = "Position: " + References.player.Transform.origin.ToString("F2");
        ((RichTextLabel)GetNode(UIPath + "UI/InfoPanel/PositionText")).BbcodeText = UIUtils.CenterText(positionText);;
    }

    public static Color GenerateColor(int number)
    {   
        float hue = (float)number / (float)References.regions + 5;
        return Color.FromHsv(hue,1,1,1);
    }
}