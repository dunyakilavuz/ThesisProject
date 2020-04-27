using Godot;
using System;
using System.Collections.Generic;

public class GraphVertex
{  
    int data;
    Region region;
    List<GraphVertex> connections;

    public GraphVertex(int data)
    {
        this.data = data;
        connections = new List<GraphVertex>();
    }

    public void AddConnection(GraphVertex vert)
    {
        connections.Add(vert);
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
            if(connections == null)
                return 0;
            else
                return connections.Count;
        }
    }

    public Region Region
    {
        get
        {
            return region;
        }
    }

    public override string ToString()
    {
        return data.ToString(); 
    }
}