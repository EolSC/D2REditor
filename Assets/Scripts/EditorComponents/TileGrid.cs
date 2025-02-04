using Diablo2Editor;
using UnityEditor;
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
    // Distance between cells in Unity units
    static float TILE_GRID_SIZE = 10.0f;
    static float SUBTILE_SIZE_X = TILE_GRID_SIZE / DT1Block.SUBTILES_X;
    static float SUBTILE_SIZE_Y = TILE_GRID_SIZE / DT1Block.SUBTILES_Y;

    // Distance between cells in Unity units
    static Vector3 TILE_GRID_STEP = new Vector3(10.0f, 0.0f, 10.0f);

    static float LINE_THICKNESS = 1.0f;

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
        switch(walkable)
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
                var position = tile_world_pos + new Vector3(x * SUBTILE_SIZE_X, 0.0f, y * SUBTILE_SIZE_Y);

                Vector3 v1 = position + new Vector3(0.0f, 0.15f, 0.0f);
                Vector3 v2 = new Vector3(SUBTILE_SIZE_X, 0.0f, 0.0f);
                Vector3 v3 = new Vector3(0.0f, 0.0f, SUBTILE_SIZE_Y);
                Vector3 v4 = new Vector3(SUBTILE_SIZE_X, 0.0f, SUBTILE_SIZE_Y);


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
        Vector3 world_pos = new Vector3(x * TILE_GRID_STEP.x, y * TILE_GRID_STEP.y + 0.1f, z * TILE_GRID_STEP.z);
        CreateWalkableInfo(world_pos);

        mesh = new Mesh();
        material = new Material(Shader.Find("Standard"));
        material.SetFloat("_Glossiness", 0.0f);
        Vector3 v1 = world_pos;
        Vector3 v2 = new Vector3(TILE_GRID_SIZE, 0.0f, 0.0f);
        Vector3 v3 = new Vector3(0.0f, 0.0f, LINE_THICKNESS);
        Vector3 v4 = new Vector3(TILE_GRID_SIZE, 0.0f, LINE_THICKNESS);


        Vector3 v5 = v1 + v2;

        Vector3 v6 = new Vector3(0.0f, 0.0f, TILE_GRID_SIZE);
        Vector3 v7 = new Vector3(-LINE_THICKNESS, 0.0f, 0.0f);
        Vector3 v8 = new Vector3(-LINE_THICKNESS, 0.0f, TILE_GRID_SIZE);

        Vector3 v9 = v1 + new Vector3(LINE_THICKNESS, 0.0f, 0.0f);

        Vector3 v10 = v6;
        Vector3 v11 = v7;
        Vector3 v12 = v8;

        Vector3 v13 = v1 + new Vector3(0.0f, 0.0f, TILE_GRID_SIZE - LINE_THICKNESS);

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
        boxCollider.size = new Vector3(TILE_GRID_SIZE, 0.1f, TILE_GRID_SIZE);

        collider = boxCollider;



        UpdateStatus();
    }

    public void UpdateStatus()
    {
        Color color = Color.gray;
        switch(status)
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

        switch(hover)
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
        Graphics.DrawMesh(mesh, Matrix4x4.identity, material, 0, camera, 0 , null, false, false);
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

[ExecuteInEditMode]
public class TileGrid : MonoBehaviour
{
    [SerializeField]
    public bool drawWalkableInfo = true;

    public DS1Level level;
    Tile[][] tiles;
    Tile selected = null;

    public Tile GetSelectedTile()
    {
        return selected;
    }

    void OnDisable()
    {
        UpdateCameraCallback(false);
    }

    private void OnEnable()
    {
        UpdateCameraCallback(true);
    }

    public bool Raycast(Ray ray, bool mouseUp)
    {
        if (tiles == null)
        {
            return false;
        }

        for (int z = 0; z < tiles.Length; z++)
        {
            for (int x = 0; x < tiles[z].Length; x++)
            {
                var tile = tiles[z][x];
                bool hovered = tile.Raycast(ray, out _, Mathf.Infinity);
                if (hovered)
                {
                    if (mouseUp)
                    {
                        if (selected != null && tile != selected)
                        {
                            selected.SetSelection(TileSelection.None);
                        }
                        tile.SetSelection(TileSelection.Selected);
                        selected = tile;
                        return true;
                    }
                    else
                    {
                        tile.SetHover(TileHover.Hovered);
                    }
                }
                else
                {
                    tile.SetHover(TileHover.None);
                }
            }
        }
        return false;
    }

    private void Draw(Camera camera)
    {
        if (tiles  == null)
        {
            return;
        }

        for (int z = 0; z < tiles.Length; z++)
        {
            for (int x = 0; x < tiles[z].Length; x++)
            {
                var tile = tiles[z][x];
                tile.Draw(camera);
                if (drawWalkableInfo)
                {
                    tile.DrawWalkableInfo(camera);
                }
            }
        }
    }

    public void SetLevelData(DS1Level level)
    {
        this.level = level;

        UpdateTileGrid();
        this.enabled = true;
    }

    private void UpdateTileGrid()
    {
        if ((level.width > 0) && (level.height > 0))
        {
            tiles = new Tile[level.height][];


            for (int y = 0; y < level.height; y++)
            {
                tiles[y] = new Tile[level.width];
                for (int x = 0; x < level.width; x++)
                {
                    var tile = new Tile();
                    var collider = gameObject.AddComponent<BoxCollider>();
                    collider.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                    tile.Create(collider, x, 0, y);
                    tiles[y][x] = tile;
                    var walkableData = level.walkableInfo.GetWalkableData(x, y);

                    tile.UpdateWalkableInfo(walkableData.walkable);
                    if (level.wall.wall_num > 0)
                    {
;                       for ( int i = 0; i < level.wall.wall_num; i++)
                        {
                            var wallTile = level.wall.wall_array[i, y, x];
                            tile.x = x;
                            tile.y = y;

                            if (wallTile.IsSpecial())
                            {
                                tile.SetStatus(TileStatus.Special);
                            }
                        }

                    }

                }

            }
        }
    }

    private void UpdateCameraCallback(bool isEnabled)
    {
        if (UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline == null) // built-in
        {
            if (isEnabled)
            {
                Camera.onPreCull += Draw;
            }
            else
            {
                Camera.onPreCull -= Draw;

            }
        }
    }
}

[CustomEditor(typeof(TileGrid))]
public class TileGridEditor : Editor
{
    private TileGrid grid;

    void OnEnable()
    {
        grid = (TileGrid)target;
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }

    void OnSceneGUI()
    {
        if (grid != null)
        {
            var e = Event.current;
            bool mouseUp = e.isMouse && e.button == 0 && e.type == EventType.MouseUp;

            UnityEngine.Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (grid.Raycast(ray, mouseUp))
            {
                Repaint();
            }
            // Keep grid selected while we are working with it
            // Helps with clicks through models
            Selection.activeObject = grid;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TileGrid grid = (TileGrid)target;
        if (grid != null)
        {
            Tile selected = grid.GetSelectedTile();
            DS1Level lvl = grid.level;
            if (selected != null)
            {

                DS1WallCell cell = lvl.wall.wall_array[0, selected.y, selected.x];
                GUIStyle labelStyle = new GUIStyle();
                GUIStyle areaStyle = new GUIStyle();

                labelStyle.padding = new RectOffset(0, 0, 4, 4);
                labelStyle.normal.textColor = Color.white;
                areaStyle.normal.textColor = Color.white;
                if (cell != null)
                {
                    if (cell.bt_idx != -1) 
                    {
                        GUIStyle previewStyle = GUI.skin.textArea;
                        Rect previewRect = new Rect(40, 20, 100, 100);
                        var bitmaps = grid.level.block_table[cell.bt_idx].tileData.bitmaps;
                        var previewIndex = grid.level.block_table[cell.bt_idx].block_idx;
                        var preview = bitmaps[previewIndex];
                        GUILayout.Box(preview, previewStyle);
                        


                    }

                    GUILayout.BeginVertical("Cell properties", areaStyle);
                    GUILayout.Space(20);
                    DrawByteAttribute("Priority: ", ref cell.prop1, labelStyle);
                    DrawByteAttribute("Sub index:", ref cell.prop2, labelStyle);
                    int mainIndex = cell.GetMainIndex();
                    if (DrawIntAttribute("Main index: ", ref mainIndex, labelStyle))
                    {
                        cell.SetMainIndex(mainIndex);
                    }


                    DrawByteAttribute("Orientation: ", ref cell.orientation, labelStyle);
                    GUILayout.EndVertical();
                    
                    GUILayout.BeginVertical("Special data", areaStyle);
                    GUILayout.Space(20);
                    bool hidden = cell.IsHidden();
                    if (DrawBoolAttribute("Is hidden: ", ref hidden, labelStyle))
                    {
                        cell.SetHidden(hidden);
                    }
                    bool isSpecial = cell.IsSpecial();
                    DrawBoolAttribute("Is special: ", ref isSpecial, labelStyle);
                    if (isSpecial)
                    {
                        selected.SetStatus(TileStatus.Special);
                    }
                    else
                    {
                        selected.SetStatus(TileStatus.Empty);

                    }
                    GUILayout.EndVertical();
                    DrawWalkableData(lvl, selected.x, selected.y);
                    
                }
            }
        }
    }
    private bool DrawByteAttribute(string name, ref byte attribute, GUIStyle labelStyle)
    {
        GUIStyle horizontalStyle = new GUIStyle();
        horizontalStyle.padding = new RectOffset(0, 20, 0, 0);

        GUIStyle textStyle = GUI.skin.textField;
        textStyle.fixedWidth = 100;

        string oldValue = attribute.ToString();
        GUILayout.BeginHorizontal(horizontalStyle);
        GUILayout.Label(name, labelStyle);
        string value = GUILayout.TextField(oldValue, 30, textStyle);
        GUILayout.EndHorizontal();
        if (value != oldValue)
        {
            return byte.TryParse(value, out attribute);
        }
        return false;
    }

    private bool DrawIntAttribute(string name, ref int attribute, GUIStyle labelStyle)
    {
        GUIStyle horizontalStyle = new GUIStyle();
        horizontalStyle.padding = new RectOffset(0, 20, 0, 0);

        GUIStyle textStyle = GUI.skin.textField;
        textStyle.fixedWidth = 100;

        string oldValue = attribute.ToString();
        GUILayout.BeginHorizontal(horizontalStyle);
        GUILayout.Label(name, labelStyle);
        string value = GUILayout.TextField(oldValue, 30, textStyle);
        GUILayout.EndHorizontal();
        if (value != oldValue)
        {
            return int.TryParse(value, out attribute);
        }
        return false;
    }

    private bool DrawBoolAttribute(string name, ref bool attribute, GUIStyle style)
    {
        bool oldValue = attribute;
        GUILayout.BeginHorizontal();
        GUILayout.Label(name, style);
        bool newValue = EditorGUILayout.Toggle(attribute);
        if (oldValue != newValue)
        {
            attribute = newValue;
            return true;
        }
        GUILayout.EndHorizontal();
        return false;
    }

    private void DrawWalkableData(DS1Level level, int x, int y)
    {
        var walkableInfo = level.walkableInfo.GetWalkableData(x, y);
        if (walkableInfo != null)
        {
            // TODO: draw some meaningful walkable data
            byte[] walkable = walkableInfo.walkable;

        }
    }

}
