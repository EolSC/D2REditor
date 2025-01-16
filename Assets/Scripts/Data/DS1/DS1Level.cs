using System.Collections.Generic;
using UnityEngine;


/*
 
typedef struct DS1_S
{
   char          dt1_idx[DT1_IN_DS1_MAX];
   int           dt1_mask[DT1_IN_DS1_MAX];
   int           txt_act;
   BLOCK_TABLE_S * block_table;
   int           bt_num;
   UBYTE         wall_layer_mask[WALL_MAX_LAYER];
   UBYTE         floor_layer_mask[FLOOR_MAX_LAYER];
   char          shadow_layer_mask[SHADOW_MAX_LAYER];
   OL_ENUM       objects_layer_mask;
   UBYTE         paths_layer_mask;
   UBYTE         walkable_layer_mask;
   UBYTE         animations_layer_mask;
   UBYTE         special_layer_mask;
   int           subtile_help_display;
   char          name[256];    // long filename with paths
   char          filename[50]; // short filename
   UNDO_S        undo;

   // from file
   long          version;
   long          tag_type;
   long          width;    // from file, +1
   long          height;   // from file, +1
   long          act;      // from file, +1

   // files in the ds1 (not used by the game)
   long          file_num;
   char          * file_buff;
   int           file_len;
   
   // floors
   CELL_F_S      * floor_buff,   // buffer for all floor layers
                 * floor_buff2;  // 2nd buffer, for copy/paste
   int           floor_buff_len; // sizeof the floor buffer (in bytes)
   int           floor_num;      // # of layers in floor buffer
   int           floor_line;     // width * floor_num
   int           floor_len;      // floor_line * height
   
   // shadow
   CELL_S_S      * shadow_buff,   // buffer for all shadow layers
                 * shadow_buff2;  // 2nd buffer, for copy/paste
   int           shadow_buff_len; // sizeof the shadow buffer (in bytes)
   int           shadow_num;      // # of layers in shadow buffer
   int           shadow_line;     // width * shadow_num
   int           shadow_len;      // shadow_line * height
   
   // walls
   CELL_W_S      * wall_buff,    // buffer for all wall layers
                 * wall_buff2;   // 2nd buffer, for copy/paste
   int           wall_buff_len;  // sizeof the wall buffer (in bytes)
   int           wall_num;       // # of layers in wall buffer
   int           wall_line;      // width * wall_num
   int           wall_len;       // wall_line * height

   // tag
   CELL_T_S      * tag_buff,   // buffer for all unk layers
                 * tag_buff2;  // 2nd buffer, for copy/paste
   int           tag_buff_len; // sizeof the unk buffer (in bytes)
   int           tag_num;      // # of layers in unk buffer
   int           tag_line;     // width * unk_num
   int           tag_len;      // unk_line * height

   // groups for tag layer
   long          group_num;
   int           group_size;
   GROUP_S       * group;

   // internal
   ZOOM_E        cur_zoom;
   int           tile_w;
   int           tile_h;
   int           height_mul;
   int           height_div;
   SCROLL_S      cur_scroll;

   // screen position and size for this ds1
   WIN_PREVIEW_S own_wpreview;
   
   // objects and npc paths (paths are in obj struct)
   int           * drawing_order;
   OBJ_S         * obj;
   OBJ_S         * obj_undo;
   long          obj_num;
   long          obj_num_undo;
   int           can_undo_obj;
   int           draw_edit_obj; // edit Type-Id of objects, FALSE / TRUE
   WIN_EDT_OBJ_S win_edt_obj;

   // current animated floor frame
   int           cur_anim_floor_frame;

   // path editing window of this ds1
   PATH_EDIT_WIN_S path_edit_win;

   // save count
   UDWORD save_count;

   // current number of objects
   long current_obj_max;
} DS1_S;

extern DS1_S * glb_ds1;
*/

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

        public char [] dt1_idx = new char[DS1Consts.DT1_IN_DS1_MAX];
        public int [] dt1_mask = new int[DS1Consts.DT1_IN_DS1_MAX];
        public int txt_act;
        public List<DS1BlockTable> block_table = new List<DS1BlockTable>();
        public int bt_num;
        public byte[] wall_layer_mask = new byte[DS1Consts.WALL_MAX_LAYER];
        public byte [] floor_layer_mask = new byte[DS1Consts.FLOOR_MAX_LAYER];
        public char [] shadow_layer_mask = new char[DS1Consts.SHADOW_MAX_LAYER];
        public DS1ObjectLayer objects_layer_mask;
        public byte paths_layer_mask;
        public byte walkable_layer_mask;
        public byte animations_layer_mask;
        public byte special_layer_mask;
        public int subtile_help_display;
        public string name;    // long filename with paths
        public string filename; // short filename
        //UNDO_S undo;

        // from file
        public long version;
        public long tag_type;
        public long width;    // from file, +1
        public long height;   // from file, +1
        public long act;      // from file, +1

        // files in the ds1 (not used by the game)
        public long file_num;
        public char[] file_buff;
        public int file_len;

        public DS1Floor floor;
        public DS1Shadow shadow;



    }
}