using Godot;
using System.Collections.Generic;
public class Quest
{
    QuestType type;
    int identifier;
    bool done;
    Quest preReq;

    public Quest(string name, QuestType type, int identifier)
    {
        this.type = type;
        this.identifier = identifier;
        this.done = false;
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