using Leopotam.Ecs;
using UnityEngine;

public class GameStateSystem : MonoBehaviour, IEcsInitSystem
{
    public GameObject GameStateLink;

    private EcsWorld _world = null;

    public void Init()
    {
        var entity = _world.NewEntity()
            .SetGameObject(GameStateLink)
            .Set(new Health { Value = 100 })
            .Set(new Gold { Value = 100 })
            .Set(new Wave { Value = 0 });
    }
}
