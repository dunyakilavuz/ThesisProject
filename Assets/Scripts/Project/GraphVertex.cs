using Godot;
using System;
using System.Collections.Generic;

public class GraphVertex
{  
    int data;
    Region region;
    List<GraphVertex> connections;

    public GraphVertex(Region region)
    {
        this.region = region;
        connections = new List<GraphVertex>();
    }

    public void AddConnection(GraphVertex vert)
    {
        connections.Add(vert);
    }

    public bool Equals(GraphVertex vert)
    {
        if(region == vert.region)
            return true;
        else
            return false;
    }

    public List<GraphVertex> Connections
    {
        get
        {
            if(connections == null)
                return null;
            else
                return connections;
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
        return region.number.ToString(); 
    }
}