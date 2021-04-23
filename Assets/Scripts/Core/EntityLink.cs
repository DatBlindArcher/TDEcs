using Leopotam.Ecs;
using UnityEngine;

public enum UpdateDirection
{
    None = 0,
    FromEntity = 1,
    FromGameObject = 2
}

public class EntityLink : MonoBehaviour
{
    public EcsEntity Link;
    public UpdateDirection UpdatePosition = UpdateDirection.None;
    public UpdateDirection UpdateRotation = UpdateDirection.None;
    public UpdateDirection UpdateScale = UpdateDirection.None;

    private void OnDestroy()
    {
        if (Link.IsWorldAlive() && Link.IsAlive()) Link.Destroy();
    }
}

public class EntityToGameObjectSystem : IEcsRunSystem
{
    private EcsFilter<GameObjectLink, Position, Rotation, Scale> _filter = null;

    public void Run()
    {
        GameObject gameObject;

        foreach (var i in _filter)
        {
            ref var link = ref _filter.Get1(i);
            gameObject = link.Link;

            if (link.UpdatePosition == UpdateDirection.FromEntity)
                gameObject.transform.localPosition = _filter.Get2(i).Value;
            if (link.UpdateRotation == UpdateDirection.FromEntity)
                gameObject.transform.localRotation = _filter.Get3(i).Value;
            if (link.UpdateScale == UpdateDirection.FromEntity)
                gameObject.transform.localScale = _filter.Get4(i).Value;
        }
    }
}

public class GameObjectToEntitySystem : IEcsRunSystem
{
    private EcsFilter<GameObjectLink, Position, Rotation, Scale> _filter = null;

    public void Run()
    {
        GameObject gameObject;

        foreach (var i in _filter)
        {
            ref var link = ref _filter.Get1(i);
            gameObject = link.Link;

            if (link.UpdatePosition == UpdateDirection.FromGameObject)
                _filter.Get2(i).Value = gameObject.transform.localPosition;
            if (link.UpdateRotation == UpdateDirection.FromGameObject)
                _filter.Get3(i).Value = gameObject.transform.localRotation;
            if (link.UpdateScale == UpdateDirection.FromGameObject)
                _filter.Get4(i).Value = gameObject.transform.localScale;
        }
    }
}