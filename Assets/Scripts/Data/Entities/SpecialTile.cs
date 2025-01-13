using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class SpecialTile : ISerializable
    {
        public override void Deserialize(JSONObject json)
        {

        }


        public override JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            return result;
        }
    }
}