using Leopotam.Ecs;
using UnityEngine;

public class MapLoadingSystem : MonoBehaviour, IEcsInitSystem
{
    public GameObject PathPrefab;
    public GameObject GrassPrefab;
    public GameObject PortalPrefab;
    public GameObject CastlePrefab;

    public Map map;

    public void Init()
    {
        float xOffset = map.Tiles.Width / 2f - 0.5f;
        float yOffset = map.Tiles.Height / 2f - 0.5f;

        for (int i = 0; i < map.Tiles.Width; i++)
        {
            for (int j = 0; j < map.Tiles.Height; j++)
            {
                var val = map.Tiles[i, j];
                GameObject prefab;

                if (val == 0) prefab = GrassPrefab;
                else if (val == 2) prefab = PortalPrefab;
                else if (val == 3) prefab = CastlePrefab;
                else prefab = PathPrefab;

                Instantiate(prefab, new Vector3(i - xOffset, 0f, j - yOffset), Quaternion.identity, transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying || map is null) return;
#endif
        float xOffset = map.Tiles.Width / 2f - 0.5f;
        float yOffset = map.Tiles.Height / 2f - 0.5f;

        for (int i = 0; i < map.Tiles.Width; i++)
        {
            for (int j = 0; j < map.Tiles.Height; j++)
            {
                var val = map.Tiles[i, j];

                if (val == 0) Gizmos.color = Color.green;
                else if (val == 2) Gizmos.color = Color.grey;
                else if (val == 3) Gizmos.color = Color.red;
                else Gizmos.color = Color.yellow;

                Gizmos.DrawCube(new Vector3(i - xOffset, 0f, j - yOffset), Vector3.one);
            }
        }

        if (map.Waypoints.Length > 0)
        {
            Gizmos.color = Color.cyan;

            for (int i = 0; i < map.Waypoints.Length; i++)
            {
                Gizmos.DrawSphere(new Vector3(map.Waypoints[i].x - xOffset, 1f, map.Waypoints[i].y - yOffset), 0.1f);

                if (i > 0)
                {
                    Gizmos.DrawLine(
                        new Vector3(map.Waypoints[i - 1].x - xOffset, 1f, map.Waypoints[i - 1].y - yOffset),
                        new Vector3(map.Waypoints[i].x - xOffset, 1f, map.Waypoints[i].y - yOffset));
                }
            }
        }
    }
}
