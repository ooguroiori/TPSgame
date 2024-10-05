using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStatusSO : ScriptableObject
{
    public List<EnemyStatus> enemyStatusList = new List<EnemyStatus>();

    [System.Serializable]
    public class EnemyStatus{
        [Header("名前")]
        [SerializeField] string name;
        [SerializeField] int hp;
        [SerializeField] int atk;
        [SerializeField] int def;
        [SerializeField] Type type;

        public enum Type{
            nomal,
            fire,
            water,
            thunder,
            light,
            dark
        }

        public int HP { get => hp; }
    }
}