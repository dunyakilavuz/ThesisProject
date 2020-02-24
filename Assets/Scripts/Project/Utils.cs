using Godot;
using System;

public class Utils
{
    public static Spatial InstantiatePrefab(string path, Vector3 position)
    {
        Spatial node;
        Transform transform = Transform.Identity;
        transform.origin = position;
        PackedScene nodeScene = (PackedScene)ResourceLoader.Load(path);
        node = (Spatial)nodeScene.Instance();
        References.world.AddChild(node);
        node.Transform = transform;
        return node;
    }
}
