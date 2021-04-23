using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Tower Defence/Map")]
public class Map : ScriptableObject
{
    public TableMatrix Tiles;
    public Vector2Int[] Waypoints;
}