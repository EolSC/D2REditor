using UnityEngine;
namespace Diablo2Editor
{
    public class DS1BlockTable
    {
        public enum BlockType
        {
            /* null    */
            BT_NULL,
            /* floors  */
            BT_STATIC, BT_ANIMATED,
            /* walls   */
            BT_WALL_UP, BT_WALL_DOWN, BT_ROOF, BT_SPECIAL, BT_WALL_ANIMATED,
            /* shadows */
            BT_SHADOW,
            BT_MAX
        }
        // key
        int dt1_idx_for_ds1;
        long main_index;
        long orientation;
        long sub_index;

        // datas
        int dt1_idx;
        long rarity;
        int block_idx;
        BlockType type;
        int zero_line;
        int roof_y;

        // conflicts managment
        char conflict;
        char used_by_game;   // True / False
        char used_by_editor; // True / False

        // animated tile
        long curr_frame;
        int updated;
    }
}
