using SimpleJSON;

namespace Diablo2Editor
{
    public class AudioEmitterComponent : LevelEntityComponent
    {
        /*
         *           "audioId": "object_flies_loop_hd",
              "randomDelay": {
                "x": 0,
                "y": 0
              },
              "forceNoRandom": false
        */
        string audioId;
        UnityEngine.Vector2 randomDelay;
        bool forceNoRandom = false;

        public override void Deserialize(JSONObject json)
        {
            base.Deserialize(json);
            audioId = json["audioId"];
            
            JSONObject position = json["randomDelay"].AsObject;
            this.randomDelay.x = ISerializable.DeserializeFloat(position["x"]);
            this.randomDelay.y = ISerializable.DeserializeFloat(position["y"]);

            forceNoRandom = json["forceNoRandom"];
        }

        public override JSONObject Serialize()
        {
            JSONObject result = base.Serialize();
            result["audioId"] = audioId;

            JSONObject randomDelayObject = new JSONObject();
            randomDelayObject["x"] = ISerializable.SerializeFloat(this.randomDelay.x);
            randomDelayObject["y"] = ISerializable.SerializeFloat(this.randomDelay.y);

            result["forceNoRandom"] = forceNoRandom;

            return result;
        }
    }

}
