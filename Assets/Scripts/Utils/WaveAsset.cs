using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Tower Defence/Wave")]
public class WaveAsset : ScriptableObject
{
    public List<WaveSpawn> Spawns;
}

[Serializable]
public struct WaveSpawn
{
    public float time;
    public float health;
    public GameObject prefab;
    public float movementSpeed;
    public int damage;
    public int gold;
}