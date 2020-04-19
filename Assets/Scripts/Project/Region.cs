using Godot;
using System;
using System.Collections.Generic;

public class Region : Node
{
    public int number;
    List<Chunk> chunks;
    float lightness = 1;
    float visibility = 1;
    float cover = 0;

    int treeCount = 0;
    int boulderCount = 0;
    int grassCount = 0;

    public Region(int number)
    {
        this.number = number;
    }

    public void AddChunk(Chunk chunk)
    {
        if(chunks == null)
            chunks = new List<Chunk>();

        chunks.Add(chunk);

        treeCount += chunk.treeCount;
        boulderCount += chunk.boulderCount;
        grassCount += chunk.grassCount;
    }

    public void CalcRegionProperties()
    {
        lightness = 1 - ((float)treeCount / (float)chunks.Count);
        visibility = 1 - ((float)grassCount / (float)chunks.Count);  
        cover = (float)boulderCount / (float)chunks.Count;
    }

    public void printRegion()
    {
        GD.Print("---");
        GD.Print("Region number: " + number);
        GD.Print("Chunks: " + chunks.Count);
        GD.Print("Lightness: " + lightness  + " , Visibility: " + visibility + " , Cover: " + cover);
        GD.Print("---");
    }
}