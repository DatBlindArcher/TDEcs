using Leopotam.Ecs;
using UnityEngine;

public class TowerFireSystem : MonoBehaviour, IEcsRunSystem
{
    private EcsFilter<Tower, Position, Rotation> _filter;
    private EcsFilter<Minion, Position>.Exclude<MinionArrived> _minionFilter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var tower = ref _filter.Get1(i);
            tower.Timer -= Time.deltaTime;

            if (tower.Timer < 0f)
            {
                var position = _filter.Get2(i).Value;
                float maxStep = float.NegativeInfinity;
                EcsEntity target = default;

                foreach (var m in _minionFilter)
                {
                    if (Vector3.Distance(position, _minionFilter.Get2(m).Value) <= tower.Radius)
                    {
                        if (_minionFilter.Get1(m).Step > maxStep)
                        {
                            target = _minionFilter.GetEntity(m);
                        }
                    }
                }

                if (!target.IsNull())
                {
                    var direction = target.Get<Position>().Value - position;
                    direction.y = 0f;
                    direction.Normalize();
                    _filter.Get3(i).Value = Quaternion.LookRotation(direction, Vector3.up);
                    tower.Timer = 1f / tower.FireRate;
                    target.Get<Minion>().Health -= tower.Damage;
                    _filter.GetEntity(i).GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
