using Godot;
using System.Collections.Generic;
public class Quest
{
    public GraphVertex[] path;
    public Objective[] objectives;
    public Option[] options;
    int identifier;
    bool done;
    int index = 0;

    public Quest(GraphVertex[] path, Objective[] objectives, Option[] options, int identifier)
    {
        this.path = path;
        this.objectives = objectives;
        this.options = options;
        this.identifier = identifier;
        this.done = false;
        this.index = 0;
    }

    public void CompleteObjective(int regionNumber)
    {
        if(ObjectiveAvailable(regionNumber))
        {
            OnComplete(path[index].Region.P1, index);
            objectives[index] = Objective.Complete;
            index++;
        }

        if(index == objectives.Length)
        {
            CompleteQuest();
            index = 0;
        }

    }

    public bool ObjectiveAvailable(int regionNumber)
    {   
        int progress = -1;

        for(int i = 0; i < path.Length; i++)
        {
            if(path[i].Region.number == regionNumber)
                progress = i;
        }

        if(progress > -1)
        {
            if(path[index].Region.number == regionNumber)
            {
                if(index == progress)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CompleteQuest()
    {
        bool allDone = false;

        for(int i = 0; i < objectives.Length; i++)
        {
            if(objectives[i] == Objective.Complete || objectives[i] == Objective.None)
            {
                allDone = true;
            }
        }

        if(allDone)
        {
            this.done = true;
        }

        return allDone;
    }

    void OnComplete(Properties properties, int index)
    {
        //Update region properties as objective is finished.
        if(objectives[index] == Objective.Kill)
            properties.Enemies = 0;
        if(objectives[index] == Objective.Deliver)
            properties.DeliverableNPC = false;
        if(objectives[index] == Objective.Escort)
            properties.EscortableNPC = false;
        if(objectives[index] == Objective.Gather)
            properties.Resources = 0;
        if(objectives[index] == Objective.DefendArea)
        {
            properties.DefendableArea = false;
            properties.Enemies = 0;
        }
        if(objectives[index] == Objective.Interact)
            properties.InteractableOBJ = false;
    }

    public bool Equals(Quest q)
    {
        if(identifier == q.identifier)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        string str = "--- Quest " + identifier + " ---\n";
        str += "Path: {";
        for(int i = 0; i < path.Length; i++)
        {
            str += path[i].Region.number;
            if(i + 1 < path.Length)
                str += ",";
            else
                str += "}\n";
        }


        str += "-- Objectives -- \n";
        for(int i = 0; i < path.Length; i++)
        {
            if(options[i] != Option.None)
                str += "*" + objectives[i].ToString() + " with "+ options[i] + " at Region " + path[i].Region.number;
            else
                str += "*" + objectives[i].ToString() + " at Region " + path[i].Region.number;
            if(i + 1 < path.Length)
                str += "\n";
            else
                str += "";
        }

        return str;
    }

    public bool Done
    {
        get
        {
            return done;
        }
        set
        {
            done = value;
        }
    }

    public int ID
    {
        get
        {
            return identifier;
        }
        set
        {
            identifier = value;
        }
    }


    public enum Objective
    {
        None = 0,
        Kill = 1,
        Deliver = 2,
        Escort = 3,
        Gather = 4,
        DefendArea = 5,
        Interact = 6,
        Complete = 7
    }

    public enum Option
    {
        None = 1,
        Stealth = 2
        
    }

}