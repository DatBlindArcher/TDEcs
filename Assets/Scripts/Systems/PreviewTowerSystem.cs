using Leopotam.Ecs;
using UnityEngine;

public class PreviewTowerSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter<Gold> _stateFilter;
    private EcsFilter<TowerPreview, Position> _filter;

    private Plane _plane;
    private Camera _camera;

    public void Init()
    {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.up / 2f);
    }

    public void Run()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        foreach (var i in _filter)
        {
            var asset = _filter.Get1(i).Asset;

            if (_plane.Raycast(ray, out float enter))
            {
                var position = ray.GetPoint(enter);
                position = new Vector3(Mathf.Floor(position.x + 0.5f), 0f, Mathf.Floor(position.z + 0.5f));
                _filter.Get2(i).Value = position;

                if (Input.GetMouseButtonDown(0))
                {
                    _filter.GetEntity(i).DestroyWithLink();

                    _world.NewEntity()
                        .SetGameObject(Instantiate(asset.prefab), position, Quaternion.identity)
                        .Set(new Tower { Damage = asset.damage, Radius = asset.radius, FireRate = asset.fireRate });

                    foreach (var j in _stateFilter)
                    {
                        _stateFilter.Get1(j).Value -= asset.gold;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                _filter.GetEntity(i).DestroyWithLink();
            }
        }
    }
}
