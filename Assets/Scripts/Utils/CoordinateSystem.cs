using UnityEngine;

public class CoordinateSystem
{
    // Distance between cells in Unity units
    public static float TILE_GRID_SIZE = 10.0f;
    public static float SUBTILE_SIZE_X = TILE_GRID_SIZE / DT1Block.SUBTILES_X;
    public static float SUBTILE_SIZE_Y = TILE_GRID_SIZE / DT1Block.SUBTILES_Y;

    // Distance between cells in Unity units
    public static Vector3 TILE_GRID_STEP = new Vector3(10.0f, 0.0f, 10.0f);

    // Distance between cells in Unity units
    public static Vector3 SUBTILE_GRID_STEP = new Vector3(SUBTILE_SIZE_X, 0.15f, SUBTILE_SIZE_Y);

}
