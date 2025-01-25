using Diablo2Editor;
using System.Buffers.Text;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DS1Drawer
{
    Texture2D target;
    int TARGET_WIDTH = 1024;
    int TARGET_HEIGHT = 768;


    public DS1Drawer()
    {
        target = new Texture2D(TARGET_WIDTH, TARGET_HEIGHT);
    }

    public void DrawTilesToTexture(DS1Level level)
    {
        // loop 1A : lower walls, floors, shadows of dt1
        for (int y = 0; y < level.height; y++)
        {
            int base_x = y * -level.tile_w / 2;
            int base_y = y * level.tile_h / 2;
            for (int x = 0; x < level.width; x++)
            {
                int mx = base_x + x * level.tile_w / 2;
                int my = base_y + x * level.tile_h / 2;
                DrawWalls(level, x, y, mx, my);      // lower walls
                DrawFloorTile(level, x, y, mx, my);     // floors
                DrawShadowTile(level, x, y, mx, my);           // shadows of dt1
                
            }
        }
    }

    public void DrawWalls(DS1Level level, int x, int y, int mx, int my)
    {
    }

    public void DrawFloorTile(DS1Level level, int x, int y, int mx, int my)
    {

    }
    public void DrawShadowTile(DS1Level level, int x, int y, int mx, int my)
    {

    }

    public void Intantiate(GameObject gameObject)
    {
        var subObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        subObject.transform.parent = gameObject.transform;
        subObject.transform.localScale = new Vector3(10, 10, 10);
        subObject.transform.position = new Vector3(120, 120, 120);
        var meshRenderer = subObject.GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            var material = new UnityEngine.Material(Shader.Find("Standard"));
            if (material)
            {
                material.SetTexture("_MainTex", target);
            }
            meshRenderer.material = material;
        }
    }

}
