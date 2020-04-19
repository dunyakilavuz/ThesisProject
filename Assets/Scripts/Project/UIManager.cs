using Godot;
using System;

public class UIManager : Node
{
    static string UIPath = "/root/MainScene/";
    RichTextLabel infoText;

    public override void _Ready()
    {
        infoText = (RichTextLabel)GetNode(UIPath + "UI/InfoPanel/InfoText");
        ReadInput();
    }

    public override void _Process(float delta)
    {
        SetInfoText();
    }

    void ReadInput()
    {
        string seedText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/SeedLine")).Text;
        string chunkSizeText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ChSizeLine")).Text;
        string chunkAmountText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ChAmountLine")).Text;
        string slopeText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/SlopeLine")).Text;
        string forestText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ForestLine")).Text;
        string regionText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/RegionLine")).Text;

        int seed;
        int chunkSize;
        int chunkAmount;
        int slope;
        int forestChance;
        int regions;


        if(seedText == "")
            seed = Maths.RandomInt();
        else
            Int32.TryParse(seedText, out seed);

        Int32.TryParse(chunkSizeText, out chunkSize);
        Int32.TryParse(chunkAmountText, out chunkAmount);
        Int32.TryParse(slopeText, out slope);
        Int32.TryParse(forestText, out forestChance);
        Int32.TryParse(regionText, out regions);

        References.noise.Seed = seed;
        References.chunkSize = chunkSize;
        References.chunkAmount = chunkAmount;
        References.walkableSlope = slope;
        References.forestChance = forestChance;
        References.regions = regions;
    }


    public void ToggleTerrain(bool enabled)
    {
        ReadInput();

        if(enabled)
            References.terrainFactory.GenerateTerrain();
        else
            References.terrainFactory.ClearTerrain();
    }

    public void ToggleBorders(bool enabled)
    {
        ReadInput();

        if(enabled)
            References.drawChunkBorders = true;
        else
            References.drawChunkBorders = false;
    }

    public void ToggleQuests(bool enabled)
    {
        ReadInput();

        if(enabled)
            References.questFactory.GenerateQuests();
        else
            References.questFactory.ClearQuests();
    }

    void SetInfoText()
    {
        string infoString = "";
        string positionText = "Player Pos: " + References.player.Transform.origin.ToString("F2");
        string regionText = "Player Region: " + References.player.Region;
        infoString += positionText + "\n" + regionText;
        infoText.BbcodeText = infoString;
    }

    public static Color IntToColor(int value, float alpha = 1)
    {   
        float hue = (float)value / (float)References.regions + 5;
        return Color.FromHsv(hue,1,1,alpha);
    }
}