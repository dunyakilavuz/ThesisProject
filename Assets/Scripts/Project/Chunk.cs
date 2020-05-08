using Godot;
using System.Collections.Generic;
public class Chunk : Spatial
{
    OpenSimplexNoise noise;
    public Vector3 position;
    int chunkSize;
    public float avgHeight = 0.0f;
    public float slope = 0;
    public Region region;

    PlaneMesh planeMesh;
    SurfaceTool surfaceTool;
    MeshDataTool meshDataTool;
    ArrayMesh arrayPlane;
    MeshInstance meshInstance;


    public MeshInstance chunkBorders;
    Area collisionArea;
    
    int chunkBorderHeight;
    int collisionAreaHeight;


    public override void _Ready()
    {
        if(region == null)
            region = new Region(-1);
    }

    public override void _Process(float delta)
    {
        if(References.drawChunkBorders)
            chunkBorders.Visible = true;
        else   
            chunkBorders.Visible = false;
    }

    public void AreaEntered(PhysicsBody body)
    {
        try
        {
            Player player = (Player) body;
            if(player != null)
                player.Region = region;
        }
        catch{}
    }

    public void AreaExited(PhysicsBody body)
    {

    }



    public void Init(Vector3 position)
    {
        this.noise = References.noise;
        this.chunkSize = References.chunkSize;
        this.position = position * chunkSize;
        Translation = this.position;

        chunkBorders = (MeshInstance)GetChild(0);
        collisionArea = (Area)GetChild(1);
        
        chunkBorders.Scale = new Vector3(References.chunkSize * 0.5f, References.chunkBorderAreaHeight,References.chunkSize * 0.5f);
        collisionArea.Scale = new Vector3(References.chunkSize * 0.5f,References.chunkCollisionAreaHeight,References.chunkSize * 0.5f);
    }
    public void Generate()
    {
        planeMesh = new PlaneMesh();
        planeMesh.Size = new Vector2(chunkSize, chunkSize);
        planeMesh.SubdivideDepth = Mathf.RoundToInt(chunkSize * 0.5f);
        planeMesh.SubdivideWidth = Mathf.RoundToInt(chunkSize * 0.5f);

        surfaceTool = new SurfaceTool();
        meshDataTool = new MeshDataTool();
        surfaceTool.CreateFrom(planeMesh,0);
        arrayPlane = surfaceTool.Commit();
        meshDataTool.CreateFromSurface(arrayPlane,0);

        for(int i = 0; i < meshDataTool.GetVertexCount(); i++)
        {
            Vector3 vertex = meshDataTool.GetVertex(i);
            vertex.y = noise.GetNoise3d(
                vertex.x + position.x,
                vertex.y,
                vertex.z + position.z) * References.steepness;

            meshDataTool.SetVertex(i,vertex);
            avgHeight += vertex.y;
        }
        avgHeight /= meshDataTool.GetVertexCount();

        for(int i = 0; i < arrayPlane.GetSurfaceCount(); i++)
        {
            arrayPlane.SurfaceRemove(i);
        }
        
        for(int i = 0; i < meshDataTool.GetFaceCount(); i++)
        {
            Vector3 A = meshDataTool.GetVertex(meshDataTool.GetFaceVertex(i,0));
            Vector3 B = meshDataTool.GetVertex(meshDataTool.GetFaceVertex(i,1));
            Vector3 C = meshDataTool.GetVertex(meshDataTool.GetFaceVertex(i,2));      
            Vector3 face = (A + B + C) / 3 + position;
            Vector3 normal = meshDataTool.GetFaceNormal(i);
            slope += Maths.Angle(Vector3.Up,normal);
        }
        slope /= meshDataTool.GetFaceCount();

        meshDataTool.CommitToSurface(arrayPlane);
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        surfaceTool.CreateFrom(arrayPlane,0);
        surfaceTool.GenerateNormals();

        meshInstance = new MeshInstance();
        meshInstance.Mesh = surfaceTool.Commit();
        meshInstance.SetSurfaceMaterial(0, (Material)ResourceLoader.Load("res://Assets/Shader/Terrain.material"));
        meshInstance.CreateTrimeshCollision();
        meshInstance.CastShadow = GeometryInstance.ShadowCastingSetting.On;
        AddChild(meshInstance);
    }

}
