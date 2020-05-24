using Godot;
using System.Collections.Generic;
public class Quest
{
    public Objective objective;
    public Option option;
    int identifier;
    bool done;
    List<Quest> prerequisite;

    public Quest(Objective objective, Option option)
    {
        this.objective = objective;
        this.option = option;
        this.done = false;
    }

    public void AddPrerequisite(Quest q)
    {
        if(prerequisite == null)
            prerequisite = new List<Quest>();
        
        prerequisite.Add(q);
    }

    public bool Available()
    {
        if(!done)
            if(prerequisite != null)
                for(int i = 0; i < prerequisite.Count; i++)
                    if(prerequisite[i].Done == false)
                        return false;
        return true;
    }

    public void Complete(Region region)
    {
        if(Available())
        {
            OnComplete(region.Properties);
            done = true;
        }
    }

    public void OnComplete(Properties properties)
    {
        if(objective == Objective.Kill)
            properties.Enemies = 0;
        if(objective == Objective.Deliver)
            properties.DeliverableNPC = false;
        if(objective == Objective.Escort)
            properties.EscortableNPC = false;
        if(objective == Objective.Gather)
            properties.Resources = 0;
        if(objective == Objective.DefendArea)
        {
            properties.DefendableArea = false;
            properties.Enemies = 0;
        }
        if(objective == Objective.Interact)
            properties.InteractableOBJ = false;
    }

    public bool Equals(Quest q)
    {
        if(this.objective == q.objective && this.option == q.option)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        string str = "--- Quest " + identifier + " ---\n";
        if(option == Option.None)
            str += "-- Objective --\n" + objective.ToString() + ".";
        else
            str += "-- Objective --\n" + objective.ToString() + " with " + option.ToString() + ".";

        str += "\n";
        if(Available())
            str += "Quest available!";
        else
        {
            str += "First complete quests:\n{";
            for(int i = 0; i < prerequisite.Count; i++)
            {
                str += prerequisite[i].identifier;
                if(i + 1 < prerequisite.Count)
                    str+= ",";
            }
            str += "}";
        }
        return UIUtils.CenterText(str);
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
        Kill = 1,
        Deliver = 2,
        Escort = 3,
        Gather = 4,
        DefendArea = 5,
        Interact = 6
    }

    public enum Option
    {
        None = 1,
        Stealth = 2
        
    }

}