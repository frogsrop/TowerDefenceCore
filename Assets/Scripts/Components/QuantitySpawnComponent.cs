using Unity.Entities;

public struct QuantitySpawnComponent : IComponentData
{
    public int QuantityTowers;
    public int QuantityEnemies;
    public float SpeedShoot;
    public bool OnOff;
}
