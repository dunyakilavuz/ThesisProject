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
        List<Vector2> availableCells = new List<Vector2>();

        /*Select random cells as region start points.*/
        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                if(grid[i,j] == -1)
                {
                    availableCells.Add(new Vector2(i,j));
                }
            }
        }
        int maxIndex = regionSize;
        if(regionSize > availableCells.Count)
        {
            GD.Print("Region size is larger than availableCells, creating " +  availableCells.Count + " regions.");
            maxIndex = availableCells.Count;
        }

        Vector2[] regionPositions = new Vector2[regionSize];

        for(int i = 0; i < maxIndex; i++)
        {
            int rnd = Maths.random.Next(availableCells.Count);
            regionPositions[i] = availableCells[rnd];
            availableCells.RemoveAt(rnd);
        }
        availableCells.Clear();

        for(int i = 0; i < regionSize; i++)
        {
            Region region = new Region(i);
            regions[i] = region;
            Vector2 pos = regionPositions[i];
            grid[(int)pos.x,(int)pos.y] = region.number;
        }
        
        /*Spread of the regions*/

        bool regionSpread;
        bool spreadContinues = true;
        int l = 0;
//        GD.Print("Iteration: " + l);
//        printGrid(grid);

        while(spreadContinues)
        {
            spreadContinues = false;
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
                        spreadContinues = true;
                        regionSpread = false;
                        break;
                    }
                }
            }
            l++;
//            GD.Print("Iteration: " + l);
//            printGrid(grid);
        }

        /*Print Results*/
//        printGrid(grid);  //Comment when benchmarking
    }

    void GenerateGraph()
    {
        int mapSize = References.chunkAmount;

        if(regionGraph == null)
            regionGraph = new Graph();

        /*Add Vertices*/
        for(int i = 0; i < References.regionSize; i++)
        {
            GraphVertex vertex = new GraphVertex(regions[i]);
            regionGraph.AddVertex(vertex);
        }

        /*Add Edges*/
        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                GraphVertex A;
                GraphVertex B;
                Edge edge;

                if(grid[i,j] >= 0 && grid[i,j] < regions.Length)
                {
                    A = regionGraph.GetVertex(grid[i,j]);
                    /*UP*/
                    if(i - 1 >= 0)
                    {  
                        int value = grid[i - 1, j];
                        if(value >= 0 && value < regions.Length)
                        {
                            B = regionGraph.GetVertex(value);
                            edge = new Edge(A,B);
                            regionGraph.AddEdge(edge);
                        }
                    }

                    /*LEFT*/
                    if(j + 1 < References.chunkAmount)
                    {
                        int value = grid[i, j + 1];
                        if(value >= 0 && value < regions.Length)
                        {
                            B = regionGraph.GetVertex(value);
                            edge = new Edge(A,B);
                            regionGraph.AddEdge(edge);
                        }
                    }

                    /*DOWN*/
                    if(i + 1 < References.chunkAmount)
                    {
                        int value = grid[i + 1, j];
                        if(value >= 0 && value < regions.Length)
                        {
                            B = regionGraph.GetVertex(value);
                            edge = new Edge(A,B);
                            regionGraph.AddEdge(edge);
                        }
                    }

                    /*RIGHT*/
                    if(j - 1 >= 0)
                    {
                        int value = grid[i, j - 1];
                        if(value >= 0 && value < regions.Length)
                        {
                            B = regionGraph.GetVertex(value);
                            edge = new Edge(A,B);
                            regionGraph.AddEdge(edge);
                        }
                    }
                }
            }
        }        

//        regionGraph.PrintGraph(); //Comment when benchmarking
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