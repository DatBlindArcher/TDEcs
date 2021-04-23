using Leopotam.Ecs;
using UnityEngine;

public class ArriveMinionSystem : MonoBehaviour, IEcsRunSystem
{
    private EcsFilter<Health> _stateFilter;
    private EcsFilter<MinionArrived, Minion> _minionFilter;

    public void Run()
    {
        int damage = 0;

        foreach (var i in _minionFilter)
        {
            damage += _minionFilter.Get2(i).Damage;
            _minionFilter.GetEntity(i).DestroyWithLink();
        }

        foreach (var i in _stateFilter)
        {
            _stateFilter.Get1(i).Value -= damage;
        }
    }
}
