using Godot;
using System;

public class GraphVertex
{  
    int data;
    Quest quest;
    int connections;

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

    public int Connections
    {
        get
        {
            return connections;
        }

        set
        {
            connections = value;
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