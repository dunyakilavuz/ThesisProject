using Godot;
using System;

public class GraphVertex
{  
    int data;

    public GraphVertex(int data)
    {
        this.data = data;
    }

    public int Data
    {
        get
        {
            return data;
        }
    }

    public override string ToString()
    {
        return data.ToString(); 
    }
}