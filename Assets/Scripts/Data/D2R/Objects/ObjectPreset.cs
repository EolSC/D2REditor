using Diablo2Editor;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
namespace Diablo2Editor
{
    public class ObjectPreset : JsonPreset
    {

        public ObjectPreset(GameObject gameObject)
            :base(gameObject)
        {
        }

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
        }
        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();

            return result;
        }
    }
}
