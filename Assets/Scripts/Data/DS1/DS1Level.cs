using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/* Internal data of DS1 level. 
 * 
 */

namespace Diablo2Editor
{
    public class  DS1Level
    {
        public DS1Level()
        {

        }
        public byte[] test_data; // source DS1 data for tests. TODO - delete after Save/Load is implemented

        public char [] dt1_idx = new char[DS1Consts.DT1_IN_DS1_MAX];
        public int [] dt1_mask = new int[DS1Consts.DT1_IN_DS1_MAX];
        public int txt_act;
        public List<DS1BlockTable> block_table = new List<DS1BlockTable>();
        public List<DT1Data> textureBanks;
        public int bt_num;
        public byte[] wall_layer_mask = new byte[DS1Consts.WALL_MAX_LAYER];
        public byte [] floor_layer_mask = new byte[DS1Consts.FLOOR_MAX_LAYER];
        public byte [] shadow_layer_mask = new byte[DS1Consts.SHADOW_MAX_LAYER];
        public DS1ObjectLayer objects_layer_mask;
        public byte paths_layer_mask;
        public byte walkable_layer_mask;
        public byte animations_layer_mask;
        public byte special_layer_mask;
        public int subtile_help_display;

        // from file
        public long version;
        public long tag_type;
        public long width;    // from file, +1
        public long height;   // from file, +1
        public long act;      // from file, +1

        // files in the ds1 (not used by the game)
        public long file_num;
        public List<string> files = new List<string>();

        // tiles data
        public DS1Floor floor = new DS1Floor();     // floor
        public DS1Shadow shadow = new DS1Shadow();  // shadow
        public DS1Wall wall = new DS1Wall();        // wall
        public DS1Tagged tagged = new DS1Tagged();  // tagged

        // groupData
        public long group_num;
        public int group_size;
        public List<DS1Group> group = new List<DS1Group>();

        public int tile_w;
        public int tile_h;

        /*
         Internal data. TODO - clean this up if not needed
        // internal
        ZOOM_E cur_zoom;
        int tile_w;
        int tile_h;
        int height_mul;
        int height_div;
        SCROLL_S cur_scroll;

        // screen position and size for this ds1
        WIN_PREVIEW_S own_wpreview;
        */


        // Objects data 
        public int[] drawing_order;
        public List<DS1Object> objects = new List<DS1Object>();
        public long obj_num;

        // current animated floor frame
        public int cur_anim_floor_frame;

        public void InitBlockTable(List<DT1Data> tileData)
        {
            textureBanks = tileData;
        }

        public void Intantiate(GameObject gameObject)
        {
            var textureBank = textureBanks[0];
            var texture = textureBank.bitmaps[0];
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
                    material.SetTexture("_MainTex", texture);
                }
                meshRenderer.material = material;
            }
            


        }
    }
}