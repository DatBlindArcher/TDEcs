using Leopotam.Ecs;
using UnityEngine;

public static class EcsExtensions
{
    public static EcsEntity SetGameObject(
        this EcsEntity entity, GameObject gameObject,
        UpdateDirection updatePosition = UpdateDirection.None,
        UpdateDirection updateRotation = UpdateDirection.None,
        UpdateDirection updateScale = UpdateDirection.None)
    {
        var entityLink = gameObject.GetComponent<EntityLink>() ?? gameObject.AddComponent<EntityLink>();
        entityLink.Link = entity;

        ref var link = ref entity.Get<GameObjectLink>();
        link.Link = gameObject;
        link.UpdatePosition = entityLink.UpdatePosition != UpdateDirection.None ? entityLink.UpdatePosition : updatePosition;
        link.UpdateRotation = entityLink.UpdateRotation != UpdateDirection.None ? entityLink.UpdateRotation : updateRotation;
        link.UpdateScale = entityLink.UpdateScale != UpdateDirection.None ? entityLink.UpdateScale : updateScale;

        entity.Set(new Position { Value = gameObject.transform.localPosition });
        entity.Set(new Rotation { Value = gameObject.transform.localRotation });
        entity.Set(new Scale { Value = gameObject.transform.localScale });
        return entity;
    }

    public static EcsEntity SetGameObject(
        this EcsEntity entity, GameObject gameObject,
        Vector3 position, Quaternion rotation,
        UpdateDirection updatePosition = UpdateDirection.None, 
        UpdateDirection updateRotation = UpdateDirection.None, 
        UpdateDirection updateScale = UpdateDirection.None)
    {
        var entityLink = gameObject.GetComponent<EntityLink>() ?? gameObject.AddComponent<EntityLink>();
        entityLink.Link = entity;

        ref var link = ref entity.Get<GameObjectLink>();
        link.Link = gameObject;
        link.UpdatePosition = entityLink.UpdatePosition != UpdateDirection.None ? entityLink.UpdatePosition : updatePosition;
        link.UpdateRotation = entityLink.UpdateRotation != UpdateDirection.None ? entityLink.UpdateRotation : updateRotation;
        link.UpdateScale = entityLink.UpdateScale != UpdateDirection.None ? entityLink.UpdateScale : updateScale;

        gameObject.transform.localPosition = position;
        gameObject.transform.localRotation = rotation;

        entity.Set(new Position { Value = position });
        entity.Set(new Rotation { Value = rotation });
        entity.Set(new Scale { Value = gameObject.transform.localScale });
        return entity;
    }

    public static void DestroyWithLink(this EcsEntity entity)
    {
        Object.Destroy(entity.Get<GameObjectLink>().Link);
        entity.Destroy();
    }

    public static T GetComponent<T>(this EcsEntity entity) where T : Component
    {
        if (entity.Has<ComponentLink<T>>())
        {
            return entity.Get<ComponentLink<T>>().Link;
        }

        else if (entity.Has<GameObjectLink>())
        {
            var obj = entity.Get<GameObjectLink>().Link;
            T component = obj.GetComponent<T>();

            if (component)
            {
                entity.Get<ComponentLink<T>>().Link = component;
                return component;
            }

            throw new MissingComponentException($"Could not get Component {typeof(T)} of GameObject {obj.name}.");
        }

        else
        {
            throw new MissingComponentException($"Could not get Component {typeof(T)} of EcsEntity without a GameObjectLink.");
        }
    }



    public static EcsEntity Set<T>(this EcsEntity entity, T component = default) where T : struct
    {
        entity.Replace(component);
        return entity;
    }

    public static void AddSystem(this IEcsSystem system) => EcsBootstrap.Instance?.AddSystem(system);
    public static void AddUISystem(this IEcsSystem system) => EcsBootstrap.Instance?.AddUISystem(system);
    public static void RemoveSystem(this IEcsSystem system) => EcsBootstrap.Instance?.RemoveSystem(system);
    public static void RemoveUISystem(this IEcsSystem system) => EcsBootstrap.Instance?.RemoveUISystem(system);
}