using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ShopObject", fileName = "NewShopObject", order = 51)]
public class TowerData : ScriptableObject
{
    [SerializeField]
    private string nameTower;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int price;
    // [SerializeField] 
    // private string towerPrefabName;
    
    [SerializeField] 
    private Entity towerEntity;


    public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public string NameTower
    {
        get { return nameTower; }
        set { nameTower = value; }
    }

    public int Price
    {
        get { return price; }
        set { price = value; }
    }

    // public string Pref
    // {
    //     get { return towerPrefabName; }
    //     set { towerPrefabName = value; }
    // }

    public Entity entity
    {
        get { return towerEntity; }
        set { towerEntity = value; }
    }
}