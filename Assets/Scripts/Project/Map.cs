using Godot;
using System.Collections.Generic;
public class Map : Spatial
{
    public Chunk[,] gridChunk;
    public int[,] grid;
    public Graph regionGraph;




    public void Init(List<Chunk> chunks)
    {
        gridChunk = new Chunk[References.chunkAmount, References.chunkAmount];

        /*List to Grid*/
        int k = 0;
        for(int i = 0; i < References.chunkAmount; i++)
        {
            for(int j = 0; j < References.chunkAmount; j++)
            {
                gridChunk[i,j] = chunks[k];
                k++;
            }
        }
        /*List to Grid*/

        /*Grid to int*/
        grid = new int[References.chunkAmount, References.chunkAmount];

        for(int i = 0; i < References.chunkAmount; i++)
        {
            for(int j = 0; j < References.chunkAmount; j++)
            {                
                if(gridChunk[i,j].avgHeight > 0 && gridChunk[i,j].slope < References.walkableSlope)
                {
                    grid[i,j] = 1;
                }
                else
                {
                    grid[i,j] = 0;
                }
            }
        }
        /*Grid to int*/

        GenerateRegions();
        GenerateGraph();
    }

    public void GenerateRegions()
    {
        int regions = References.regions;
        int mapSize = References.chunkAmount;

        /*Select random cells as region start points.*/

        Vector2[] regionPositions = new Vector2[regions];
        int u = 0;
        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                if(grid[i,j] == 1)
                {
                    regionPositions[u] = new Vector2(i,j);
                    u++;

                    if(u >= regions)
                        break;
                }
            }
            if(u >= regions)
                break;
        }

        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                if(grid[i,j] == 1)
                {
                    int rnd = References.random.Next(0,mapSize);
                    if(rnd < regions)
                    {
                        regionPositions[rnd] = new Vector2(i,j);
                    }
                }
            }
        }

        int regionNumber = 2; // Since 0 denotes empty and 1 is available cells, regions start from 2.

        for(int i = 0; i < regionPositions.Length; i++)
        {
            Vector2 pos = regionPositions[i];
            grid[(int)pos.x,(int)pos.y] = regionNumber;
            regionNumber++;
        }

        /*Spread of the regions*/
        bool regionSpread;
        int l = 0;

        while(l < regions * 5)
        {
            regionNumber = 2;
            for(int k = 0; k < regions; k++)
            {
                regionSpread = false;
                for(int i = 0; i < mapSize; i++)
                {
                    for(int j = 0; j < mapSize; j++)
                    {
                        if(grid[i,j] == regionNumber)
                        {
                            Vector2 result = checkNeighborsOne(i,j,grid);
                            if(result != Vector2.NegOne)
                            {
                                grid[(int)result.x,(int)result.y] = regionNumber;
                                regionSpread = true;
                                break;
                            }
                        }
                    }
                    if(regionSpread == true)
                    {
                        regionSpread = false;
                        break;
                    }
                }
                regionNumber++;
            }
            l++;
        }

        /*Print Results*/
        printGrid(grid);

    }

    void GenerateGraph()
    {
        int regions = References.regions;
        int mapSize = References.chunkAmount;

        if(regionGraph == null)
            regionGraph = new Graph();

        /*Add Vertices*/
        for(int i = 0; i < References.regions; i++)
        {
            GraphVertex vertex = new GraphVertex(i + 2);
            regionGraph.AddVertex(vertex);
        }

        /*Add Edges*/
        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                GraphVertex A = new GraphVertex(grid[i,j]);
                GraphVertex B;
                Edge edge;

                /*UP*/
                if(i - 1 >= 0)
                {  
                    B = new GraphVertex(grid[i - 1, j]);
                    edge = new Edge(A,B);
                    regionGraph.AddEdge(edge);
                }

                /*LEFT*/
                if(j + 1 < References.chunkAmount)
                {
                    B = new GraphVertex(grid[i, j + 1]);
                    edge = new Edge(A,B);
                    regionGraph.AddEdge(edge);
                }

                /*DOWN*/
                if(i + 1 < References.chunkAmount)
                {
                    B = new GraphVertex(grid[i + 1, j]);
                    edge = new Edge(A,B);
                    regionGraph.AddEdge(edge);
                }

                /*RIGHT*/
                if(j - 1 >= 0)
                {
                    B = new GraphVertex(grid[i, j - 1]);
                    edge = new Edge(A,B);
                    regionGraph.AddEdge(edge);
                }

            }
        }        

        regionGraph.PrintGraph();

    }

    Vector2 checkNeighborsOne(int i, int j, int[,] map)
    {
        /*UP*/
        if(i - 1 >= 0)
        {
            if(map[i - 1, j] == 1)
            {
                return new Vector2(i-1,j);
            }
        }

        /*LEFT*/
        if(j + 1 < References.chunkAmount)
        {
            if(map[i, j + 1] == 1)
            {
                return new Vector2(i,j+1);
            }
        }

        /*DOWN*/
        if(i + 1 < References.chunkAmount)
        {
            if(map[i + 1, j] == 1)
            {
                return new Vector2(i+1,j);
            }
        }

        /*RIGHT*/
        if(j - 1 >= 0)
        {
            if(map[i, j - 1] == 1)
            {
                return new Vector2(i,j-1);
            }
        }

        return Vector2.NegOne;
    }

    void printGrid(int[,] map)
    {
        GD.Print("\n--Region Map--");
        string result = "";
        for(int i = 0; i < References.chunkAmount; i++)
        {
            for(int j = 0; j < References.chunkAmount; j++)
            {
                result += "[" + map[i,j] + "]";
            }
            GD.Print(result);
            result = "";
        }
    }


    public void ClearMap()
    {
        gridChunk = null;
        grid = null;
        regionGraph = null;
    }
    
}