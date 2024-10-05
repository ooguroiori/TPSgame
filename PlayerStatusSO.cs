using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatusSO : ScriptableObject
{
    [SerializeField] int hp;
    [SerializeField] int mp;
    [SerializeField] int atk;
    [SerializeField] int def;

    public int HP { get => hp; }
    public int MP { get => mp; }
    public int ATK { get => atk; }
    public int DEF { get => def; }
}