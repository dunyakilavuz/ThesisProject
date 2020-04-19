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

        bool exists = false;
        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertex.Data == vertices[i].Data)
            {
                exists = true;
                break;
            }
        }

        if(exists == false)
        {
            vertices.Add(vertex);
            return true;
        }
        else
            return false;
    }

    public bool AddEdge(Edge edge)
    {
        if(edges == null)
            edges = new List<Edge>();

        if(edges.Count == 0)
        {
            if(edge.A.Data > 1 && edge.B.Data > 1 && edge.A.Data != edge.B.Data)
            {
                for(int i = 0; i < vertices.Count; i++)
                {
                    if(vertices[i].Data == edge.A.Data)
                        vertices[i].Connections++;
                    if(vertices[i].Data == edge.B.Data)
                        vertices[i].Connections++;
                }
                edges.Add(edge);
                return true;
            }
            else
            {
                return false;
            }
        }

        bool discard = false;
        for(int i = 0; i < edges.Count; i++)
        {
            if(
            (edge.A.Data < 2 || edge.B.Data < 2) ||
            (edge.A.Data == edge.B.Data) ||
            ((edge.A.Data == edges[i].A.Data) && (edge.B.Data == edges[i].B.Data)) ||
            ((edge.A.Data == edges[i].B.Data) && (edge.B.Data == edges[i].A.Data)))
            {
                discard = true;
                break;
            }
        }
        
        if(discard == false)
        {
            for(int i = 0; i < vertices.Count; i++)
            {
                if(vertices[i].Data == edge.A.Data)
                    vertices[i].Connections++;
                if(vertices[i].Data == edge.B.Data)
                    vertices[i].Connections++;
            }
            edges.Add(edge);
            return true;
        }
        else
            return false;
    }


    public GraphVertex MostConnected()
    {
        int max = -99;
        int index = -1;
        for(int i = 0; i < vertices.Count; i++)
        {
            if(vertices[i].Connections > max)
            {
                index = i;
                max = vertices[i].Connections;
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
            if(vertices[i].Connections != 0) // Not accepting 0 connections, because that kind of node is disconnected.
            {
                if(vertices[i].Connections < min)
                {
                    index = i;
                    min = vertices[i].Connections;
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
            if(vertices[i].Connections == 0)
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
        GD.Print("Edge Count:" + edges.Count);

        string text = "V = {";
        for(int i = 0; i< vertices.Count; i++)
        {
            text += vertices[i] + ", ";
        }
        text = text.Remove(text.Length - 2, 1);  
        text += "}";
        GD.Print(text);

        text = "E = {";
        for(int i = 0; i < edges.Count; i++)
        {
            text += edges[i].A.Data + "-" + edges[i].B.Data + ", ";
        }
        text = text.Remove(text.Length - 2, 1);  
        text += "}";
        GD.Print(text);


        GraphVertex mostConnected = MostConnected();
        GraphVertex leastConnected = LeastConnected();
        GraphVertex disconnected = Disconnected();

        if(mostConnected != null)
            GD.Print("Most Connected Node: " + mostConnected.Data);
        else
            GD.Print("No most connected node found.");

        if(leastConnected != null)
            GD.Print("Least Connected Node: " + leastConnected.Data);
        else
            GD.Print("No least connected node found.");

        if(disconnected != null)
            GD.Print("Disconnected Node: " + disconnected.Data);
        else
            GD.Print("No disconnected node found.");
    }
}