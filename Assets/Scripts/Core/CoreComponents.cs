using UnityEngine;

public struct GameObjectLink
{
    public GameObject Link;
    public UpdateDirection UpdatePosition;
    public UpdateDirection UpdateRotation;
    public UpdateDirection UpdateScale;
}

public struct ComponentLink<T> where T : Component
{
    public T Link;
}

public struct Position
{
    public Vector3 Value;
}

public struct Rotation
{
    public Quaternion Value;
}

public struct Scale
{
    public Vector3 Value;
}