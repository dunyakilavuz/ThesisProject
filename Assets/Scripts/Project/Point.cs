using Godot;
using System;

public class Point : MeshInstance
{

    public override void _Ready()
    {
        Scale = Vector3.One;
    }

    public override void _Process(float delta)
    {
        
    }

    public void Paint(Color color)
    {
        SpatialMaterial material = new SpatialMaterial();
        material.AlbedoColor = color;
        this.MaterialOverride = material;
    }

}
