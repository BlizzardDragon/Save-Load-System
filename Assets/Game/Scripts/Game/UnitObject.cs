using UnityEngine;

public enum UnitType
{
    RoyalWarrior,
    RoyalArcher,
    RoyalRider,
    OrcShaman,
    OrcArcher,
    OrcСatapult,
}

namespace SaveLoad
{
    public sealed class UnitObject : MonoBehaviour
    {
        [SerializeField]
        private int hitPoints;

        [SerializeField]
        private int speed;

        [SerializeField]
        private int damage;

        [SerializeField]
        private UnitType type;

        public int HitPoints { get => hitPoints; set => hitPoints = value; }
        public int Speed { get => speed; set => speed = value; }
        public int Damage { get => damage; set => damage = value; }
        public UnitType Type { get => type; set => type = value; }
    }
}