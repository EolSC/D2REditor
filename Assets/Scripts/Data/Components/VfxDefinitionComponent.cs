using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    /*
    * VfxDefinitionComponent component. Not rendered in unity but can be edited.
    * 
    */
    public class VfxDefinitionComponent : LevelEntityComponent
    {
        public string filename;
        public bool hardKillOnDestroy = false;
        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            filename = json["filename"];
            hardKillOnDestroy = json["hardKillOnDestroy"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["filename"] = filename;
            result["hardKillOnDestroy"] = hardKillOnDestroy;

            return result;
        }

    }
}