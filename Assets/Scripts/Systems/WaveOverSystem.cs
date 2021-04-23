using Leopotam.Ecs;
using UnityEngine;

public class WaveOverSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<Minion> _minionFilter;
    private EcsFilter<Wave, WaveInProgress>.Exclude<WaveSpawning> _stateFilter;

    private int waves;

    public void Init()
    {
        waves = FindObjectOfType<SpawnWaveSystem>().Waves.Count;
    }

    public void Run()
    {
        foreach (var i in _stateFilter)
        {
            if (_minionFilter.GetEntitiesCount() == 0)
            {
                if (_stateFilter.Get1(i).Value == waves)
                {
                    Debug.Log("You Made It!");
                }

                else
                {
                    _stateFilter.GetEntity(i).Del<WaveInProgress>();
                }
            }
        }
    }
}
