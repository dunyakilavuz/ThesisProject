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
        
        edges.Add(edge);
        return true;
    }

    GraphVertex AStar(GraphVertex A, GraphVertex B)
    {
        List<GraphVertex> open = new List<GraphVertex>();
        List<GraphVertex> closed = new List<GraphVertex>();
        open.Add(A);
        A.distance = 0;
        GraphVertex current;
        List<GraphVertex> neighbors;

        while(open.Count > 0)
        {
            int min = MinList(open);
            current = open[min];
            neighbors = Neighbors(current);

            if(current.Equals(B))
                return B;

            RemoveElement(open,current);
            closed.Add(current);

            for(int i = 0; i < neighbors.Count; i++)
            {
                if(!ContainsElement(closed,neighbors[i]))
                {
                    if(!ContainsElement(open,neighbors[i]))
                    {
                        float newDistance = current.distance + 1;

                        if(neighbors[i].distance > newDistance)
                        {
                            open.Add(neighbors[i]);
                            neighbors[i].distance = newDistance;
                            neighbors[i].prev = current;
                        }
                    }

                }

            }
        }
        return null;
    }

    public List<GraphVertex> Path(GraphVertex A, GraphVertex B)
    {
        GraphVertex vert = AStar(A,B);

        if(vert == null)
        {
            GD.Print("Path is unreachable.");
            return null;
        }

        List<GraphVertex> path = new List<GraphVertex>();
        GraphVertex current = vert;

        while(true)
        {
            path.Add(current);

            if(current.prev != null)
                current = current.prev;
            else
                break;
        }
        path.Reverse();
        PrintList(path);
        return path;
    }

    public List<GraphVertex> Neighbors(GraphVertex A)
    {
        List<GraphVertex> neighbors = new List<GraphVertex>();

        for(int i = 0; i < edges.Count; i++)
        {
            if(A.Equals(edges[i].A))
                neighbors.Add(edges[i].B);
            if(A.Equals(edges[i].B))
                neighbors.Add(edges[i].A);
        }
        return neighbors;
    }


    public GraphVertex MostConnected()
    {
        int max = -99;
        int index = -1;
        for(int i = 0; i < vertices.Count; i++)
        {
            if(Neighbors(vertices[i]).Count > max)
            {
                index = i;
                max = Neighbors(vertices[i]).Count;;
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
            if(Neighbors(vertices[i]).Count != 0) // Not accepting 0 connections, because that kind of node is disconnected.
            {
                if(Neighbors(vertices[i]).Count < min)
                {
                    index = i;
                    min = Neighbors(vertices[i]).Count;
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
            if(Neighbors(vertices[i]).Count == 0)
                index = i;
        }

        if(index != -1)
            return vertices[index];
        else
            return null;
    }

    void PrintList(List<GraphVertex> list)
    {
        if(list.Count == 0 || list == null)
        {
            GD.Print("List empty.");
            return;
        }

        string listStr = "{";
        for(int i = 0; i < list.Count; i++)
        {
            listStr += list[i].Region.number;
            if(i + 1 < list.Count)
                listStr += ",";
            else
                listStr += "}";
        }
        GD.Print(listStr);
    }

    int MinList(List<GraphVertex> list)
    {
        int index = -1;
        float min = Maths.INF;

        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].distance < min)
            {
                min = list[i].distance;
                index = i;
            }
        }
        return index;
    }

    bool ContainsElement(List<GraphVertex> list, GraphVertex element)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].Equals(element))
                return true;
        }
        return false;
    }

    void RemoveElement(List<GraphVertex> list, GraphVertex element)
    {
        int index = -1;
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].Equals(element))
            {
                index = i;
            }
        }

        if(index != -1)
        {
            list.RemoveAt(index);
        }
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