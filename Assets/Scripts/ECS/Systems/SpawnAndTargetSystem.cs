using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SpawnAndTargetSystem : SystemBase
{
    private Entity bulletPrefab;
    private Entity newBullet;
    private EntityQuery queryTower;
    private EntityQuery queryBullet;
    private EntityQuery queryCreep;
    


    protected override void OnCreate()
    {
        queryTower = GetEntityQuery(ComponentType.ReadOnly<TowerTag>(), ComponentType.ReadOnly<Translation>()); //������ �������
        queryBullet = GetEntityQuery(ComponentType.ReadOnly<BulletTag>(), ComponentType.ReadOnly<Translation>());   //������ ����
        queryCreep = GetEntityQuery(ComponentType.ReadOnly<CreepTag>(), ComponentType.ReadOnly<IDEnemy>()); //������ ������
    }

    protected override void OnStartRunning()
    {
        bulletPrefab = GetSingleton<BulletSetPrefab>().Prefab;  //������ ������ ����
    }

    protected override void OnUpdate()
    {
        var bulletArray = queryBullet.ToComponentDataArray<BulletTag>(Allocator.Temp);  //������ ����
        if (bulletArray.Length < 1)//���� ���� �� ����� 0
        {
            newBullet = EntityManager.Instantiate(bulletPrefab);    //������� ����
            var towerTranslation = queryTower.ToComponentDataArray<Translation>(Allocator.Temp)[0]; //������� ��������� ������
            var towerPosition = new float3(towerTranslation.Value.x, towerTranslation.Value.y + 0.5f,
                towerTranslation.Value.z);  //���������� ���������� ������ ����
            var setSpawnPosition = new Translation { Value = towerPosition };   //������� ���������� Translation
            EntityManager.SetComponentData(newBullet, setSpawnPosition);    //�������� ��������� ������� ��������� ���� �� �������� ������

            var readEnemyID = queryCreep.ToComponentDataArray<IDEnemy>(Allocator.Temp)[0]; //������� ���� ������� �����
            var enemyID = readEnemyID.value;
            EntityManager.AddComponentData(newBullet, new IDTarget { value = enemyID }); //�������� ������� ���� �� ����
        }
    }
}
