using Godot;
using System.Collections.Generic;
public class QuestFactory : Node
{
    Map map;
    Graph graph;
    List<Quest> quests;
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
        graph = map.regionGraph;
        
        if(quests == null)
            quests = new List<Quest>();
        
        int index = 0;
        while(index < References.questCount)
        {
            Quest q = GenerateQuest(graph.RandomPath(), index + 1);
            quests.Add(q);
            References.player.AcceptQuest(q);
            index++;
        }
    }

 
    Quest GenerateQuest(GraphVertex[] path, int index)
    {
        if(path == null)
        {
            GD.Print("Path is null, returning.");
            return null;
        }

        Properties properties;
        Quest.Objective[] objectives = new Quest.Objective[path.Length];
        Quest.Option[] options = new Quest.Option[path.Length];

        for(int i = 0; i < path.Length; i++)
        {
            properties = path[i].Region.P0;
            objectives[i] = GenerateObjective(properties);
            options[i] = GenerateOption(properties);
        }

        Quest quest = new Quest(path,objectives,options,index);

        quests.Add(quest);
        return quest;
    }

    Quest.Objective GenerateObjective(Properties properties)
    {
        List<Quest.Objective> availableObjectives = new List<Quest.Objective>();
        Quest.Objective obj = Quest.Objective.None;

        if(properties == null)
            return obj;

        if(properties.DefendableArea)
            availableObjectives.Add(Quest.Objective.DefendArea);
        if(properties.Enemies > 0)
            availableObjectives.Add(Quest.Objective.Kill);
        if(properties.DeliverableNPC)
            availableObjectives.Add(Quest.Objective.Deliver);
        if(properties.Resources > 3)
            availableObjectives.Add(Quest.Objective.Gather);
        if(properties.EscortableNPC)
            availableObjectives.Add(Quest.Objective.Escort);
        if(properties.InteractableOBJ)
            availableObjectives.Add(Quest.Objective.Interact);

        if(availableObjectives.Count == 0)
            return obj;
                
        int selection = Maths.random.Next(availableObjectives.Count);
        obj = availableObjectives[selection];

        return obj;
    }

    Quest.Option GenerateOption(Properties properties)
    {
        Quest.Option option = Quest.Option.None;

        if(properties == null)
            return option;

        if(properties.Cover > 5 && properties.Enemies > 0)
            option = Quest.Option.Stealth;

        return option;
    }

    public string CompletedQuestsSTR()
    {
        if(quests != null)
        {
            string completed = "Completed Quests: {";
            for(int i = 0; i < quests.Count; i++)
            {
                if(quests[i].Done)
                    completed += quests[i].ID;
                if(i + 1 < quests.Count)
                    if(quests[i + 1].Done)
                        completed += ",";

            }
            completed += "}";
            return UIUtils.CenterText(completed);
        }
        return "";
    }

    public bool AllCompleted()
    {
        if(quests != null)
        {
            bool allCompleted = true;
            for(int i = 0; i < quests.Count; i++)
                if(!quests[i].Done)
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
        
        if(quests == null)
            return;

        References.player.ClearQuests();
        quests.Clear();
    }
}