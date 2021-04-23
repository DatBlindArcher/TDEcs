using Leopotam.Ecs;
using UnityEngine;

public class DeathSystem : MonoBehaviour, IEcsRunSystem
{
    private EcsFilter<Health> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            if (_filter.Get1(i).Value <= 0)
            {
                Debug.Log("You Died!");
            }
        }
    }
}
