using Godot;
using Vector3 = Godot.Vector3;
using System.Collections.Generic;

public class Player : KinematicBody
{
    float moveForce = 50;
    float rotateVelocity = 2;
    float jumpForce = 1;

    Vector3 velocity = Vector3.Zero;
    float deltaTime;

    Vector2 mousePosLast = Vector2.Zero;
    Vector2 mousePos = Vector2.Zero;
    Region region;
    Vector3 position;

    List<Quest> quests;
    List<Quest> completedQuests;
    Quest activeQuest;

    public override void _Ready()
    {

    }


    public override void _Process(float delta)
    {
        deltaTime = delta;
        RotatePlayer();
        MovePlayer();
    }

    public Region Region
    {
        get
        {
            return region;
        }

        set
        {
            region = value;
        }
    }

    public Quest ActiveQuest
    {
        get
        {
            return activeQuest;
        }

        set
        {
            activeQuest = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    public void AcceptQuest(Quest q)
    {
        if(quests == null)
            quests = new List<Quest>();
        quests.Add(q);

        if(activeQuest == null)
            activeQuest = quests[0];
    }

    public void CompleteObjective()
    {
        activeQuest.CompleteObjective(region.number);

        if(activeQuest.Done)
        {
            quests.Remove(activeQuest);

            if(completedQuests == null)
                completedQuests = new List<Quest>();

            completedQuests.Add(activeQuest);

            if(quests.Count > 0)
                activeQuest = quests[0];
        }
    }

    public void ClearQuests()
    {
        activeQuest = null;

        if(quests != null)
            quests.Clear();
        if(completedQuests != null)
            completedQuests.Clear();
    }

    void MovePlayer()
    {
        velocity = Vector3.Zero;

        if(Input.IsActionPressed("ui_forward"))
        {
            velocity += -Transform.basis.z * moveForce * deltaTime;
        }
        if(Input.IsActionPressed("ui_back"))
        {
            velocity += Transform.basis.z * moveForce * deltaTime;
        }
        if(Input.IsActionPressed("ui_up"))
        {
            velocity += Transform.basis.y * moveForce * deltaTime;
        }
        if(Input.IsActionPressed("ui_down"))
        {
            velocity += -Transform.basis.y * moveForce * deltaTime;
        }
        if(Input.IsActionPressed("ui_left"))
        {
            velocity += -Transform.basis.x * moveForce * deltaTime;
        }
        if(Input.IsActionPressed("ui_right"))
        {
            velocity += Transform.basis.x * moveForce * deltaTime;
        }
        if(Input.IsActionPressed("ui_speed"))
        {
            velocity += velocity * 2;
        }

        MoveAndCollide(velocity);
        position = Translation;
    }

    void RotatePlayer()
    {
        mousePos = GetViewport().GetMousePosition();
        if(Input.IsActionPressed("ui_mouseRight"))
        {
            float angleY = mousePosLast.x - mousePos.x;
            float angleX = mousePosLast.y - mousePos.y;
            Rotate(Vector3.Up, angleY * deltaTime);
            Rotate(Transform.basis.x.Normalized(), angleX * deltaTime);
        }
        mousePosLast = mousePos;
    }
    
}
