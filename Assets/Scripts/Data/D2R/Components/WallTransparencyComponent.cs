using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * WallTransparencyComponent component. Not rendered in unity but can be edited.
    * 
    */
    public class WallTransparencyComponent : LevelEntityComponent
    {
        public int drawOrder = 0;
        public UnityEngine.Vector2Int wallTileLocalCoord;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            drawOrder = json["drawOrder"];
            JSONObject wall_tile_obj = json["wallTileLocalCoord"].AsObject;
            wallTileLocalCoord.x = wall_tile_obj["x"];
            wallTileLocalCoord.y = wall_tile_obj["y"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["drawOrder"] = drawOrder;

            JSONObject local_coord = new JSONObject();
            local_coord["x"] = wallTileLocalCoord.x;
            local_coord["y"] = wallTileLocalCoord.y;
            result["wallTileLocalCoord"] = local_coord;

            return result;
        }

    }
}