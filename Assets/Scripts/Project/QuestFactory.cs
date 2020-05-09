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
        if(References.map == null)
            return;
        else
            map = References.map;

        if(givenQuests == null)
            givenQuests = new List<Quest>();
        if(regionTabuList == null)
            regionTabuList = new List<Region>();

        GraphVertex current = map.regionGraph.MostConnected();
        GD.Print("\n-- Generating Quests --");
        GD.Print("\nStarting with Region: " + current.Region.number);

        Quest quest;
        int questID = 1;
        int iterations = 0;

        while(true)
        {
            quest = GenerateQuest(current);
            if(quest != null)
            {
                quest.ID = questID;
                questID++;
                quest.OnComplete(current.Region.PropertiesAfterQuest);
                givenQuests.Add(quest);
                current.Region.AddQuest(quest);
                GD.Print("- Quest " + quest.ID + " assigned to Region: " + current.Region.number +
                "  --  Tabu List Add Region: " + current.Region.number + " -");
                regionTabuList.Add(current.Region);
            }
            else
            {
                GD.Print("Failed assigning quest.");
            }

            iterations++;

            if(iterations > maxIter)
            {
                GD.Print("Max Iterations reached, breaking.");
                break;
            }

            if(givenQuests.Count == References.questCount)
            {
                GD.Print("All quests are generated.");
                break;
            }

            current = NextRegion(current);
        }
        GD.Print("\n-- Completed generating quests --");
    }

    GraphVertex NextRegion(GraphVertex currentVert)
    {
        int connectionCount = currentVert.Connections.Count;
        string neighbors = "Neighbors of Region: " + currentVert.Region.number + "\t[";
        for(int i = 0; i < connectionCount; i++)
        {
            neighbors += currentVert.Connections[i];
            if(i + 1 < connectionCount)
                neighbors += ",";
            else
                neighbors += "]";
        }
        GD.Print(neighbors);
        int index = Maths.random.Next(connectionCount);
        GraphVertex newVert = currentVert.Connections[index];
        GD.Print("Picked a new Region: " + newVert.Region.number);
        
        bool equal = true;
        int tabuCount = 0;

        while(equal)
        {
            equal = false;
            for(int i = 0; i < regionTabuList.Count; i++)
            {
                if(newVert.Region == regionTabuList[i])
                {
                    GD.Print("Region " + newVert.Region.number + " is in tabu list.");
                    equal = true;
                    break;
                }
            }

            if(equal)
            {
                connectionCount = currentVert.Connections.Count;
                neighbors = "Neighbors of Region: " + currentVert.Region.number + "\t [";
                for(int i = 0; i < connectionCount; i++)
                {
                    neighbors += currentVert.Connections[i];
                    if(i + 1 < connectionCount)
                        neighbors += ",";
                    else
                        neighbors += "]";
                }
                GD.Print(neighbors);
                index = Maths.random.Next(connectionCount);
                newVert = currentVert.Connections[index];
                GD.Print("Picked a new Region: " + newVert.Region.number);
                tabuCount++;
            }

            if(tabuCount > tabuRemoveAfter)
            {
                tabuCount = 0;
                GD.Print("- Tabu List Remove Region: " + regionTabuList[0].number + " -");
                regionTabuList.RemoveAt(0);
            }
        }

        return newVert;
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