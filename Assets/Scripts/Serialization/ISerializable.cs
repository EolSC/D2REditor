using SimpleJSON;
using System.Collections.Generic;

namespace Diablo2Editor
{

    public abstract class ISerializable
    {
        public abstract JSONObject Serialize();
        public abstract void Deserialize(JSONObject json);

        protected List<T> DeserializeList<T>(JSONObject json, string depType) where T : ISerializable, new()
        {
            List<T> result = new List<T>();
            foreach (JSONObject item in json[depType])
            {
                T serializable = new T();
                serializable.Deserialize(item);
                result.Add(serializable);
            }
            return result;
        }

        protected JSONArray SerializeList<T>(List<T> list) where T : ISerializable
        {
            JSONArray result = new JSONArray();
            foreach (T item in list)
            {
                JSONObject obj = item.Serialize();
                result.Add(obj);
            }
            return result;
        }

        protected float DeserializeFloat(JSONNode json)
        {
            return json.AsFloat;
        }

        protected float SerializeFloat(float value, string format = "{0:f}")
        {
            return value;
        }

    }
}