using UnityEngine;
using SimpleJSON;

namespace Diablo2Editor
{
    public class TileBiomeOverrides : ISerializable
    {
        public void Deserialize(JSONObject json)
        {

        }


        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();
            return result;
        }
    }
}