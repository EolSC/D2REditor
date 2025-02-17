using UnityEngine;
using SimpleJSON;
using NUnit.Framework;
using System.Collections.Generic;

namespace Diablo2Editor
{

    public class TileBiomeOverride : ISerializable
    {
        public string name;
        public string biomeFilename;
        public int tileX;
        public int tileY;
        public string type;

        public void Deserialize(JSONObject json)
        {
            biomeFilename = json["biomeFilename"];
            name = json["name"];
            tileX = json["tileX"];
            tileY = json["tileY"];
            type = json["type"];
        }


        public JSONObject Serialize()
        {
            JSONObject result = new JSONObject();

            result["biomeFilename"] = biomeFilename;
            result["name"] = name;
            result["tileX"] = tileX;
            result["tileY"] = tileY;
            result["type"] = type;

            return result;
        }
    }
}