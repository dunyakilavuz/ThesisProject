using Godot;
using System;

public class Edge
{
    GraphVertex a;
    GraphVertex b;
    float distance;

    public Edge(GraphVertex A, GraphVertex B, float distance = 1.0f)
    {
        this.a = A;
        this.b = B;
        this.distance = distance;
    }

    public GraphVertex A
    {
        get
        {
            return a;
        }
    }

    public GraphVertex B
    {
        get
        {
            return b;
        }
    }
    
    public float Distance
    {
        get
        {
            return distance;
        }
    }
}