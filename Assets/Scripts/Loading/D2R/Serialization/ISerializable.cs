using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Diablo2Editor
{
    /*
     * Basic serialzation via Simple JSON
     * Contains some utility functions to work with lists
     */
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

        public static List<Base> DeserializeList<T, Base>(JSONObject json, string depType) where T : ISerializable, Base, new()
        {
            List<Base> result = new List<Base>();
            if (json[depType] != null)
            {
                foreach (JSONObject item in json[depType])
                {
                    T serializable = new T();
                    serializable.Deserialize(item);
                    result.Add(serializable);
                }
            }
            return result;
        }

        /*
         * Unity components require gameObject to be properly instanced so we need separate function for
         * component list
         */
        public static List<T> DeserializeComponentList<T>(JSONObject json, LevelEntity entity, string depType) where T : LevelEntityComponent, ISerializable
        {
            List<T> result = new List<T>();
            GameObject gameObject = entity.gameObject;

            foreach (JSONObject item in json[depType])
            {
                T component = gameObject.AddComponent<T>();
                component.entity = entity;
                component.Deserialize(item);
                result.Add(component);
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

        public static Vector3 DeserializeVector(JSONObject obj)
        {   
            Vector3 vector3 = new Vector3();
            vector3.x = ISerializable.DeserializeFloat(obj["x"]);
            vector3.y = ISerializable.DeserializeFloat(obj["y"]);
            vector3.z = ISerializable.DeserializeFloat(obj["z"]);
            return vector3;
        }

        public static JSONObject SerializeVector(Vector3 vector)
        {
            JSONObject obj = new JSONObject();
            obj["x"] = vector.x;
            obj["y"] = vector.y;
            obj["z"] = vector.z;
            return obj;
        }

        public static float SerializeFloat(float value)
        {
            return value;
        }

    }
}