using Godot;
using System.Collections.Generic;
public class Map : Spatial
{
    public Chunk[,] gridChunk;
    public Region[] regions;
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
                    grid[i,j] = -1; // Available cells for regions
                }
                else
                {
                    grid[i,j] = -2; // Non available cells.
                }
            }
        }
        /*Grid to int*/

        GenerateRegions();
        GenerateGraph();
    }

    public void GenerateRegions()
    {
        int regionSize = References.regionSize;
        int mapSize = References.chunkAmount;
        regions = new Region[regionSize];

        /*Select random cells as region start points.*/
        Vector2[] regionPositions = new Vector2[regionSize];
        int u = 0;
        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                if(grid[i,j] == -1)
                {
                    regionPositions[u] = new Vector2(i,j);
                    u++;

                    if(u >= regionSize)
                        break;
                }
            }
            if(u >= regionSize)
                break;
        }

        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                if(grid[i,j] == -1)
                {
                    int rnd = Maths.RandomInt(0,mapSize);
                    if(rnd < regionSize)
                    {
                        regionPositions[rnd] = new Vector2(i,j);
                    }
                }
            }
        }

        for(int i = 0; i < regionSize; i++)
        {
            Region region = new Region(i);
            regions[i] = region;
            Vector2 pos = regionPositions[i];
            grid[(int)pos.x,(int)pos.y] = region.number;
        }

        /*Spread of the regions*/

        bool regionSpread;
        int l = 0;

        while(l < regionSize * 5)
        {
            for(int k = 0; k < regionSize; k++)
            {
                regionSpread = false;
                for(int i = 0; i < mapSize; i++)
                {
                    for(int j = 0; j < mapSize; j++)
                    {
                        if(grid[i,j] == k)
                        {
                            Vector2 result = checkNeighbors(i,j,grid);
                            if(result != Vector2.NegOne)
                            {
                                grid[(int)result.x,(int)result.y] = k;
                                gridChunk[(int)result.x,(int)result.y].region = regions[k];
                                regions[k].AddChunk(gridChunk[(int)result.x,(int)result.y]);
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
            }
            l++;
        }

        for(int i = 0; i < regions.Length; i++)
            regions[i].CalcRegionProperties();

        /*Print Results*/
        printGrid(grid);
        
        for(int i = 0; i < regions.Length; i++)
            regions[i].printRegion();

    }

    void GenerateGraph()
    {
        int mapSize = References.chunkAmount;

        if(regionGraph == null)
            regionGraph = new Graph();

        /*Add Vertices*/
        for(int i = 0; i < References.regionSize; i++)
        {
            GraphVertex vertex = new GraphVertex(i);
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

    Vector2 checkNeighbors(int i, int j, int[,] map)
    {
        /*UP*/
        if(i - 1 >= 0)
        {
            if(map[i - 1, j] == -1)
            {
                return new Vector2(i-1,j);
            }
        }

        /*LEFT*/
        if(j + 1 < References.chunkAmount)
        {
            if(map[i, j + 1] == -1)
            {
                return new Vector2(i,j+1);
            }
        }

        /*DOWN*/
        if(i + 1 < References.chunkAmount)
        {
            if(map[i + 1, j] == -1)
            {
                return new Vector2(i+1,j);
            }
        }

        /*RIGHT*/
        if(j - 1 >= 0)
        {
            if(map[i, j - 1] == -1)
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
                if(map[i,j] < 0)
                    result += "[" + map[i,j] + " ]";
                else
                    result += "[ " + map[i,j] + " ]";
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