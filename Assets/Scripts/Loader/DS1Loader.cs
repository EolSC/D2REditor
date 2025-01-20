using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Diablo2Editor
{

    public class DS1Loader
    {
        private const bool PRINT_DEBUG = true;
        private const int BLOCK_SIZE = 4;  // number of bytes in one reading from file
        public DS1Loader()
        {

        }

        private void InitDS1LevelStruct(DS1Level level)
        {
            // layers mask
            for (int i = 0; i < DS1Consts.WALL_MAX_LAYER; i++)
                level.wall_layer_mask[i] = 1;

            for (int i = 0; i < DS1Consts.FLOOR_MAX_LAYER; i++)
                level.floor_layer_mask[i] = 1;
            for (int i = 0; i < DS1Consts.SHADOW_MAX_LAYER; i++)
                level.shadow_layer_mask[i] = 3;

            level.objects_layer_mask = DS1ObjectLayer.OL_NONE;
            level.paths_layer_mask = 1;
            level.walkable_layer_mask = 0;
            level.subtile_help_display = 1;
            level.animations_layer_mask = 1;
            level.special_layer_mask = 1;
        }

        private void PrintDebugHeaderData(DS1Level level)
        {
            Debug.Log("DS1 level version " + level.version);
            Debug.Log("DS1 level width " + level.width);
            Debug.Log("DS1 level height " + level.height);
            Debug.Log("DS1 level act " + level.act);
            Debug.Log("DS1 level tag type " + level.tag_type);
        }

        public DS1Level ReadDS1(byte[] content)
        {
            DS1Level level = new DS1Level();
            level.test_data = content;
            InitDS1LevelStruct(level);
            int w_num = 0; // # of wall & orientation layers
            int f_num = 0; // # of floor layer
            int s_num = 1; // # of shadow layer, always here
            int t_num = 0; // # of tag layer

            int bytes_counter = 0;
            level.version = BitConverter.ToUInt32(content, bytes_counter);
            bytes_counter++;
            level.width = BitConverter.ToUInt32(content, bytes_counter * BLOCK_SIZE) + 1;
            bytes_counter++;
            level.height = BitConverter.ToUInt32(content, bytes_counter * BLOCK_SIZE) + 1;
            bytes_counter++;
            level.act = 1;
            if (level.version >= 8)
            {
                level.act = BitConverter.ToUInt32(content, bytes_counter * BLOCK_SIZE) + 1;
                bytes_counter++;
                if (level.act > 5) // clamp act 1-5
                    level.act = 5;
            }
            // tag_type
            level.tag_type = 0;
            if (level.version >= 10)
            {
                level.tag_type = BitConverter.ToUInt32(content, bytes_counter * BLOCK_SIZE);
                bytes_counter++;

                // adjust eventually the # of tag layer
                if ((level.tag_type == 1) || (level.tag_type == 2))
                    t_num = 1;
            }
            if (PRINT_DEBUG)
            {
                PrintDebugHeaderData(level);
            }
            return level;
        }
    }
}
