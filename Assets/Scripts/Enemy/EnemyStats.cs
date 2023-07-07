using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObject/EnemyStats", order = 0)]
    public class EnemyStats : ScriptableObject
    {
        public float health;
        public float attackPower;
        public float defense;
        public float speed;
    }
}
