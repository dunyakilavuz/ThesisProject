using Godot;
using System;
using System.Collections.Generic;

public class GraphVertex
{  
    int data;
    Region region;
    public float distance;
    public GraphVertex prev;

    public GraphVertex(Region region)
    {
        this.region = region;
    }

    public bool Equals(GraphVertex vert)
    {
        if(region.Equals(vert.region))
            return true;
        else
            return false;
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