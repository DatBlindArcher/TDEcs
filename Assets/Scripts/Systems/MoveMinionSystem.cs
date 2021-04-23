using Leopotam.Ecs;
using UnityEngine;

public class MoveMinionSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<Minion, Position, Rotation> _filter;

    private float _length;
    private Vector2Int[] _waypoints;
    private Vector2 _offset;

    public void Init()
    {
        var mapSystem = FindObjectOfType<MapLoadingSystem>();
        _waypoints = mapSystem.map.Waypoints;
        _offset = new Vector2(mapSystem.map.Tiles.Width / 2f, mapSystem.map.Tiles.Height / 2f) - Vector2.one / 2f;
        _length = 0f;

        if (_waypoints.Length > 1)
        {
            for (int i = 1; i < _waypoints.Length; i++)
            {
                _length += Vector2Int.Distance(_waypoints[i - 1], _waypoints[i]);
            }
        }
    }

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var minion = ref _filter.Get1(i);
            minion.Step += Time.deltaTime * minion.Speed;

            if (minion.Step >= _length)
            {
                _filter.GetEntity(i).Set<MinionArrived>();
            }

            else
            {
                float localStep;
                var distance = 0f;
                var target = 0;

                do
                {
                    target++;
                    localStep = minion.Step - distance;
                    distance += Vector2.Distance(_waypoints[target - 1], _waypoints[target]);
                } while (minion.Step > distance);

                Vector2 direction = _waypoints[target] - _waypoints[target - 1];
                Vector2 position = _waypoints[target - 1] + 
                    direction * (localStep / Vector2.Distance(_waypoints[target - 1], _waypoints[target]))
                    - _offset;

                _filter.Get2(i).Value = new Vector3(position.x, 1f, position.y);
            }
        }
    }
}
