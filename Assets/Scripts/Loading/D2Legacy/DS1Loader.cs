using System;
using System.Text;
using UnityEngine;

namespace Diablo2Editor
{
    public class DS1Loader
    {
        private const bool PRINT_DEBUG = true;
        private const bool ALWAYS_MAX_LAYERS = false;
        // length of 1 wall tile in .ds1 file(bytes). Used to jump to orient data for walls
        private const int WALL_TILE_DATA_SIZE = 4;

        // Lookup table for tile orientation
        static byte[] dir_lookup = {
                  0x00, 0x01, 0x02, 0x01, 0x02, 0x03, 0x03, 0x05, 0x05, 0x06,
                  0x06, 0x07, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E,
                  0x0F, 0x10, 0x11, 0x12, 0x14
               };

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

        private bool TryReadUint(byte[] content, ref int streamPosition, out uint value)
        {
            int dataSize = sizeof(uint);
            int dataEnd = streamPosition + dataSize;
            if (dataEnd < content.Length)
            {
                value = BitConverter.ToUInt32(content, streamPosition);
                streamPosition += dataSize;
                return true;
            }
            value = 0;
            return false;
        }

        private uint ReadUint(byte[] content, ref int streamPosition)
        {
            uint result;
            if (TryReadUint(content, ref streamPosition, out result))
             {
                return result;
            }
            else
            {
                Debug.Log("Error reading ds1: End of content.");
            }
            return 0;
        }

        private string ReadString(byte[] content, ref int streamPosition)
        {
            string result = "";
            // read in cycle until we got terminated
            bool eof;
            bool eol;
            int start_pos = streamPosition;
            do
            {
                byte nextChar = content[streamPosition++];
                eof = streamPosition == content.Length;
                eol = nextChar == '\0';
            } while (!eof && !eol);
            result = Encoding.ASCII.GetString(content, start_pos, streamPosition - start_pos - 1);
            return result;
        }

        // wall data and orientation data are located separately in ds1 file
        // so we need to know offset pointing for orientation data to load walls properly
        private DS1WallCell ReadWallCell(byte[] content, 
            ref int streamPosition, long version)
        {
           DS1WallCell tile = new DS1WallCell();
            tile.prop1 = content[streamPosition++];
            tile.prop2 = content[streamPosition++];
            tile.prop3 = content[streamPosition++];
            tile.prop4 = content[streamPosition++];
           return tile;
        }

        private DS1FloorCell ReadFloorCell(byte[] content, ref int streamPosition)
        {

            DS1FloorCell tile = new DS1FloorCell();
            tile.prop1 = content[streamPosition++];
            tile.prop2 = content[streamPosition++];
            tile.prop3 = content[streamPosition++];
            tile.prop4 = content[streamPosition++];
            return tile;
        }

        private DS1ShadowCell ReadShadowCell(byte[] content, ref int streamPosition)
        {

            DS1ShadowCell tile = new DS1ShadowCell();
            tile.prop1 = content[streamPosition++];
            tile.prop2 = content[streamPosition++];
            tile.prop3 = content[streamPosition++];
            tile.prop4 = content[streamPosition++];
            return tile;
        }
        private DS1TaggedCell ReadTaggedCell(byte[] content, ref int streamPosition)
        {

            DS1TaggedCell tile = new DS1TaggedCell();
            tile.num = BitConverter.ToUInt32(content, streamPosition);
            streamPosition += sizeof(uint);
            return tile;
        }

        private void ReadWallOrientation(DS1WallCell tile, byte[] content, int position, long version)
        {
            byte orient_data = content[position];

            if (version < 7)
            {
                tile.orientation = dir_lookup[orient_data];
            }
            else
            {
                tile.orientation = orient_data;
            }
        }

        public DS1Level ReadDS1(byte[] content, bool alwaysMaxLayers = false)
        {
            DS1Level level = new DS1Level();
            level.test_data = content;
            InitDS1LevelStruct(level);
            uint wallNumber;     // # of wall & orientation layers
            uint floorNumber;     // # of floor layer
            uint shadowNumber = 1; // # of shadow layer, always here
            uint tagNumber = 0; // # of tag layer

            // tile layers data
            int[] layerStreamingCache = new int[14];
            int numberOfLayers;

            // Current position in data buffer
            int streamPosition = 0;
            level.version = ReadUint(content, ref streamPosition);
            level.width = ReadUint(content, ref streamPosition) + 1;
            level.height = ReadUint(content, ref streamPosition) + 1;
            level.act = 1;
            if (level.version >= 8)
            {
                level.act = ReadUint(content, ref streamPosition) + 1;
                if (level.act > 5) // clamp act 1-5
                    level.act = 5;
            }
            // tag_type
            level.tag_type = 0;
            if (level.version >= 10)
            {
                level.tag_type = ReadUint(content, ref streamPosition);

                // adjust eventually the # of tag layer
                if ((level.tag_type == 1) || (level.tag_type == 2))
                    tagNumber = 1;
            }


            // filenames
            level.file_num = 0;
            if (level.version >= 3)
            {
                level.file_num = ReadUint(content, ref streamPosition);

                for (int i  = 0; i < level.file_num; i++)
                {
                    string fileName = ReadString(content, ref streamPosition);
                    level.files.Add(fileName);
                }
            }

            // skip 2 dwords ?
            if ((level.version >= 9) && (level.version <= 13))
            {
                streamPosition += sizeof(ulong) * 2;
            }
                

            // number of wall, floor and tag layers
            if (level.version >= 4)
            {
                // number of wall (and orientation) layers
                wallNumber = ReadUint(content, ref streamPosition);

                // number of floor layers
                if (level.version >= 16)
                {
                    floorNumber = ReadUint(content, ref streamPosition);
                }
                else
                    floorNumber = 1; // default # of floor layer
            }
            else // in version < 4
            {
                // # of layers hardcoded
                wallNumber = 1;
                floorNumber = 1;
                tagNumber = 1;
            }

            // which order ?
            if (level.version < 4)
            {
                layerStreamingCache[0] = 1; // wall 1
                layerStreamingCache[1] = 9; // floor 1
                layerStreamingCache[2] = 5; // orientation 1
                layerStreamingCache[3] = 12; // tag
                layerStreamingCache[4] = 11; // shadow
                numberOfLayers = 5;
            }
            else
            {
                numberOfLayers = 0;
                for (int i = 0; i < wallNumber; i++)
                {
                    layerStreamingCache[numberOfLayers++] = 1 + i;          // wall x
                    layerStreamingCache[numberOfLayers++] = 5 + i;   // orientation x
                }
                for (int i = 0; i < floorNumber; i++)
                {
                    layerStreamingCache[numberOfLayers++] = 9 + i; // floor x
                }
                if (shadowNumber > 0)
                {
                    layerStreamingCache[numberOfLayers++] = 11;    // shadow
                }
                if (tagNumber > 0)
                {
                    layerStreamingCache[numberOfLayers++] = 12;    // tag
                }
            }

            // layers num
            if (alwaysMaxLayers)
            {
                level.floor.floor_num = DS1Consts.FLOOR_MAX_LAYER;
                level.wall.wall_num = DS1Consts.WALL_MAX_LAYER;
            }
            else
            {
                level.floor.floor_num = floorNumber;
                level.wall.wall_num = wallNumber;

            }
            level.shadow.shadow_num = shadowNumber;
            level.tagged.tag_num = tagNumber;

            /*
             * Create data arrays for all tile types
             */
            level.floor.floor_array = new DS1FloorCell[floorNumber, level.height, level.width];
            level.wall.wall_array = new DS1WallCell[wallNumber, level.height, level.width];
            level.shadow.shadow_array = new DS1ShadowCell[shadowNumber, level.height, level.width];
            if (level.tagged.tag_num > 0)
            {
                level.tagged.tag_array = new DS1TaggedCell[tagNumber, level.height, level.width];

            }

            /* 
             DS1 initializes some buffers here
             Work with buffers is now obsolete. It goes like this:
            
               // floor buffer
               glb_ds1[ds1_idx].floor_line     = new_width * glb_ds1[ds1_idx].floor_num;
               glb_ds1[ds1_idx].floor_len      = glb_ds1[ds1_idx].floor_line * new_height;
               glb_ds1[ds1_idx].floor_buff_len = glb_ds1[ds1_idx].floor_len * sizeof(CELL_F_S);
               glb_ds1[ds1_idx].floor_buff     = (CELL_F_S *) malloc(glb_ds1[ds1_idx].floor_buff_len);
               if (glb_ds1[ds1_idx].floor_buff == NULL)
               {
                  free(ds1_buff);
                  sprintf(tmp, "not enough mem (%i bytes) for floor buffer\n",
                     glb_ds1[ds1_idx].floor_buff_len);
                  ds1edit_error(tmp);
               }
               memset(glb_ds1[ds1_idx].floor_buff, 0, glb_ds1[ds1_idx].floor_buff_len);
                ... // etc for other tile types
                
                We can skip this part 'cause data is stored in collections not with plain data chunks
            
            */

            /*
             * DS1 Tiles data stored like this(version >=4)
                WWWWWWW
                OOOOOOO
                WWWWWWW
                OOOOOOO
                ...
                FFFFFFF
                SSSSSSS
                TTTTTTT

                W - Wall tile
                O - orientation data for wall tile
                F - floor tile
                S - shadow tile
                T - tag tile

                Old versions(version < 4) have 5 layers exactly
                WWWWWWW
                FFFFFFF
                OOOOOOO
                TTTTTTT
                SSSSSSS
            */
            // cycle through layer map
            for (int n = 0; n < numberOfLayers; n++)
            {
                for (int y = 0; y < level.height; y++)
                {
                    for (int x = 0; x < level.width; x++)
                    {
                        switch(layerStreamingCache[n])
                        {
                            default:
                            case 1:  // walls
                            case 2:
                            case 3:
                            case 4:
                            {
                                int layerNumber = layerStreamingCache[n] - 1;
                                var tile = ReadWallCell(content, ref streamPosition, level.version);
                                level.wall.wall_array[layerNumber, y, x] = tile;
                            }; break;
                            case 5: // orientation
                            case 6:
                            case 7:
                            case 8:
                                {
                                    // Skip 4 bytes, this data is read in ReadWallCell
                                    int wallLayerNum = layerStreamingCache[n] - 5;
                                    var wallCell = level.wall.wall_array[wallLayerNum, y, x];
                                    ReadWallOrientation(wallCell, content, streamPosition, level.version);
                                    streamPosition += 4;
                                }; break;
                            case 9:
                            case 10:  // floor
                                {
                                    int layerNumber = layerStreamingCache[n] - 9;
                                    var tile = ReadFloorCell(content, ref streamPosition);
                                    level.floor.floor_array[layerNumber, y, x] = tile;
                                }; break;
                            case 11:
                                {
                                    int layerNumber = layerStreamingCache[n] - 11;
                                    var tile = ReadShadowCell(content, ref streamPosition);
                                    level.shadow.shadow_array[layerNumber, y, x] = tile;

                                }; break;
                            case 12:
                                {
                                    int layerNumber = layerStreamingCache[n] - 12;
                                    var tile = ReadTaggedCell(content, ref streamPosition);
                                    level.tagged.tag_array[layerNumber, y, x] = tile;
                                }; break;

                        }
                    }
                }
            }

            if (level.version >= 2)
            {
                level.obj_num = ReadUint(content, ref streamPosition);
            }
            else
            {
                // no objects on level
            }

            int max_subtile_width = (int)level.width * 5;
            int max_subtile_height = (int)level.height * 5;
            for (int n = 0; n < level.obj_num; n++)
            {
                DS1Object levelObject = new DS1Object();
                levelObject.type = ReadUint(content, ref streamPosition);
                levelObject.id = ReadUint(content, ref streamPosition);
                levelObject.x = ReadUint(content, ref streamPosition);
                levelObject.y = ReadUint(content, ref streamPosition);

                if (level.version > 5)
                {
                    // flags
                    levelObject.ds1_flags = ReadUint(content, ref streamPosition);
                }

                // integrity check (not done by the game I believe)
                if ((levelObject.x >= 0) && (levelObject.x < max_subtile_width) && (levelObject.y >= 0) && (levelObject.y < max_subtile_height))
                {
                    // some init for the paths of this object
                    levelObject.path_num = 0;
                    levelObject.desc_idx = -1;
                    levelObject.flags = 0;

                    levelObject.frame_delta = (byte)(UnityEngine.Random.Range(0, 255) % 256);

                    levelObject.label.rx = 0;
                    levelObject.label.ry = 0;
                    levelObject.label.w = 0;
                    levelObject.label.h = 0;
                    levelObject.label.flags = 0;
                    // TODO: fill object label
                    level.objects.Add(levelObject);
                }
                else
                {
                    Debug.Log("Level object with id" + levelObject.id + " is invalid. Skipping it");
                }
            }
            level.obj_num = level.objects.Count;

            /* groups for tag layer

               warning : in fact there can be less groups than expected
               like in data\global\tiles\act1\outdoors\trees.ds1, where the file
               stop right after the last tile_x group data, leaving the other
               datas unknown (tile_y, width, height), and npc paths unknown */

            if ((level.version >= 12) &&
                 ((level.tag_type == 1) || (level.tag_type == 2))
               )
            {
                // skip 1 dword ?
                if (level.version >= 18)
                {
                    streamPosition += sizeof(uint);
                }

                level.group_num = ReadUint(content, ref streamPosition);

                // fill it
                // For cases when group data is incomplete we 'll use safe way to read 
                // data(TryReadUint32)
                for (int i = 0; i < level.group_num; i++)
                {
                    DS1Group group = new DS1Group();
                    uint temp_value;
                    if (TryReadUint(content, ref streamPosition, out temp_value))
                    {
                        group.tile_x = temp_value;
                    }
                    if (TryReadUint(content, ref streamPosition, out temp_value))
                    {
                        group.tile_y = temp_value;
                    }
                    if (TryReadUint(content, ref streamPosition, out temp_value))
                    {
                        group.width = temp_value;
                    }
                    if (TryReadUint(content, ref streamPosition, out temp_value))
                    {
                        group.height = temp_value;
                    }

                    if (level.version >= 13)
                    {
                        if (TryReadUint(content, ref streamPosition, out temp_value))
                        {
                            group.unk = temp_value;
                        }
                    }
                }
            }

            if (level.version >= 14)
            {
                uint temp_value;

                // now we're on the npc's paths datas

                uint npc_paths_count;
                if (TryReadUint(content, ref streamPosition, out temp_value))
                {
                    npc_paths_count = temp_value;
                }
                else
                {
                    npc_paths_count = 0;
                }

                bool invalidPathDetected = false;
                for (int i = 0; i < npc_paths_count; i++)
                {
                    uint path_count = ReadUint(content, ref streamPosition);
                    uint x = ReadUint(content, ref streamPosition);
                    uint y = ReadUint(content, ref streamPosition);

                    // search of which object are these paths datas
                    int cur_obj = 0;
                    int last_obj = 0;
                    int number = 0;
                    bool done = false;
                    while (!done)
                    {
                        if (cur_obj < level.obj_num)
                        {
                            if ((level.objects[cur_obj].x == x) && (level.objects[cur_obj].y == y))
                            {
                                last_obj = cur_obj;
                                number++;
                                if (number >= 2)
                                    done = true;
                            }
                            cur_obj++; // next object
                        }
                        else
                            done = true;
                    }

                    if (number >= 2)
                    {
                        // there are a least 2 objects at the same coordinates

                        // put a warning
                        if (!invalidPathDetected)
                        {
                            invalidPathDetected = true;
                            Debug.Log("WARNING, there are at least 2 objects at the same coordinates for some paths datas");
                        }
                        Debug.Log("Removing " + path_count + " paths points of 1 object at coordinates (" + x + ", " + y + ")");

                        // first, delete already assigned paths
                        foreach (var levelObject in level.objects)
                        {

                            if ((levelObject.x == x) && (levelObject.y == y) &&
                                (levelObject.path_num != 0))
                            {
                                levelObject.paths.Clear();
                                levelObject.path_num = 0;
                            }
                        }

                        // now, skip these paths
                        if (level.version >= 15)
                        {
                            for (int p = 0; p < path_count; p++)
                            {
                                streamPosition += 3 * sizeof(uint); // skip 3 dwords per path
                            }

                        }
                        else
                        {
                            for (int p = 0; p < path_count; p++)
                            {
                                streamPosition += 2 * sizeof(uint); // skip 3 dwords per path
                            }
                        }
                    }
                    else
                    {
                        // only 1 object at these coordinates for paths, it's ok
                        cur_obj = last_obj;

                        // does these paths are pointing to a valid object position ?
                        if (cur_obj >= level.obj_num)
                        {
                            // nope
                            // the game don't alert the user, so why me ?
                            // but we'll skip them
                            if (level.version >= 15)
                            {
                                for (int p = 0; p < path_count; p++)
                                {
                                    streamPosition += 3 * sizeof(uint); // skip 3 dwords per path
                                }

                            }
                            else
                            {
                                for (int p = 0; p < path_count; p++)
                                {
                                    streamPosition += 2 * sizeof(uint); // skip 3 dwords per path
                                }
                            }
                        }
                        else
                        {
                            // yep, valid object

                            // all ok for assigning the paths to this object
                            var levelObject = level.objects[cur_obj];
                            levelObject.path_num = path_count;

                            for (int p = 0; p < levelObject.path_num; p++)
                            {
                                DS1ObjectPath pathObject = new DS1ObjectPath();
                                pathObject.x = ReadUint(content, ref streamPosition);
                                pathObject.y = ReadUint(content, ref streamPosition);
                                if (level.version >= 15)
                                {
                                    pathObject.action = ReadUint(content, ref streamPosition);
                                }
                                else
                                {
                                    pathObject.action = 1; // default action
                                }
                                levelObject.paths.Add(pathObject);
                            }
                        }
                    }
                }
                //TODO make object label
            }
            level.tile_w = 160;
            level.tile_h = 80;

            if (PRINT_DEBUG)
            {
                var debug_printer = new DS1LoaderDebugPrinter();
                debug_printer.PrintDebugInfo(level);
            }

            return level;
        }
    }
}
