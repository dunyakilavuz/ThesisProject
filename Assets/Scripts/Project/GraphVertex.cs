using Godot;
using System;

public class GraphVertex
{  
    int data;
    Quest quest;

    public GraphVertex(int data)
    {
        this.data = data;
    }

    public GraphVertex(Quest quest)
    {
        this.quest = quest;
    }

    public int Data
    {
        get
        {
            return data;
        }
    }

    public Quest Quest
    {
        get
        {
            return quest;
        }
    }

    public override string ToString()
    {
        return data.ToString(); 
    }
}