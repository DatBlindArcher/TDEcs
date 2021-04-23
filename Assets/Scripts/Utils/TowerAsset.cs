using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower Defence/Tower")]
public class TowerAsset : ScriptableObject
{
    public int gold;
    public float radius;
    public float damage;
    public float fireRate;
    public GameObject prefab;
    public GameObject preview;
}