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
        public long main_index = -1;
        public long orientation = -1;
        public long sub_index = -1;

        // datas
        public DT1Data tileData;
        public int dt1_idx;
        public long rarity = -1;
        public int block_idx = -1;
        public BlockType type = BlockType.BT_NULL;
        public int zero_line = -1;
        public int roof_y = -1;

        // conflicts managment
        public char conflict;
        public bool used_by_game = false;   // True / False
        public bool used_by_editor = false; // True / False

        // animated tile
        public long curr_frame = -1;
        public int updated = -1;

        public int CompareTo(DS1BlockTable other)
        {
            int n1 = 0;
            int n2 = 0;
            if (this.main_index != other.main_index)
            {
                n1 = (int)main_index;
                n2 = (int)other.main_index;
            }
            else if (this.orientation != other.orientation)
            {
                n1 = (int)orientation;
                n2 = (int)other.orientation;
            }
            else if (this.sub_index != other.sub_index)
            {
                n1 = (int)this.sub_index;
                n2 = (int)other.sub_index;
            }
            else if (dt1_idx != other.dt1_idx)
            {
                n1 = dt1_idx;
                n2 = other.dt1_idx;
            }
            else if (this.rarity != other.rarity)
            {
                n1 = (int)this.rarity;
                n2 = (int)other.rarity;
            }
            else if (this.block_idx != other.block_idx)
            {
                n1 = this.block_idx;
                n2 = other.block_idx;
            }
            return n1 - n2;
        }
    }
}
