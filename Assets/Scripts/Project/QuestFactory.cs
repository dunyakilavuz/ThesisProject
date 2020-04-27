using Godot;
using System.Collections.Generic;
public class QuestFactory : Node
{
    Map map;
    List<Quest> givenQuests;
    int questCount;
    public override void _Ready()
    {
        questCount = 5;
    }

    public override void _Process(float delta)
    {

    }

    public void GenerateQuests()
    {
        GD.Print("Generate");

        if(References.map == null)
            return;
        else
            map = References.map;

        if(givenQuests == null)
            givenQuests = new List<Quest>();

        
        GraphVertex mostConnected = map.regionGraph.MostConnected();
        
        for(int i = 0; i < questCount; i++)
        {
            
        }
    }

    Quest ReadRegion(int questID)
    {
        Quest.QuestType type = (Quest.QuestType)Maths.RandomInt(1,6);
        Quest.QuestOption option = (Quest.QuestOption)Maths.RandomInt(1,2);
        Quest quest = new Quest(type,option,questID);
        for(int i = 0; i < givenQuests.Count; i++)
        {
            quest.AddPrerequisite(givenQuests[i]);
        }

        return quest;
    }

    public void ClearQuests()
    {
        GD.Print("Clear");
    }

}