using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

public class BuyTowerSystem : MonoBehaviour, IEcsRunSystem
{
    public List<TowerAsset> towers;

    private EcsWorld _world;
    private EcsFilter<BuyTowerEvent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        { 
            var tower = towers[_filter.Get1(i).Value];
            _filter.GetEntity(i).Del<BuyTowerEvent>();

            _world.NewEntity()
                .SetGameObject(Instantiate(tower.preview))
                .Set(new TowerPreview { Asset = tower });
        }
    }
}
