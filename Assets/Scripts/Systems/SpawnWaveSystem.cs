using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaveSystem : MonoBehaviour, IEcsRunSystem
{
    public List<WaveAsset> Waves;

    private EcsWorld _world;
    private EcsFilter<Wave, WaveSpawning> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            var wave = _filter.Get1(i).Value;
            ref var spawning = ref _filter.Get2(i);

            float previousTime = spawning.Timer;
            spawning.Timer += Time.deltaTime;
            float currentTime = spawning.Timer;
            bool finished = true;

            foreach (var spawn in Waves[wave - 1].Spawns)
            {
                if (spawn.time >= previousTime && spawn.time < currentTime)
                {
                    _world.NewEntity()
                        .SetGameObject(Instantiate(spawn.prefab))
                        .Set(new Minion { 
                            Step = 0f, 
                            Speed = spawn.movementSpeed, 
                            Damage = spawn.damage, 
                            Health = spawn.health, 
                            Gold = spawn.gold 
                        });
                }

                else if (spawn.time > currentTime)
                    finished = false;
            }

            if (finished) _filter.GetEntity(i).Del<WaveSpawning>();
        }
    }
}
