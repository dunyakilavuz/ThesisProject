using Godot;
using System;

public class References : Node
{
    public static Spatial world;
    public static Main main;
    public static Player player;
    public static TerrainFactory terrainFactory;
    public static QuestFactory questFactory;
    public static TextureRect noiseTexture;

    public static Map map;
    public static int chunkSize;
    public static int chunkAmount;
    public static int steepness;
    public static int walkableSlope;
    public static int regionSize;
    public static int forestChance;
    public static int chunkBorderAreaHeight;
    public static int chunkCollisionAreaHeight;
    public static bool drawChunkBorders;
    public static OpenSimplexNoise noise;

    public override void _Ready()
    {
        world = (Spatial)GetNode("/root/MainScene/World");
        player = (Player)GetNode("/root/MainScene/World/Player");
        terrainFactory = (TerrainFactory)GetNode("/root/MainScene/Main/TerrainFactory");
        questFactory = (QuestFactory)GetNode("/root/MainScene/Main/QuestFactory");
        noiseTexture = (TextureRect) GetNode("/root/MainScene/Main/NoiseTexture");

        noise = ((NoiseTexture) noiseTexture.Texture).Noise;
        ((LineEdit)GetNode("/root/MainScene/UI/SettingsPanel/SeedLine")).Text = noise.Seed.ToString();

        steepness = 50;
        chunkBorderAreaHeight = 30;
        chunkCollisionAreaHeight = 500;
        forestChance = 50;
    }

    public override void _Process(float delta)
    {
        if(Input.IsActionJustReleased("ui_accept"))
		{
            int iter = 0;
            chunkSize = 30;
            chunkAmount = 10;
            regionSize = 100;
            int questCount = 0;
            terrainFactory.GenerateTerrain();
			GD.Print("-- Benchmark start --");

            while(iter < 100)
            {
                questCount += 10;

                var watch = System.Diagnostics.Stopwatch.StartNew();
                for(int i = 0; i < questCount; i++)
                {
                    questFactory.GenerateQuests();
                }
                watch.Stop();
                questFactory.ClearQuests();
                GD.Print(questCount + " " + watch.ElapsedMilliseconds * 0.001);
                iter++;
            }

            GD.Print("-- Benchmark end --");
            terrainFactory.ClearTerrain();
		}
    }
}
