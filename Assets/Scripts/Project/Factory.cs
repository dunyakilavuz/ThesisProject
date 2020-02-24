using Godot;
using System;
using System.Collections.Generic;

public class Factory : Node
{
    Spatial water;
    List<Chunk> chunks = new List<Chunk>();
    List<Point> wayPoints = new List<Point>();
    List<Spatial> trees = new List<Spatial>();



    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {

    }

    public void GenerateTerrain()
    {
        TerrainGenerator();
    }

    public void ClearTerrain()
    {
        TerrainDestroyer();
    }

    public void GeneratePoints()
    {
        WayPointGenerator();
    }

    public void ClearPoints()
    {
        WayPointDestroyer();
    }

    public void GenerateForest()
    {
        ForestGenerator();
    }

    public void ClearForest()
    {
        ForestDestroyer();
    }

    void WayPointGenerator()
    {
        int[,] grid = References.map.grid;
        Chunk[,] chunkGrid = References.map.gridChunk;
        
        if(grid == null || chunkGrid == null)
            return;

        for(int i = 0; i < References.chunkAmount; i++)
        {
            for(int j = 0; j < References.chunkAmount; j++)
            {
                if(grid[i,j] != 0)
                {
                    float scale = 1;
                    if(References.chunkAmount <= References.chunkSize)
                        scale = References.chunkAmount / References.chunkSize;

                    Vector3 pos = new Vector3(
                        chunkGrid[i,j].position.x * scale, 
                        chunkGrid[i,j].position.y + chunkGrid[i,j].avgHeight + 10, 
                        chunkGrid[i,j].position.z * scale);
            
                    Point point = PointGenerator(pos);
                    point.Paint(UIManager.GenerateColor(grid[i,j]));
                    wayPoints.Add(point);

                }
            }
        }
    }

    void WayPointDestroyer()
    {
        for(int i = 0; i < wayPoints.Count; i++)
        {
            wayPoints[i].GetParent().RemoveChild(wayPoints[i]);
        }
        wayPoints.Clear();
        
    }


    void TerrainGenerator()
    {
        if(References.map == null)
        {
            References.map = new Map();
        }

        for(int x = (int)(-References.chunkAmount * 0.5f); x < (int)(References.chunkAmount * 0.5f); x++)
        {
            for(int z = (int)(-References.chunkAmount * 0.5f); z < (int)(References.chunkAmount * 0.5f); z++)
            {
                Vector3 position = new Vector3(x,0,z);
                chunks.Add(ChunkGenerator(position));
            }
        }
        WaterGenerator();
        References.map.Init(chunks);

    }

    void TerrainDestroyer()
    {
        for(int i = 0; i < chunks.Count; i++)
        {
            chunks[i].GetParent().RemoveChild(chunks[i]);
        }
        chunks.Clear();
        WaterDestroyer();
        References.map.ClearMap();
    }

    void WaterGenerator()
    {
        Vector3 waterPos = new Vector3(-References.chunkSize * 0.5f, 0, -References.chunkSize * 0.5f);
        water = (Spatial)Utils.InstantiatePrefab("res://Assets/Prefabs/Water.res", waterPos);
        water.Scale = Vector3.One * References.chunkSize * References.chunkAmount * 0.5f;
    }

    void WaterDestroyer()
    {
        water.GetParent().RemoveChild(water);
        water = null;
    }

    void ForestGenerator()
    {
        for(int i = 0; i < chunks.Count; i++)
        {
            int rnd = References.random.Next(0,100);
            if(chunks[i].avgHeight > 0 && References.forestChance > rnd)
            {
                Vector3 pos = new Vector3(chunks[i].position.x, chunks[i].avgHeight, chunks[i].position.z);
                trees.Add(TreeGenerator(pos));
            }
        }
    }

    void ForestDestroyer()
    {
        for(int i = 0; i < trees.Count; i++)
        {
            trees[i].GetParent().RemoveChild(trees[i]);
        }
        trees.Clear();
    }

    Chunk ChunkGenerator(Vector3 position)
    {
        Chunk chunk = (Chunk)Utils.InstantiatePrefab("res://Assets/Prefabs/Chunk.res", position);
        chunk.Init(position);
        chunk.Generate();
        return chunk;
    }

    Point PointGenerator(Vector3 position)
    {
        Point point = (Point)Utils.InstantiatePrefab("res://Assets/Prefabs/Point.res", position);
        return point;
    }

    Spatial TreeGenerator(Vector3 position)
    {
        Spatial tree = (Spatial)Utils.InstantiatePrefab("res://Assets/Prefabs/PineTree.res", position);
        return tree;
    }
}