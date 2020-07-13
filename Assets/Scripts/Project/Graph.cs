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

    public GraphVertex[] RandomPath(bool astar = true)
    {
        List<GraphVertex> vertexPool = new List<GraphVertex>();
        GraphVertex A = null;
        GraphVertex B = null;
        List<GraphVertex> path = null;
        int rnd;
        int maxIter = 1000;
        int iter = 0;
        
        while(iter < maxIter)
        {
            iter++;
            for(int i = 0; i < vertices.Count; i++) // Fill the vertex pool.
            {
                vertexPool.Add(vertices[i]);
            }

            rnd = Maths.random.Next(vertexPool.Count);
            A = vertexPool[rnd];
            vertexPool.RemoveAt(rnd);
            rnd = Maths.random.Next(vertexPool.Count);
            B = vertexPool[rnd];

            if(astar)
                path = this.PathAstar(A,B);
            else
                path = this.PathDijkstra(A,B);

            if(path != null)
            {
                return path.ToArray();
            }
            else
            {
                vertexPool.Clear();
                A = null;
                B = null;
            }
        }
        GD.Print("Can't generate path.");
        return null;
    }

    GraphVertex AStar(GraphVertex A, GraphVertex B)
    {
        List<GraphVertex> open = new List<GraphVertex>();
        List<GraphVertex> closed = new List<GraphVertex>();
        GraphVertex current;
        List<GraphVertex> neighbors;

        for(int i = 0; i < vertices.Count; i++)
        {
            vertices[i].distance = Maths.INF;
            vertices[i].prev = null;
        }

        open.Add(A);
        A.distance = 0;
        
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

    public List<GraphVertex> PathAstar(GraphVertex A, GraphVertex B)
    {
        GraphVertex vert = AStar(A,B);

        if(vert == null)
        {
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
        return path;
    }

    int[] Dijkstra(GraphVertex source)
    {
        List<GraphVertex> verts = new List<GraphVertex>();
        GraphVertex current = null;
        int[] distance = new int[vertices.Count];

        for(int i  = 0; i < vertices.Count; i++)
        {
            distance[i] = int.MaxValue;
            vertices[i].prev = null;
            verts.Add(vertices[i]);
        }
        distance[source.Region.number] = 0;

        while(verts.Count > 0)
        {
            int min = int.MaxValue;
            int index = -1;
            for(int i = 0; i < verts.Count; i++)
            {
                if(distance[verts[i].Region.number] <= min)
                {
                    min = distance[verts[i].Region.number];
                    index = i;
                }
            }
            current = verts[index];
            verts.RemoveAt(index);
            List<GraphVertex> neighbors = Neighbors(current);

            for(int i = 0; i < neighbors.Count; i++)
            {
                if(verts.Contains(neighbors[i]))
                {
                    int newDistance = distance[current.Region.number] + 1;
                    if(newDistance < distance[neighbors[i].Region.number])
                    {
                        distance[neighbors[i].Region.number] = newDistance;
                        neighbors[i].prev = current;
                    }
                }
            }
        }

        return distance;
    }

    
    public List<GraphVertex> PathDijkstra(GraphVertex A, GraphVertex B)
    {
        int[] distance = Dijkstra(A);

        if(distance[B.Region.number] == int.MaxValue)
            return null;

        List<GraphVertex> path = new List<GraphVertex>();
        GraphVertex current = B;

        while(true)
        {
            path.Add(current);

            if(current.prev != null)
                current = current.prev;
            else
                break;
        }
        path.Reverse();
        return path;
    }


    List<GraphVertex> Neighbors(GraphVertex A)
    {
        List<GraphVertex> neighbors = new List<GraphVertex>();

        if(edges == null)
            return null;

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

    public static void PrintList(List<GraphVertex> list)
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
    }
}