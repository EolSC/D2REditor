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
        public int dt1_idx_for_ds1;
        public long main_index;
        public long orientation;
        public long sub_index;

        // datas
        public int dt1_idx;
        public long rarity;
        public int block_idx;
        public BlockType type;
        public int zero_line;
        public int roof_y;

        // conflicts managment
        public char conflict;
        public char used_by_game;   // True / False
        public char used_by_editor; // True / False

        // animated tile
        public long curr_frame;
        public int updated;
    }
}
