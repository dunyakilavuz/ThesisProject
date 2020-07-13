using Godot;
using System.Collections.Generic;
public class QuestFactory : Node
{
    Map map;
    Graph graph;
    List<Quest> quests;
    int questIndex = 0;
    
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }

    public void GenerateQuests()
    {
//        var watch = System.Diagnostics.Stopwatch.StartNew(); //Comment when benchmarking
        map = References.map;

        if(map == null)
            return;

        graph = map.regionGraph;

        if(graph == null)
            return;
        
        if(quests == null)
            quests = new List<Quest>();
        
        questIndex++;
        Quest q = GenerateQuest(graph.RandomPath(false), questIndex);
        quests.Add(q);
        References.player.AcceptQuest(q);
//        watch.Stop(); //Comment when benchmarking
//        GD.Print(q.ToString("Elapsed Time: " + watch.ElapsedMilliseconds * 0.001 + " seconds.")); //Comment when benchmarking
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
            if(quests.Count == 0)
                return false;

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
        
        questIndex = 0;
        References.player.ClearQuests();
//        GD.Print("Clear Quests.");  //Comment when benchmarking
        quests.Clear();
    }
}