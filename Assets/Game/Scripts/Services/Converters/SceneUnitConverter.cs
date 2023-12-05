using System.Collections.Generic;

namespace SaveLoad
{
    public class SceneUnitConverter : 
        SceneObjectConverter<SceneUnitData, SceneUnitsDataArray, UnitObject, UnitType>
    {
        public SceneUnitConverter()
        {
            _prefabLinks = new()
            {
                {UnitType.RoyalWarrior, "Units/WK_heavy_infantry_B"},
                {UnitType.RoyalArcher, "Units/WK_light_archer_B"},
                {UnitType.RoyalRider, "Units/WK_light_cavalry"},
                {UnitType.OrcShaman, "Units/Orc_shaman"},
                {UnitType.OrcArcher, "Units/Orc_archer"},
                {UnitType.OrcСatapult, "Units/Orc_Catapult"},
            };
        }


        protected override bool CheckForType(SceneUnitData data)
        {
            return _prefabLinks.ContainsKey(data.Type);
        }

        protected override string GetLinkToPrefab(SceneUnitData data, Dictionary<UnitType, string> _prefabLinks)
        {
            return _prefabLinks[data.Type];
        }

        protected override void SetupObject(SceneUnitData data, UnitObject gameObj)
        {
            gameObj.HitPoints = data.HitPoints;
            gameObj.Speed = data.Speed;
            gameObj.Damage = data.Damage;
            gameObj.Damage = data.Damage;
            gameObj.Type = data.Type;

            if (data.HitPoints < 1)
            {
                gameObj.gameObject.SetActive(false);
            }
        }

        protected override void SetupData(SceneUnitData data, UnitObject gameObj)
        {
            data.HitPoints = gameObj.HitPoints;
            data.Speed = gameObj.Speed;
            data.Damage = gameObj.Damage;
            data.Type = gameObj.Type;
        }
    }
}