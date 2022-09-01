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
        queryTower = GetEntityQuery(ComponentType.ReadOnly<TowerTag>(), ComponentType.ReadOnly<Translation>()); //список таверов
        queryBullet = GetEntityQuery(ComponentType.ReadOnly<BulletTag>(), ComponentType.ReadOnly<Translation>());   //список пуль
        queryCreep = GetEntityQuery(ComponentType.ReadOnly<CreepTag>(), ComponentType.ReadOnly<IDEnemy>()); //список крипов
    }

    protected override void OnStartRunning()
    {
        bulletPrefab = GetSingleton<BulletSetPrefab>().Prefab;  //задали префаб пули
    }

    protected override void OnUpdate()
    {
        var bulletArray = queryBullet.ToComponentDataArray<BulletTag>(Allocator.Temp);  //массив пуль
        if (bulletArray.Length < 1)//если пуль на карте 0
        {
            newBullet = EntityManager.Instantiate(bulletPrefab);    //создали пулю
            var towerTranslation = queryTower.ToComponentDataArray<Translation>(Allocator.Temp)[0]; //считали положение тавера
            var towerPosition = new float3(towerTranslation.Value.x, towerTranslation.Value.y + 0.5f,
                towerTranslation.Value.z);  //рассчитали координаты спавна пули
            var setSpawnPosition = new Translation { Value = towerPosition };   //создали переменную Translation
            EntityManager.SetComponentData(newBullet, setSpawnPosition);    //добавили компонент который заспавнит пулю на верхушке тавера

            var readEnemyID = queryCreep.ToComponentDataArray<IDEnemy>(Allocator.Temp)[0]; //считали айди перовго крипа
            var enemyID = readEnemyID.value;
            EntityManager.AddComponentData(newBullet, new IDTarget { value = enemyID }); //добавили айдишку моба на пулю
        }
    }
}
