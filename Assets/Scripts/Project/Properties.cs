using Godot;
using System;
using System.Collections.Generic;
public class Properties
{
    int enemies;
    int cover;
    int resources;
    bool escortableNPC;
    bool deliverableNPC;
    bool defendableArea;
    bool interactableOBJ;

    public Properties()
    {
        enemies = Maths.RandomInt(0,10);
        cover = Maths.RandomInt(0,10);
        resources = Maths.RandomInt(0,10);
        escortableNPC = Maths.RandomInt(0,1) != 0;
        deliverableNPC = Maths.RandomInt(0,1) != 0;
        defendableArea = Maths.RandomInt(0,1) != 0;
        interactableOBJ = Maths.RandomInt(0,1) != 0;
    }

    public Properties(Properties p)
    {
        this.enemies = p.enemies;
        this.cover = p.cover;
        this.resources = p.resources;
        this.escortableNPC = p.escortableNPC;
        this.deliverableNPC = p.deliverableNPC;
        this.defendableArea = p.defendableArea;
        this.interactableOBJ = p.interactableOBJ;
    }

    public int Enemies
    {
        get{return enemies;}
        set{enemies = value;}
    }
    public int Cover
    {
        get{return cover;}
    }

    public int Resources
    {
        get{return resources;}
        set{resources = value;}
    }

    public bool EscortableNPC
    {
        get{return escortableNPC;}
        set{escortableNPC = value;}
    }
    public bool DeliverableNPC
    {
        get{return deliverableNPC;}
        set{deliverableNPC = value;}
    }

    public bool DefendableArea
    {
        get{return defendableArea;}
        set{defendableArea = value;}
    }

    public bool InteractableOBJ
    {
        get{return interactableOBJ;}
        set{interactableOBJ = value;}
    }
}