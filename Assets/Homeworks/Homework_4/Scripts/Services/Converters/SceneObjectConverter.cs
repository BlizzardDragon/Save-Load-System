using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SaveLoad
{
    public abstract class SceneObjectConverter<TData, TDataArray, TObject, TType>
        where TData : GameObjectData, new()
        where TDataArray : SceneDataArray<TData>, new()
    {
        protected Dictionary<TType, string> _prefabLinks;


        protected abstract bool CheckForType(TData data);
        protected abstract string GetLinkToPrefab(TData data, Dictionary<TType, string> _prefabLinks);
        protected abstract void SetupObject(TData data, TObject gameObj);
        protected abstract void SetupData(TData data, TObject gameObj);

        public GameObject ConvertDataToGameObject(TData data)
        {
            if (!CheckForType(data))
            {
                throw new ArgumentException("This type of prefab does not exist.");
            }

            var position = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
            var rotation = new Quaternion(data.Rotation[0], data.Rotation[1], data.Rotation[2], data.Rotation[3]);

            GameObject newUnit =
                Object.Instantiate(
                    Resources.Load<GameObject>(GetLinkToPrefab(data, _prefabLinks)),
                    position,
                    rotation);

            var tObject = newUnit.GetComponent<TObject>();
            SetupObject(data, tObject);

            return newUnit;
        }

        public GameObject[] ConvertDataToGameObjectsArray(TDataArray dataArray)
        {
            TData[] data = dataArray.DataArray;
            var newObjects = new GameObject[data.Length];

            for (int i = 0; i < newObjects.Length; i++)
            {
                newObjects[i] = ConvertDataToGameObject(data[i]);
            }

            return newObjects;
        }

        public TData[] ConvertGameObjectsToData(List<GameObject> gameObjects)
        {
            int count = 0;
            foreach (var obj in gameObjects)
            {
                if (obj != null)
                {
                    count++;
                }
            }

            TData[] data = new TData[count];

            int objectIndex = 0;
            for (int i = 0; i < data.Length; i++, objectIndex++)
            {
                while (gameObjects[objectIndex] == null)
                {
                    objectIndex++;
                }

                GameObject gameObj = gameObjects[objectIndex];
                data[i] = new TData
                {
                    Position = new float[3]
                    {
                        gameObj.transform.position.x,
                        gameObj.transform.position.y,
                        gameObj.transform.position.z
                    },
                    Rotation = new float[4]
                    {
                        gameObj.transform.rotation.x,
                        gameObj.transform.rotation.y,
                        gameObj.transform.rotation.z,
                        gameObj.transform.rotation.w
                    }
                };

                var tObject = gameObj.GetComponent<TObject>();
                SetupData(data[i], tObject);
            }

            return data;
        }
        
        public TDataArray ConvertGameObjectsToDataArray(List<GameObject> gameObjects)
        {
            return new TDataArray { DataArray = ConvertGameObjectsToData(gameObjects) };
        }
    }
}