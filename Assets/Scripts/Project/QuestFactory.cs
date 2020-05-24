using Godot;
using System.Collections.Generic;
public class QuestFactory : Node
{
    Map map;
    List<Quest> givenQuests;
    List<Region> regionTabuList;
    int tabuRemoveAfter = 2;
    int maxIter = 5000;
    
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }

    public void GenerateQuests()
    {
        map = References.map;
        GraphVertex A = map.regionGraph.vertices[0];
        GraphVertex B = map.regionGraph.vertices[5];
        map.regionGraph.Path(A,B);
    }

 
    Quest GenerateQuest(GraphVertex vert)
    {
        Properties properties = vert.Region.PropertiesAfterQuest;
        if(vert == null)
        {
            GD.Print("Vertex is null, returning.");
            return null;
        }

        if(properties == null)
        {
            GD.Print("Properties are null, returning.");   
            return null;
        }
            
        Quest quest = QuestDecision(properties);

        if(quest == null)
        {
            GD.Print("No eligible quests found for Region: " + vert.Region.number);
            return null;
        }

        for(int i = 0; i < givenQuests.Count; i++)
            quest.AddPrerequisite(givenQuests[i]);

        return quest;
    }

    Quest QuestDecision(Properties properties)
    {
        List<Quest.Objective> availableTypes = new List<Quest.Objective>();
        Quest.Objective type;
        Quest.Option option = Quest.Option.None;

        if(properties.Cover > 5 && properties.Enemies > 0)
            option = Quest.Option.Stealth;

        if(properties.DefendableArea)
            availableTypes.Add(Quest.Objective.DefendArea);
        if(properties.Enemies > 0)
            availableTypes.Add(Quest.Objective.Kill);
        if(properties.DeliverableNPC)
            availableTypes.Add(Quest.Objective.Deliver);
        if(properties.Resources > 3)
            availableTypes.Add(Quest.Objective.Gather);
        if(properties.EscortableNPC)
            availableTypes.Add(Quest.Objective.Escort);
        if(properties.InteractableOBJ)
            availableTypes.Add(Quest.Objective.Interact);

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