using Godot;
using System.Collections.Generic;
public class Quest
{
    string name;
    QuestType type;
    int identifier;

    public Quest(string name, QuestType type, int identifier)
    {
        this.name = name;
        this.type = type;
        this.identifier = identifier;
    }


    public enum QuestType
    {
        Kill = 1,
        Escort = 2,
        DefendArea = 3,
        Gather = 4,
        Deliver = 5,
        Interact = 6
    }

}