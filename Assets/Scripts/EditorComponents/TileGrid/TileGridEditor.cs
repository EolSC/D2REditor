using Diablo2Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileGrid))]
public class TileGridEditor : Editor
{
    private TileGrid grid;
    bool[] wall_folders = new bool[DS1Consts.WALL_MAX_LAYER];
    bool[] floor_folders = new bool[DS1Consts.FLOOR_MAX_LAYER];
    bool shadow_folder = false;
    bool tagged_folder = false;
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
        if (grid != null && (grid.levelComponent != null))
        {
            Tile selected = grid.GetSelectedTile();
            if (selected != null)
            {
                DrawTileInspector(selected);
            }
        }
    }

    private void DrawTileInspector(Tile selected)
    {
        var level = grid.levelComponent.GetDS1Level();

        int x = selected.x;
        int y = selected.y;
        bool needUpdate = false;
        GUIStyle labelStyle = new();
        GUIStyle areaStyle = new();

        labelStyle.padding = new RectOffset(0, 0, 4, 4);
        labelStyle.normal.textColor = Color.white;
        areaStyle.normal.textColor = Color.white;

        for (int n = 0; n < level.wall.layers; n++)
        {
            DS1WallTile cell = level.wall.data[n, y, x];

            if (cell != null)
            {
                wall_folders[n] = EditorGUILayout.Foldout(wall_folders[n], "Wall tile " + n);
                if (wall_folders[n])
                {
                    needUpdate |= DrawWallTile(cell, labelStyle, areaStyle);

                }
            }
        }

        for (int n = 0; n < level.floor.layers; n++)
        {
            DS1FloorTile cell = level.floor.data[n, y, x];

            if (cell != null)
            {
                floor_folders[n] = EditorGUILayout.Foldout(floor_folders[n], "Floor tile " + n);
                if (floor_folders[n])
                {
                    needUpdate |= DrawDS1Block(cell, labelStyle, areaStyle);

                }
            }
        }

        for (int n = 0; n < level.shadow.layers; n++)
        {
            DS1ShadowTile cell = level.shadow.data[n, y, x];

            if (cell != null)
            {
                shadow_folder = EditorGUILayout.Foldout(shadow_folder, "Shadow tile " + n);
                if (shadow_folder)
                {
                    needUpdate |= DrawDS1Block(cell, labelStyle, areaStyle);
                }
            }
        }

        for (int n = 0; n < level.tagged.layers; n++)
        {
            DS1TaggedTile cell = level.tagged.data[n, y, x];

            if (cell != null)
            {
                tagged_folder = EditorGUILayout.Foldout(tagged_folder, "Tagged tile " + n);
                if (shadow_folder)
                {
                    needUpdate |= DrawTaggedTile(cell, labelStyle, areaStyle);
                }
            }
        }

        if (needUpdate)
        {
            UpdateTile(level, selected);
        }
    }

    private bool DrawDS1Block(DS1Tile cell, GUIStyle labelStyle, GUIStyle areaStyle)
    {
        bool result = DrawProperties(cell, labelStyle, areaStyle);
        result |= DrawFlags(cell, labelStyle, areaStyle);
        return result;
    }

    private bool DrawWallTile(DS1WallTile cell, GUIStyle labelStyle, GUIStyle areaStyle)
    {
        bool result = DrawProperties(cell, labelStyle, areaStyle);

        GUILayout.BeginVertical(areaStyle);
        result |= DrawByteAttribute("Orientation: ", ref cell.orientation, labelStyle);
        GUILayout.EndVertical();

        result |= DrawFlags(cell, labelStyle, areaStyle);

        GUILayout.BeginVertical(areaStyle);
        bool isSpecial = cell.IsSpecial();
        GUILayout.Label("Is special: " + isSpecial, labelStyle);
        GUILayout.EndVertical();
        return result;
    }

    private bool DrawTaggedTile(DS1TaggedTile cell, GUIStyle labelStyle, GUIStyle areaStyle)
    {
        bool result = false;


        GUILayout.BeginVertical("Cell properties", areaStyle);
        GUILayout.Space(20);
        result |= DrawUintAttribute("Number: ", ref cell.num, labelStyle);
        GUILayout.EndVertical();

        return result;
    }

    private void UpdateTile(DS1Level level, Tile tile)
    {
        int x = tile.x;
        int y = tile.y;
        var walkInfo = level.walkableInfo;
        walkInfo.UpdateWalkableInfo(x, y);
        var walkData = walkInfo.GetWalkableData(x, y);
        tile.UpdateWalkableInfo(walkData.walkable);

        bool isSpecial = level.HasSpecialTiles(x, y);
        if (isSpecial)
        {
            tile.SetStatus(TileStatus.Special);
        }
        else
        {
            tile.SetStatus(TileStatus.Empty);
        }
    }
    private bool DrawProperties(DS1Tile cell, GUIStyle labelStyle, GUIStyle areaStyle)
    {
        bool result = false;
        if (cell.bt_idx != -1)
        {
            var level = grid.levelComponent.GetDS1Level();
            GUIStyle previewStyle = GUI.skin.textArea;
            var bitmaps = level.block_table[cell.bt_idx].tileData.bitmaps;
            var previewIndex = level.block_table[cell.bt_idx].block_idx;
            var preview = bitmaps[previewIndex];
            GUILayout.Box(preview, previewStyle);
        }

        GUILayout.BeginVertical("Cell properties", areaStyle);
        GUILayout.Space(20);
        result |= DrawByteAttribute("Priority: ", ref cell.prop1, labelStyle);
        result |= DrawByteAttribute("Sub index:", ref cell.prop2, labelStyle);
        int mainIndex = cell.GetMainIndex();
        if (DrawIntAttribute("Main index: ", ref mainIndex, labelStyle))
        {
            cell.SetMainIndex(mainIndex);
            result = true;
        }
        GUILayout.EndVertical();
        return result;
    }

    private bool DrawFlags(DS1Tile cell, GUIStyle labelStyle, GUIStyle areaStyle)
    {
        bool result = false;
        GUILayout.BeginVertical(areaStyle);
        bool hidden = cell.IsHidden();
        if (DrawBoolAttribute("Is hidden: ", ref hidden, labelStyle))
        {
            result = true;
            cell.SetHidden(hidden);
        }
        bool walkable = cell.IsWalkable();
        if (DrawBoolAttribute("Is walkable: ", ref walkable, labelStyle))
        {
            result = true;
            cell.SetWalkable(walkable);
        }
        GUILayout.EndVertical();

        return result;
    }

    private bool DrawByteAttribute(string name, ref byte attribute, GUIStyle labelStyle)
    {
        GUIStyle horizontalStyle = new()
        {
            padding = new RectOffset(0, 20, 0, 0)
        };

        GUIStyle textStyle = GUI.skin.textField;
        textStyle.fixedWidth = 100;
        textStyle.margin = new RectOffset(0, 0, 4, 4);

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

    private bool DrawUintAttribute(string name, ref uint attribute, GUIStyle labelStyle)
    {
        GUIStyle horizontalStyle = new()
        {
            padding = new RectOffset(0, 20, 0, 0)
        };

        GUIStyle textStyle = GUI.skin.textField;
        textStyle.fixedWidth = 100;

        string oldValue = attribute.ToString();
        GUILayout.BeginHorizontal(horizontalStyle);
        GUILayout.Label(name, labelStyle);
        string value = GUILayout.TextField(oldValue, 30, textStyle);
        GUILayout.EndHorizontal();
        if (value != oldValue)
        {
            return uint.TryParse(value, out attribute);
        }
        return false;
    }

    private bool DrawIntAttribute(string name, ref int attribute, GUIStyle labelStyle)
    {
        GUIStyle horizontalStyle = new()
        {
            padding = new RectOffset(0, 20, 0, 0)
        };

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
}

