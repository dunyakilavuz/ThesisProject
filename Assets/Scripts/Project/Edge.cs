using Godot;
using System;

public class Edge
{
    GraphVertex a;
    GraphVertex b;

    public Edge(GraphVertex A, GraphVertex B)
    {
        this.a = A;
        this.b = B;
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
}