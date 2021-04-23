using Leopotam.Ecs;
using UnityEngine;

public class KillMinionSystem : MonoBehaviour, IEcsRunSystem
{
    private EcsFilter<Gold> _stateFilter;
    private EcsFilter<Minion>.Exclude<MinionArrived> _filter;

    public void Run()
    {
        int gold = 0;

        foreach (var i in _filter)
        {
            if (_filter.Get1(i).Health <= 0)
            {
                gold += _filter.Get1(i).Gold;
                _filter.GetEntity(i).DestroyWithLink();
            }
        }

        if (gold != 0)
        {
            foreach (var i in _stateFilter)
            {
                _stateFilter.Get1(i).Value += gold;
            }
        }
    }
}
