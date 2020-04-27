using Godot;
using System.Collections.Generic;
public class Quest
{
    QuestType type;
    QuestOption option;
    int identifier;
    bool done;
    List<Quest> prerequisite;

    public Quest(QuestType type, QuestOption option, int identifier)
    {
        this.type = type;
        this.option = option;
        this.identifier = identifier;
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
        if(prerequisite != null)
        {
            for(int i = 0; i < prerequisite.Count; i++)
            {
                if(prerequisite[i].Done == false)
                    return false;
            }
        }
        return true;
    }

    public void Finish()
    {
        if(Available())
            done = true;
        else
            GD.Print("Quest has prerequisite.");
    }

    public override string ToString()
    {
        string str = "--- Quest " + identifier + " ---\n";
        if(option == QuestOption.NoOption)
            str += "Objective: " + type.ToString() + ".";
        else
            str += "Objective: " + type.ToString() + " with " + option.ToString() + ".";

        str += "\n";
        if(Available())
            str += "Quest available.";
        else
        {
            str += "Prerequisite: Quests ";
            for(int i = 0; i < prerequisite.Count; i++)
            {
                str += prerequisite[i].identifier;
                if(i + 1 < prerequisite.Count)
                    str+= ",";
            }
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


    public enum QuestType
    {
        Kill = 1,
        Deliver = 2,
        Escort = 3,
        Gather = 4,
        DefendArea = 5,
        Interact = 6
    }

    public enum QuestOption
    {
        NoOption = 1,
        Stealth = 2
        
    }

}