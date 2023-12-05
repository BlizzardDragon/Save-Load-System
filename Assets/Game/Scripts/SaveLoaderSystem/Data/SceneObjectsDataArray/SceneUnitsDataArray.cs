using System;
using UnityEngine;

namespace SaveLoad
{
    [Serializable]
    public class SceneUnitsDataArray : SceneDataArray<SceneUnitData>
    {
    }

    [Serializable]
    public class SceneUnitData : GameObjectData, IObjectType<UnitType>
    {
        public int HitPoints;
        public int Speed;
        public int Damage;
        [field: SerializeField] public UnitType Type { get; set; }
    }
}