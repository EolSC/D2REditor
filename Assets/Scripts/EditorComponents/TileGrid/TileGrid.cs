using Diablo2Editor;
using UnityEngine;

[ExecuteInEditMode]
public class TileGrid : MonoBehaviour
{
    [SerializeField]
    public bool drawWalkableInfo = false;

    private LevelComponent mLevelComponent;

    [SerializeField]
    public LevelComponent levelComponent { 
        get
        { 
            return mLevelComponent; 
        }
        set
        {
            this.mLevelComponent = value;
            UpdateLevelData();
        }
    }

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

    public void UpdateLevelData()
    {
        UpdateTileGrid();
    }

    private void UpdateTileGrid()
    {
        var level = levelComponent.GetDS1Level();
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
                    tile.x = x;
                    tile.y = y;

                    if (level.HasSpecialTiles(x, y))
                    {
                        tile.SetStatus(TileStatus.Special);
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

