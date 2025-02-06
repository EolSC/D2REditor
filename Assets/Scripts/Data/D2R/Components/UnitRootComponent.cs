using Diablo2Editor;
using SimpleJSON;
using System.Collections.Generic;

public class UnitRootComponent : LevelEntityComponent
{
    public string state_machine_filename;
    public bool doNotInheritRotation;
    public UnityEngine.Quaternion rotationOverride;
    public bool doNotUseHDHeight;
    public bool hideAllMeshWhenInOpenedMode;
    public string onCreateEventName;
    public override void Deserialize(JSONObject json)
    {
        base.Deserialize(json);
        state_machine_filename = json["state_machine_filename"];
        doNotInheritRotation = json["doNotInheritRotation"];

        JSONObject rotationOverride = json["rotationOverride"].AsObject;
        this.rotationOverride.x = ISerializable.DeserializeFloat(rotationOverride["x"]);
        this.rotationOverride.y = ISerializable.DeserializeFloat(rotationOverride["y"]);
        this.rotationOverride.z = ISerializable.DeserializeFloat(rotationOverride["z"]);
        this.rotationOverride.w = ISerializable.DeserializeFloat(rotationOverride["w"]);

        doNotUseHDHeight = json["doNotUseHDHeight"];
        hideAllMeshWhenInOpenedMode = json["hideAllMeshWhenInOpenedMode"];
        onCreateEventName = json["onCreateEventName"];
    }

    public override JSONObject Serialize()
    {
        JSONObject result = base.Serialize();
        
        result["state_machine_filename"] = state_machine_filename;
        result["doNotInheritRotation"] = doNotInheritRotation;

        JSONObject rotationOverride = new JSONObject();
        rotationOverride["x"] = ISerializable.SerializeFloat(this.rotationOverride.x);
        rotationOverride["y"] = ISerializable.SerializeFloat(this.rotationOverride.y);
        rotationOverride["z"] = ISerializable.SerializeFloat(this.rotationOverride.z);
        rotationOverride["w"] = ISerializable.SerializeFloat(this.rotationOverride.w);
        result["rotationOverride"] = rotationOverride;

        result["doNotUseHDHeight"] = doNotUseHDHeight;
        result["hideAllMeshWhenInOpenedMode"] = hideAllMeshWhenInOpenedMode;
        result["onCreateEventName"] = onCreateEventName;

        JSONArray animArray = new JSONArray();
        // TODO - implement animations
        result["animations"] = animArray;


        return result;
    }
}
