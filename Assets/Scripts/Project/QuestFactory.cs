using Godot;
using System.Collections.Generic;
public class QuestFactory : Node
{
    Map map;
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }

    public void GenerateQuests()
    {
        GD.Print("Generate");

        if(References.map == null)
            return;
        else
            map = References.map;

        
        
    }

    public void ClearQuests()
    {
        GD.Print("Clear");
    }

}