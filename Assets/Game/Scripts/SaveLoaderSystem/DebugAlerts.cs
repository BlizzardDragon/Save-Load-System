using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    public static class DebugAlerts
    {
        public static void AlertOnLoad(string name, int value)
        {
            Debug.Log($"<color=yellow>{name} loaded: {value}!</color>");
        }

        public static void AlertOnSave(string name, int value)
        {
            Debug.Log($"<color=green>{name} saved: {value}!</color>");
        }

        public static void AlertOnDefaultLoad(string name, int value)
        {
            Debug.Log($"<color=cyan>{name} loaded: {value}!</color>");
        }

        public static void AlertObjectsSirensOnLoad<T>(IEnumerable<IObjectType<T>> objects)
        {
            CountTypes(out Dictionary<string, int> objTypes, objects);

            foreach (var type in objTypes)
            {
                AlertOnLoad(type.Key, type.Value);
            }
        }

        public static void AlertOnSaveSceneObjects<T>(IEnumerable<IObjectType<T>> objects)
        {
            CountTypes(out Dictionary<string, int> objTypes, objects);

            foreach (var type in objTypes)
            {
                AlertOnSave(type.Key, type.Value);
            }
        }

        public static void AlertOnDefaultLoadSceneObjects<T>(IEnumerable<IObjectType<T>> objects)
        {
            CountTypes(out Dictionary<string, int> objTypes, objects);

            foreach (var type in objTypes)
            {
                AlertOnDefaultLoad(type.Key, type.Value);
            }
        }

        private static void CountTypes<T>(
            out Dictionary<string, int> objTypes,
            IEnumerable<IObjectType<T>> objects)
        {
            objTypes = new();

            foreach (var obj in objects)
            {
                var key = obj.Type.ToString();
                if (objTypes.ContainsKey(key))
                {
                    objTypes[key]++;
                }
                else
                {
                    objTypes.Add(key, 1);
                }
            }
        }
    }
}