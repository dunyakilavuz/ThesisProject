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
            edges.Add(edge);
            return true;
        }
        else
            return false;
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
    }
}