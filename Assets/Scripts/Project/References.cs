using Godot;
using System;

public class References : Node
{
    public static Spatial world;
    public static Main main;
    public static Player player;
    public static TerrainFactory terrainFactory;
    public static QuestFactory questFactory;

    public static Map map;
    public static int chunkSize;
    public static int chunkAmount;
    public static int steepness;
    public static int walkableSlope;
    public static int forestChance;
    public static int regions;
    public static int chunkAreaHeight;
    public static bool drawChunkBorders;
    public static OpenSimplexNoise noise;
    public override void _Ready()
    {
        world = (Spatial)GetNode("/root/MainScene/World");
        player = (Player)GetNode("/root/MainScene/World/Player");
        terrainFactory = (TerrainFactory)GetNode("/root/MainScene/Main/TerrainFactory");
        questFactory = (QuestFactory)GetNode("/root/MainScene/Main/QuestFactory");

        noise = new OpenSimplexNoise();
        noise.Octaves = 3;
        noise.Period = 100;

        steepness = 50;
        chunkAreaHeight = 50;

    }

    public override void _Process(float delta)
    {
        
    }
}
