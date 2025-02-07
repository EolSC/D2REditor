
using UnityEngine;

public enum TileStatus
{
    Empty,
    Special
}
public enum TileHover
{
    None,
    Hovered,
}
public enum TileSelection
{
    None,
    Selected,
}


public class Tile
{
    static float LINE_THICKNESS = 1.0f;
    static float TILE_GRID_Z = 0.1f;
    static float WALKABLE_Z = 0.15f;

    private Mesh mesh;

    private Mesh[] walkableInfo = new Mesh[DT1Block.SUBTILES_COUNT];
    private Material[] walkableMaterials = new Material[DT1Block.SUBTILES_COUNT];
    private Material material;
    private Collider collider;

    public int x = 0;
    public int y = 0;

    public TileSelection selection = TileSelection.None;
    public TileStatus status = TileStatus.Empty;
    public TileHover hover = TileHover.None;

    public void SetSelection(TileSelection selection)
    {
        if (this.selection != selection)
        {
            this.selection = selection;
            UpdateStatus();
        }
    }

    public void SetStatus(TileStatus status)
    {
        if (this.status != status)
        {
            this.status = status;
            UpdateStatus();
        }
    }
    public void SetHover(TileHover hover)
    {
        if (this.hover != hover)
        {
            this.hover = hover;
            UpdateStatus();
        }
    }

    public void UpdateWalkableInfo(byte[] walkableData)
    {
        for (int y = 0; y < DT1Block.SUBTILES_Y; ++y)
        {
            for (int x = 0; x < DT1Block.SUBTILES_X; ++x)
            {
                var index = y * DT1Block.SUBTILES_Y + x;
                UpdateWalkableStatus(walkableMaterials[index], walkableData[index]);
            }
        }
    }

    private void UpdateWalkableStatus(Material material, byte walkable)
    {
        switch (walkable)
        {

            case 0:
                {
                    material.color = Color.green;
                }; break;
            default:
            case 1:
                {
                    material.color = Color.red;
                }; break;
        }
    }

    private void FlipMesh(Mesh mesh)
    {
        var vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; ++i)
        {
            vertices[i].Scale(new Vector3(-1, 1, 1));
        }

        var indexes = mesh.triangles;
        for (int i = 0; i < indexes.Length; i += 3)
        {
            int temp = indexes[i];
            indexes[i] = indexes[i + 2];
            indexes[i + 2] = temp;
        }

        mesh.vertices = vertices;
        mesh.triangles = indexes;
    }

    private void CreateWalkableInfo(Vector3 tile_world_pos)
    {
        for (int y = 0; y < DT1Block.SUBTILES_Y; ++y)
        {
            for (int x = 0; x < DT1Block.SUBTILES_X; ++x)
            {
                var mesh = new Mesh();
                material = new Material(Shader.Find("Standard"));
                material.SetFloat("_Glossiness", 0.0f);
                material.color = Color.red;
                var position = tile_world_pos + new Vector3(x * CoordinateSystem.SUBTILE_SIZE_X, 0.0f, y * CoordinateSystem.SUBTILE_SIZE_Y);

                Vector3 v1 = position + new Vector3(0.0f, WALKABLE_Z, 0.0f);
                Vector3 v2 = new Vector3(CoordinateSystem.SUBTILE_SIZE_X, 0.0f, 0.0f);
                Vector3 v3 = new Vector3(0.0f, 0.0f, CoordinateSystem.SUBTILE_SIZE_Y);
                Vector3 v4 = new Vector3(CoordinateSystem.SUBTILE_SIZE_X, 0.0f, CoordinateSystem.SUBTILE_SIZE_Y);


                Vector3[] vertices = new Vector3[]
                {
                    v1,
                    v1 + v2,
                    v1 + v3,
                    v1 + v4,
                };

                mesh.vertices = vertices;
                int[] indexes = new int[]
                {
                    // lower left triangle
                    0, 2, 1,
                    // upper right triangle
                    2, 3, 1,
                };
                mesh.triangles = indexes;
                FlipMesh(mesh);

                mesh.RecalculateBounds();
                mesh.RecalculateNormals();

                var index = y * DT1Block.SUBTILES_Y + x;
                walkableInfo[index] = mesh;
                walkableMaterials[index] = material;

            }
        }
    }

    public void Create(BoxCollider boxCollider, int x, int y, int z)
    {
        Vector3 world_pos = new Vector3(
            x * CoordinateSystem.TILE_GRID_STEP.x, 
            y * CoordinateSystem.TILE_GRID_STEP.y + TILE_GRID_Z, 
            z * CoordinateSystem.TILE_GRID_STEP.z
            );
        CreateWalkableInfo(world_pos);

        mesh = new Mesh();
        material = new Material(Shader.Find("Standard"));
        material.SetFloat("_Glossiness", 0.0f);
        Vector3 v1 = world_pos;
        Vector3 v2 = new Vector3(CoordinateSystem.TILE_GRID_SIZE, 0.0f, 0.0f);
        Vector3 v3 = new Vector3(0.0f, 0.0f, LINE_THICKNESS);
        Vector3 v4 = new Vector3(CoordinateSystem.TILE_GRID_SIZE, 0.0f, LINE_THICKNESS);


        Vector3 v5 = v1 + v2;

        Vector3 v6 = new Vector3(0.0f, 0.0f, CoordinateSystem.TILE_GRID_SIZE);
        Vector3 v7 = new Vector3(-LINE_THICKNESS, 0.0f, 0.0f);
        Vector3 v8 = new Vector3(-LINE_THICKNESS, 0.0f, CoordinateSystem.TILE_GRID_SIZE);

        Vector3 v9 = v1 + new Vector3(LINE_THICKNESS, 0.0f, 0.0f);

        Vector3 v10 = v6;
        Vector3 v11 = v7;
        Vector3 v12 = v8;

        Vector3 v13 = v1 + new Vector3(0.0f, 0.0f, CoordinateSystem.TILE_GRID_SIZE - LINE_THICKNESS);

        Vector3 v14 = v2;
        Vector3 v15 = v3;
        Vector3 v16 = v4;

        Vector3[] vertices = new Vector3[]
        {
            v1,
            v1 + v2,
            v1 + v3,
            v1 + v4,
            v5,
            v5 + v6,
            v5 + v7,
            v5 + v8,
            v9,
            v9 + v10,
            v9 + v11,
            v9 + v12,
            v13,
            v13 + v14,
            v13 + v15,
            v13 + v16,

        };
        mesh.vertices = vertices;
        int[] indexes = new int[]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1,
            4, 6, 5,
            6, 7, 5,
            8, 10, 9,
            10, 11, 9,
            12, 14, 13,
            14, 15, 13,

        };

        mesh.triangles = indexes;
        FlipMesh(mesh);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        var center = mesh.bounds.center;
        boxCollider.center = center;
        boxCollider.size = new Vector3(CoordinateSystem.TILE_GRID_SIZE, 0.1f, CoordinateSystem.TILE_GRID_SIZE);

        collider = boxCollider;



        UpdateStatus();
    }

    public void UpdateStatus()
    {
        Color color = Color.gray;
        switch (status)
        {
            case TileStatus.Special:
                color = Color.yellow;
                break;
            default:
                break;
        }

        switch (selection)
        {
            case TileSelection.Selected:
                color = Color.green;
                break;

            default:
                break;
        }

        switch (hover)
        {
            case TileHover.Hovered:
                color = Color.cyan;
                break;
            default:
                break;
        }
        material.color = color;
    }

    public void Draw(Camera camera)
    {
        Graphics.DrawMesh(mesh, Matrix4x4.identity, material, 0, camera, 0, null, false, false);
    }

    public void DrawWalkableInfo(Camera camera)
    {
        for (int y = 0; y < DT1Block.SUBTILES_Y; ++y)
        {
            for (int x = 0; x < DT1Block.SUBTILES_X; ++x)
            {
                var index = y * DT1Block.SUBTILES_Y + x;
                Graphics.DrawMesh(walkableInfo[index], Matrix4x4.identity, walkableMaterials[index], 0, camera, 0, null, false, false);
            }
        }
    }

    public bool Raycast(Ray ray, out RaycastHit hit, float distance)
    {
        return collider.Raycast(ray, out hit, distance);
    }
}