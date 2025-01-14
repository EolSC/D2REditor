using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo2Editor
{

    public interface ISerializable
    {
        public abstract JSONObject Serialize();
        public abstract void Deserialize(JSONObject json);

        public static List<T> DeserializeList<T>(JSONObject json, string depType) where T : ISerializable, new()
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

        public static List<T> DeserializeComponentList<T>(JSONObject json, GameObject gameObject, string depType) where T : Component, ISerializable
        {
            List<T> result = new List<T>();
            foreach (JSONObject item in json[depType])
            {
                T serializable = gameObject.AddComponent<T>();
                serializable.Deserialize(item);
                result.Add(serializable);
            }
            return result;
        }

        public static JSONArray SerializeList<T>(List<T> list) where T : ISerializable
        {
            JSONArray result = new JSONArray();
            foreach (T item in list)
            {
                JSONObject obj = item.Serialize();
                result.Add(obj);
            }
            return result;
        }

        public static float DeserializeFloat(JSONNode json)
        {
            return json.AsFloat;
        }

        public static float SerializeFloat(float value, string format = "{0:f}")
        {
            return value;
        }

    }
}