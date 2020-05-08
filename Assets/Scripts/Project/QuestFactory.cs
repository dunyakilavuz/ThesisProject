using Godot;
using System.Collections.Generic;
public class QuestFactory : Node
{
    Map map;
    List<Quest> givenQuests;
    int maxIter = 5000;
    
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }

    public void GenerateQuests()
    {
        if(References.map == null)
            return;
        else
            map = References.map;

        if(givenQuests == null)
            givenQuests = new List<Quest>();

        GraphVertex current = map.regionGraph.MostConnected();  
        Quest quest;
        int questID = 1;
        int iterations = 0;

        while(givenQuests.Count < References.questCount)
        {
            quest = GenerateQuest(current);
            if(quest != null)
            {
                quest.ID = questID;
                questID++;
                quest.OnComplete(current.Region.PropertiesAfterQuest);
                givenQuests.Add(quest);
                current.Region.AddQuest(quest);
                current = NextRegion(current);
            }

            if(iterations > maxIter)
                break;
        }
    }

    GraphVertex NextRegion(GraphVertex currentVert)
    {
        int connectionCount = currentVert.Connections.Count;
        int index = Maths.random.Next(connectionCount);
        GraphVertex newVert = currentVert.Connections[index];
        return newVert;
    }

    Quest GenerateQuest(GraphVertex vert)
    {
        Properties properties = vert.Region.PropertiesAfterQuest;
        if(vert == null)
            return null;
        if(properties == null)
            return null;
            
        Quest quest = QuestDecision(properties);

        for(int i = 0; i < givenQuests.Count; i++)
            quest.AddPrerequisite(givenQuests[i]);

        return quest;
    }

    Quest QuestDecision(Properties properties)
    {
        List<Quest.Type> availableTypes = new List<Quest.Type>();
        Quest.Type type;
        Quest.Option option = Quest.Option.None;

        if(properties.Cover > 5 && properties.Enemies > 0)
            option = Quest.Option.Stealth;

        if(properties.DefendableArea)
            availableTypes.Add(Quest.Type.DefendArea);
        if(properties.Enemies > 0)
            availableTypes.Add(Quest.Type.Kill);
        if(properties.DeliverableNPC)
            availableTypes.Add(Quest.Type.Deliver);
        if(properties.Resources > 3)
            availableTypes.Add(Quest.Type.Gather);
        if(properties.EscortableNPC)
            availableTypes.Add(Quest.Type.Escort);
        if(properties.InteractableOBJ)
            availableTypes.Add(Quest.Type.Interact);

        if(availableTypes.Count == 0)
            return null;
        
        int selection = Maths.random.Next(availableTypes.Count);
        type = availableTypes[selection];

        return new Quest(type,option);
    }



    public string CompletedQuestsSTR()
    {
        if(givenQuests != null)
        {
            string completed = "Completed Quests: {";
            for(int i = 0; i < givenQuests.Count; i++)
            {
                if(givenQuests[i].Done)
                    completed += givenQuests[i].ID;
                if(i + 1 < givenQuests.Count)
                    if(givenQuests[i + 1].Done)
                        completed += ",";
            }
            completed += "}";
            return UIUtils.CenterText(completed);
        }
        return "";
    }

    public bool AllCompleted()
    {
        if(givenQuests != null)
        {
            bool allCompleted = true;
            for(int i = 0; i < givenQuests.Count; i++)
                if(!givenQuests[i].Done)
                    allCompleted = false;
            return allCompleted;
        }
        return false;
    }

    public void ClearQuests()
    {
        if(References.map == null)
            return;

        if(References.map.gridChunk == null)
            return;
        
        if(givenQuests == null)
            return;

        for(int i = 0; i < References.chunkAmount; i++)
        {
            for(int j = 0; j < References.chunkAmount; j++)
            {
                References.map.gridChunk[i,j].region.ClearQuests();
            }
        }
        givenQuests.Clear();
    }
}