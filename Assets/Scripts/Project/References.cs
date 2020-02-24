using Godot;
using System;

public class References : Node
{
    public static Spatial world;
    public static Main main;
    public static Player player;
    public static Factory factory;

    public static Map map;
    public static int chunkSize;
    public static int chunkAmount;
    public static int Steepness;
    public static int walkableSlope;
    public static int forestChance;
    public static int regions;
    public static OpenSimplexNoise noise;
    public static Random random;
    public override void _Ready()
    {
        world = (Spatial)GetNode("/root/MainScene/World");
        player = (Player)GetNode("/root/MainScene/World/Player");
        factory = (Factory)GetNode("/root/MainScene/Main/Factory");

        random = new Random();
        noise = new OpenSimplexNoise();
        noise.Octaves = 3;
        noise.Period = 100;

    }

    public override void _Process(float delta)
    {
        
    }
}
