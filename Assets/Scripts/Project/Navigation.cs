using Godot;
using System;

public class Navigation : Spatial
{
    Spatial camHolder;
    Spatial cam;

    public override void _Ready()
    {
        camHolder = (Spatial)GetNode("/root/MainScene/World/Player/CamHolder");
        cam = (Spatial)GetNode("/root/MainScene/World/Player/CamHolder/Camera");
    }
    public override void _Process(float delta)
    {

    }

    void Rotate()
    {
        if(Input.IsActionPressed("ui_mouseRight"))
        {
            
        }
    }

}
