using System;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

public abstract class ReactiveUI : MonoBehaviour, IEcsRunSystem
{
    private int snapshot = 0;
    private void Awake() => this.AddUISystem();
    private void OnDestroy() => this.RemoveUISystem();
    public abstract void Run();

    protected bool CheckSnapshot(int hashCode)
    {
        if (snapshot != hashCode)
        {
            snapshot = hashCode;
            return true;
        }

        return false;
    }

    protected void CheckFilter(EcsFilter filter)
    {
        if (filter.GetEntitiesCount() > 1)
            throw new IndexOutOfRangeException($"{filter.GetEntitiesCount()} entities were found of structure " +
                $"{{ {string.Join(", ", filter.IncludedTypes.Select(x => x.Name))} }} " +
                $"for ReactiveUI on {gameObject.name}.");
    }
}

public abstract class ReactiveUI<T1> : ReactiveUI where T1 : struct
{
    protected EcsFilter<T1> _filter;

    public sealed override void Run()
    {
        CheckFilter(_filter);

        foreach (var i in _filter)
            if (CheckSnapshot(_filter.Get1(i).GetHashCode()))
                OnChanged(_filter.GetEntity(i), _filter.Get1(i));
    }

    public abstract void OnChanged(in EcsEntity entity, in T1 arg1);
}

public abstract class ReactiveUI<T1, T2> : ReactiveUI where T1 : struct where T2 : struct
{
    protected EcsFilter<T1, T2> _filter;

    public sealed override void Run()
    {
        CheckFilter(_filter);

        foreach (var i in _filter)
            if (CheckSnapshot(Tuple.Create(_filter.Get1(i), _filter.Get2(i)).GetHashCode()))
                OnChanged(_filter.GetEntity(i), _filter.Get1(i), _filter.Get2(i));
    }

    public abstract void OnChanged(in EcsEntity entity, in T1 arg1, in T2 arg2);
}

public abstract class ReactiveUI<T1, T2, T3> : ReactiveUI where T1 : struct where T2 : struct where T3 : struct
{
    protected EcsFilter<T1, T2, T3> _filter;

    public sealed override void Run()
    {
        CheckFilter(_filter);

        foreach (var i in _filter)
            if (CheckSnapshot(Tuple.Create(_filter.Get1(i), _filter.Get2(i), _filter.Get3(i)).GetHashCode())) 
                OnChanged(_filter.GetEntity(i), _filter.Get1(i), _filter.Get2(i), _filter.Get3(i));
    }

    public abstract void OnChanged(in EcsEntity entity, in T1 arg1, in T2 arg2, in T3 arg3);
}

public abstract class ReactiveUI<T1, T2, T3, T4> : ReactiveUI where T1 : struct where T2 : struct where T3 : struct where T4 : struct
{
    protected EcsFilter<T1, T2, T3, T4> _filter;

    public sealed override void Run()
    {
        CheckFilter(_filter);

        foreach (var i in _filter)
            if (CheckSnapshot(Tuple.Create(_filter.Get1(i), _filter.Get2(i), _filter.Get3(i), _filter.Get4(i)).GetHashCode()))
                OnChanged(_filter.GetEntity(i), _filter.Get1(i), _filter.Get2(i), _filter.Get3(i), _filter.Get4(i));
    }

    public abstract void OnChanged(in EcsEntity entity, in T1 arg1, in T2 arg2, in T3 arg3, in T4 arg4);
}