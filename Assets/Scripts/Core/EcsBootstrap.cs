using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using UnityEngine;

//  - Think of more complex situations for this composition Type
//      - Link with Unity -> Solved with ComponentLink
//      - System Settings -> Solved with MonoBehaviours
//      - ReactiveUI -> Solved with Systems
//      - Adding component to certain Entity -> Unity through UnityLink, Systems through Entity should be owner
//      - Events -> Solved with Get/Del

[DefaultExecutionOrder(-100)]
public class EcsBootstrap : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _updateSystems;
    private EcsSystems _postSystems;
    private EcsSystems _customSystems;
    private EcsSystems _uiSystems;

    public Transform systemsRoot;

    public static EcsBootstrap Instance { get; private set; }

#if UNITY_EDITOR
    private GameObject _worldObserver;
    private GameObject _systemsObserver;
#endif

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _world = new EcsWorld();
        _updateSystems = new EcsSystems(_world, "Main Systems");
        _postSystems = new EcsSystems(_world, "Post Systems");
        _customSystems = new EcsSystems(_world, "Custom Systems");
        _uiSystems = new EcsSystems(_world, "UI Systems");

        _updateSystems.Add(new EntityToGameObjectSystem());
        _updateSystems.Add(_customSystems);
        _postSystems.Add(new GameObjectToEntitySystem());
        _postSystems.Add(_uiSystems);

        foreach (Transform transform in systemsRoot)
            if (AddSystemsOfTransform(transform, out EcsSystems rootSystems))
                _customSystems.Add(rootSystems);

#if UNITY_EDITOR
        _worldObserver = Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
        _systemsObserver = Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
#endif
    }

    private void Start()
    {
        _updateSystems.Init();
        _postSystems.Init();
    }

    private void Update()
    {
        _updateSystems.Run();
    }

    private void LateUpdate()
    {
        _postSystems.Run();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            _updateSystems.Destroy();
            _postSystems.Destroy();
            _world.Destroy();
            _updateSystems = null;
            _postSystems = null;
            _world = null;
            Instance = null;

#if UNITY_EDITOR
            Destroy(_worldObserver);
            Destroy(_systemsObserver);
#endif
        }
    }

    public void AddSystem(IEcsSystem system)
    {
        int idx = _customSystems.GetNamedRunSystem(system.GetType().Name);
        if (idx >= 0) _customSystems.SetRunSystemState(idx, true);
        else
        {
            _customSystems.Add(system, system.GetType().Name);
            EcsSystems.InjectDataToSystem(system, _world, new Dictionary<Type, object> { });
        }
    }

    public void AddUISystem(IEcsSystem system)
    {
        int idx = _uiSystems.GetNamedRunSystem(system.GetType().Name);
        if (idx >= 0) _uiSystems.SetRunSystemState(idx, true);
        else
        {
            _uiSystems.Add(system, system.GetType().Name);
            EcsSystems.InjectDataToSystem(system, _world, new Dictionary<Type, object> { });
        }
    }

    public void RemoveSystem(IEcsSystem system)
    {
        int idx = _customSystems.GetNamedRunSystem(system.GetType().Name);
        if (idx >= 0) _customSystems.SetRunSystemState(idx, false);
    }

    public void RemoveUISystem(IEcsSystem system)
    {
        int idx = _uiSystems.GetNamedRunSystem(system.GetType().Name);
        if (idx >= 0) _uiSystems.SetRunSystemState(idx, false);
    }

    private bool AddSystemsOfTransform(Transform transform, out EcsSystems systems)
    {
        bool hasSystem = false;
        systems = new EcsSystems(_world, transform.gameObject.name + " Systems");

        foreach (var system in transform.GetComponents<IEcsSystem>())
        {
            hasSystem = true;
            systems.Add(system);
        }

        foreach (Transform child in transform)
        {
            if (AddSystemsOfTransform(child, out EcsSystems childSystems))
            {
                hasSystem = true;
                systems.Add(childSystems);
            }
        }

        return hasSystem;
    }
}