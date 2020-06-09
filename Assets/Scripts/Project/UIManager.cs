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
        HandleInfo();
    }

    void ReadInput()
    {
        string seedText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/SeedLine")).Text;
        string chunkSizeText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ChSizeLine")).Text;
        string chunkAmountText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/ChAmountLine")).Text;
        string slopeText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/SlopeLine")).Text;
        string regionText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/RegionLine")).Text;
        string questsText = ((LineEdit)GetNode(UIPath + "UI/SettingsPanel/QuestsLine")).Text;

        int seed;
        int chunkSize;
        int chunkAmount;
        int slope;
        int regions;
        int questCount;


        if(seedText == "")
            seed = Maths.RandomInt();
        else
            Int32.TryParse(seedText, out seed);

        Int32.TryParse(chunkSizeText, out chunkSize);
        Int32.TryParse(chunkAmountText, out chunkAmount);
        Int32.TryParse(slopeText, out slope);
        Int32.TryParse(regionText, out regions);
        Int32.TryParse(questsText, out questCount);

        References.noise.Seed = seed;
        References.chunkSize = chunkSize;
        References.chunkAmount = chunkAmount;
        References.walkableSlope = slope;
        References.regionSize = regions;
        References.questCount = questCount;
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

    void HandleInfo()
    {
        ((LineEdit)GetNode(UIPath + "/UI/InfoPanel/PlayerPosLine")).Text = References.player.Transform.origin.ToString("F2");
        ((Button)GetNode(UIPath + "/UI/InfoPanel/QuestPanel/CompleteObjectiveButton")).Disabled = true;
        ((RichTextLabel)GetNode(UIPath + "/UI/InfoPanel/QuestPanel/CompletedQuests")).BbcodeText = References.questFactory.CompletedQuestsSTR();

        if(References.player.Region != null)
        {
            if(References.player.Region.P1 != null)
            {
                ((ProgressBar)GetNode(UIPath + "UI/InfoPanel/PropertiesPanel/Progress1")).Value = References.player.Region.P1.Enemies;
                ((ProgressBar)GetNode(UIPath + "UI/InfoPanel/PropertiesPanel/Progress2")).Value = References.player.Region.P1.Cover;
                ((ProgressBar)GetNode(UIPath + "UI/InfoPanel/PropertiesPanel/Progress3")).Value = References.player.Region.P1.Resources;
                ((ProgressBar)GetNode(UIPath + "UI/InfoPanel/PropertiesPanel/Progress4")).Value = Convert.ToInt16(References.player.Region.P1.EscortableNPC) * 10;
                ((ProgressBar)GetNode(UIPath + "UI/InfoPanel/PropertiesPanel/Progress5")).Value = Convert.ToInt16(References.player.Region.P1.DeliverableNPC) * 10;
                ((ProgressBar)GetNode(UIPath + "UI/InfoPanel/PropertiesPanel/Progress6")).Value = Convert.ToInt16(References.player.Region.P1.DefendableArea) * 10;
                ((ProgressBar)GetNode(UIPath + "UI/InfoPanel/PropertiesPanel/Progress7")).Value = Convert.ToInt16(References.player.Region.P1.InteractableOBJ) * 10;
            }
            
            ((LineEdit)GetNode(UIPath + "/UI/InfoPanel/PlayerRegionLine")).Text = References.player.Region.number.ToString();

            if(References.player.ActiveQuest != null)
            {
                ((RichTextLabel)GetNode(UIPath + "/UI/InfoPanel/QuestPanel/QuestInfo")).BbcodeText = UIUtils.CenterText(References.player.ActiveQuest.ToString());
                if(References.player.ActiveQuest.ObjectiveAvailable(References.player.Region.number))
                {
                    ((Button)GetNode(UIPath + "/UI/InfoPanel/QuestPanel/CompleteObjectiveButton")).Disabled = false;
                }
            }
        }
        else
        {
            ((LineEdit)GetNode(UIPath + "/UI/InfoPanel/PlayerRegionLine")).Text = "-- No Region --";
        }        

        if(References.questFactory.AllCompleted())
            ((RichTextLabel)GetNode(UIPath + "/UI/InfoPanel/QuestPanel/QuestInfo")).BbcodeText = UIUtils.CenterText("-- All quests are completed! --");

    }

    public static Color IntToColor(int value, float alpha = 1)
    {   
        float hue = (float)value / (float)References.regionSize + 5;
        return Color.FromHsv(hue,1,1,alpha);
    }
}