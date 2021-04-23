public struct Health
{
    public int Value;
}

public struct Gold
{
    public int Value;
}

public struct Wave
{
    public int Value;
}

public struct WaveInProgress
{

}

public struct WaveSpawning
{
    public float Timer;
}

public struct BuyTowerEvent
{
    public int Value;
}

public struct NextWaveEvent
{

}

public struct Minion
{
    public float Step;
    public float Speed;
    public float Health;
    public int Damage;
    public int Gold;
}

public struct MinionArrived
{

}

public struct Tower
{
    public float Timer;
    public float Damage;
    public float FireRate;
    public float Radius;
}

public struct TowerPreview
{
    public TowerAsset Asset;
}