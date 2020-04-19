using Godot;
using System;
using System.Collections.Generic;

public class TerrainFactory : Node
{
    Spatial water;
    List<Chunk> chunks = new List<Chunk>();
    List<Spatial> trees = new List<Spatial>();
    List<Spatial> boulders = new List<Spatial>();
    List<Spatial> grasses = new List<Spatial>();

    float dispRadius = 3; // Displacement of the trees, boulders etc.



    public override void _Ready()
    {
        dispRadius = References.chunkSize * 0.5f;
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

    public void GenerateForest()
    {
        ForestGenerator();
    }

    public void ClearForest()
    {
        ForestDestroyer();
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
        GD.Print("Seed: " + References.noise.Seed);
        WaterGenerator();
        ForestGenerator();
        References.map.Init(chunks);

        int[,] grid = References.map.grid;
        Chunk[,] chunkGrid = References.map.gridChunk;
        for(int i = 0; i < References.chunkAmount; i++)
        {
            for(int j = 0; j < References.chunkAmount; j++)
            {
                if(grid[i,j] != 0)
                {
                    MeshInstance border = chunkGrid[i,j].chunkBorders;
                    Color borderColor = UIManager.IntToColor(grid[i,j],1);
                    Paint(border,borderColor);
                    chunkGrid[i,j].regionNumber = grid[i,j];
                }
            }
        }

    }

    void TerrainDestroyer()
    {
        for(int i = 0; i < chunks.Count; i++)
        {
            chunks[i].GetParent().RemoveChild(chunks[i]);
        }
        chunks.Clear();
        WaterDestroyer();
        ForestDestroyer();
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
            int rnd = Maths.RandomInt(0,100);
            if(chunks[i].avgHeight > 0 && References.forestChance > rnd)
            {
                Vector3 pos = new Vector3(chunks[i].position.x, chunks[i].avgHeight, chunks[i].position.z);
                trees.Add(TreeGenerator(pos));
                chunks[i].treeCount++;
            }
            
            rnd = Maths.RandomInt(0,100);
            if(chunks[i].avgHeight > 0 && References.forestChance > rnd)
            {
                float dispX = Maths.RandomFloat(-dispRadius, dispRadius);
                float dispZ = Maths.RandomFloat(-dispRadius, dispRadius);
                float dispAngle = Maths.RandomFloat(0,180);
                Vector3 pos = new Vector3(chunks[i].position.x, chunks[i].avgHeight, chunks[i].position.z);
                Vector3 displacement = new Vector3(dispX,-0.5f,dispZ);
                Quat rotation = new Quat(Vector3.Up, dispAngle * Maths.Deg2Rad);
                boulders.Add(BoulderGenerator(pos + displacement,rotation));
                chunks[i].boulderCount++;

            }

            rnd = Maths.RandomInt(0,100);
            if(chunks[i].avgHeight > 0 && References.forestChance > rnd)
            {
                float dispX = Maths.RandomFloat(-dispRadius * 0.5f, dispRadius * 0.5f);
                float dispZ = Maths.RandomFloat(-dispRadius * 0.5f, dispRadius * 0.5f);
                float dispAngle = Maths.RandomFloat(0,180);
                Vector3 pos = new Vector3(chunks[i].position.x, chunks[i].avgHeight, chunks[i].position.z);
                Vector3 displacement = new Vector3(dispX,-0.5f,dispZ);
                Quat rotation = new Quat(Vector3.Up, dispAngle * Maths.Deg2Rad);
                grasses.Add(GrassGenerator(pos + displacement,rotation));
                chunks[i].grassCount++;
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

        for(int i = 0; i < boulders.Count; i++)
        {
            boulders[i].GetParent().RemoveChild(boulders[i]);
        }
        boulders.Clear();

        for(int i = 0; i < grasses.Count; i++)
        {
            grasses[i].GetParent().RemoveChild(grasses[i]);
        }
        grasses.Clear();
    }

    Chunk ChunkGenerator(Vector3 position)
    {
        Chunk chunk = (Chunk)Utils.InstantiatePrefab("res://Assets/Prefabs/Chunk.res", position);
        chunk.Init(position);
        chunk.Generate();
        return chunk;
    }

    Spatial TreeGenerator(Vector3 position)
    {
        Spatial tree = (Spatial)Utils.InstantiatePrefab("res://Assets/Prefabs/PineTree.res", position);
        tree.Scale = Vector3.One * Maths.RandomFloat(2,3);
        return tree;
    }

    Spatial BoulderGenerator(Vector3 position, Quat rotation)
    {
        int whichBoulder = Maths.RandomInt(1,4);
        Spatial boulder = (Spatial)Utils.InstantiatePrefab("res://Assets/Prefabs/Boulder" + whichBoulder + ".res", position);
        boulder.Transform = new Transform(new Basis(rotation),position);
        boulder.Scale = Vector3.One * Maths.RandomFloat(1,2);
        return boulder;
    }

    Spatial GrassGenerator(Vector3 position, Quat rotation)
    {
        Spatial grass = (Spatial)Utils.InstantiatePrefab("res://Assets/Prefabs/Grass.res", position);
        grass.Transform = new Transform(new Basis(rotation),position);
        grass.Scale = Vector3.One * Maths.RandomFloat(1,2);
        return grass;
    }

    public static void Paint(Spatial obj, Color color)
    {
        SpatialMaterial material = new SpatialMaterial();
        material.AlbedoColor = color;
        material.FlagsTransparent = true;
        ((MeshInstance) obj).MaterialOverride = material;
    }
}