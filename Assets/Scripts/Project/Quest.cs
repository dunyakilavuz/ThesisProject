using Godot;
using System.Collections.Generic;
public class Quest
{
    public Type type;
    public Option option;
    int identifier;
    bool done;
    List<Quest> prerequisite;

    public Quest(Type type, Option option)
    {
        this.type = type;
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
        if(type == Type.Kill)
            properties.Enemies = 0;
        if(type == Type.Deliver)
            properties.DeliverableNPC = false;
        if(type == Type.Escort)
            properties.EscortableNPC = false;
        if(type == Type.Gather)
            properties.Resources = 0;
        if(type == Type.DefendArea)
        {
            properties.DefendableArea = false;
            properties.Enemies = 0;
        }
        if(type == Type.Interact)
            properties.InteractableOBJ = false;
    }

    public bool Equals(Quest q)
    {
        if(this.type == q.type && this.option == q.option)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        string str = "--- Quest " + identifier + " ---\n";
        if(option == Option.None)
            str += "-- Objective --\n" + type.ToString() + ".";
        else
            str += "-- Objective --\n" + type.ToString() + " with " + option.ToString() + ".";

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


    public enum Type
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