using Godot;
using System;
using System.Collections.Generic;

public class Graph
{
    public List<GraphVertex> vertices;
    public List<Edge> edges;


    public bool AddVertex(GraphVertex vertex)
    {
        if(vertices == null)
            vertices = new List<GraphVertex>();

        if(vertices.Count == 0)
        {
            vertices.Add(vertex);
            return true;
        }

        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertex.Region.number == vertices[i].Region.number)
            {
                return false;
            }
        }

        vertices.Add(vertex);
        return true;
    }

    public GraphVertex GetVertex(int regionNumber)
    {
        if(vertices == null || vertices.Count == 0) 
            return null;

        for(int i = 0; i < vertices.Count; i++)
        {
            if(regionNumber == vertices[i].Region.number)
                return vertices[i];
        }
        return null;
    }

    public bool AddEdge(Edge edge)
    {
        if(edges == null)
            edges = new List<Edge>();

        if(edges.Count == 0)
        {
            if(edge.A.Region.number != edge.B.Region.number)
            {
                edge.A.AddConnection(edge.B);
                edge.B.AddConnection(edge.A);
                edges.Add(edge);
                return true;
            }
            else
            {
                return false;
            }
        }

        for(int i = 0; i < edges.Count; i++)
        {
            if((edge.A.Region.number == edge.B.Region.number) ||
            ((edge.A.Region.number == edges[i].A.Region.number) && (edge.B.Region.number == edges[i].B.Region.number)) ||
            ((edge.A.Region.number == edges[i].B.Region.number) && (edge.B.Region.number == edges[i].A.Region.number)))
            {
                return false;
            }
        }
        
        edge.A.AddConnection(edge.B);
        edge.B.AddConnection(edge.A);
        edges.Add(edge);
        return true;
    }


    public GraphVertex MostConnected()
    {
        int max = -99;
        int index = -1;
        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertices[i].Connections.Count > max)
            {
                index = i;
                max = vertices[i].Connections.Count;
            }
        }

        if(index != -1)
            return vertices[index];
        else
            return null;
    }

    public GraphVertex LeastConnected()
    {
        int min = 99;
        int index = -1;

        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertices[i].Connections.Count != 0) // Not accepting 0 connections, because that kind of node is disconnected.
            {
                if(vertices[i].Connections.Count < min)
                {
                    index = i;
                    min = vertices[i].Connections.Count;
                }
            }
        }

        if(index != -1)
            return vertices[index];
        else
            return null;
    }

    public GraphVertex Disconnected()
    {
        int index = -1;

        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertices[i].Connections.Count == 0)
                index = i;
        }

        if(index != -1)
            return vertices[index];
        else
            return null;
    }



    public void PrintGraph()
    {
        GD.Print("\n--Region Graph--");
        GD.Print("Vertex Count: " + vertices.Count);

        if(edges != null)
            GD.Print("Edge Count:" + edges.Count);
        else
            GD.Print("Edge Count: 0");

        string text = "V = {";
        for(int i = 0; i< vertices.Count; i++)
        {
            text += vertices[i] + ", ";
        }
        text = text.Remove(text.Length - 2, 1);  
        text += "}";
        GD.Print(text);

        text = "E = {";
        if(edges != null)
        {
            for(int i = 0; i < edges.Count; i++)
            {
                text += edges[i].A.Region.number + "-" + edges[i].B.Region.number + ", ";
            }
            text = text.Remove(text.Length - 2, 1);  
        }
        text += "}";
        GD.Print(text);

        GraphVertex mostConnected = MostConnected();
        GraphVertex leastConnected = LeastConnected();
        GraphVertex disconnected = Disconnected();

        if(mostConnected != null)
            GD.Print("Most Connected Node: " + mostConnected.Region.number);
        else
            GD.Print("No most connected node found.");

        if(leastConnected != null)
            GD.Print("Least Connected Node: " + leastConnected.Region.number);
        else
            GD.Print("No least connected node found.");

        if(disconnected != null)
            GD.Print("Disconnected Node: " + disconnected.Region.number);
        else
            GD.Print("No disconnected node found.");
    }
}