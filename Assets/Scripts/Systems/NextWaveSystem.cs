using Leopotam.Ecs;
using UnityEngine;

public class NextWaveSystem : MonoBehaviour, IEcsRunSystem
{
    private EcsFilter<NextWaveEvent, Wave> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var entity = ref _filter.GetEntity(i);
            entity.Del<NextWaveEvent>();
            _filter.Get2(i).Value++;
            entity.Set(new WaveInProgress()).Set(new WaveSpawning { Timer = 0f });
        }
    }
}
