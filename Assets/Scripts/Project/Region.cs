using Godot;
using System;
using System.Collections.Generic;

public class Region : Node
{
    public int number;
    List<Chunk> chunks;
    Properties p0;
    Properties p1;

    public Region(int number)
    {
        this.number = number;
        if(number >= 0)
        {
            p0 = new Properties();   
            p1 = new Properties(p0);
        }
    }

    public void AddChunk(Chunk chunk)
    {
        if(chunks == null)
            chunks = new List<Chunk>();

        chunks.Add(chunk);
    }

    public bool Equals(Region region)
    {
        if(this.number == region.number)
            return true;
        else
            return false;
    }

    public Properties P0
    {
        get{return p0;}
    }

    public Properties P1
    {
        get{return p1;}
    }
}