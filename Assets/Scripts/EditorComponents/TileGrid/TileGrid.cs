using Diablo2Editor;
using UnityEngine;


[ExecuteInEditMode]
public class TileGrid : MonoBehaviour
{
    [SerializeField]
    public bool drawWalkableInfo = false;

    [SerializeField]
    public bool visible = true;

    [SerializeField]
    public int newX;

    [SerializeField]
    public int newY;

    [SerializeField]
    private LevelComponent _levelComponent;

    private LevelComponent _oldValue;

    private bool _gridDirty = false;

    private Tile copy;


    public LevelComponent levelComponent { 
        get
        { 
            return _levelComponent; 
        }
        set
        {
            this._levelComponent = value;
            UpdateLevelData();
        }
    }

    Tile[][] tiles;
    Tile selected = null;

    public Tile GetSelectedTile()
    {
        return selected;
    }

    public void CopyTileAt(int x, int y)
    {
        if (x >= 0 && x <= tiles.Length)
        {
            if (y >= 0 && y <= tiles[x].Length)
            {
                copy = tiles[x][y];
            }
        }
    }

    public void PasteTileAt(int x, int y)
    {
        var level = levelComponent.GetDS1Level();
        if (level != null)
        {
            level.ReplaceTile(copy.x, copy.y, y, x);
        }
    }
    void OnDisable()
    {
        UpdateCameraCallback(false);
    }

    private void OnValidate()
    {
        if (_oldValue != _levelComponent)
        {
            _gridDirty = true;
        }
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
        if (!visible)
        {
            return;
        }

        if (_gridDirty)
        {
            UpdateLevelData();
        }

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
        _oldValue = _levelComponent;
        _gridDirty = false;
    }

    public void Resize(int newX, int newY)
    {
        var level = levelComponent.GetDS1Level();
        if (level != null)
        {
            int oldX = (int)level.width;
            int oldY = (int)level.height;
            if (oldX == newX && oldY == newY)
            {
                return;
            }
            level.Resize(newX, newY);
            UpdateTileGrid();
        }
    }

    private void UpdateTileGrid()
    {
        tiles = null;
        if (_levelComponent != null)
        {
            var level = _levelComponent.GetDS1Level();
            if ((level.width > 0) && (level.height > 0))
            {
                newX = (int)level.width;
                newY = (int)level.height;

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

