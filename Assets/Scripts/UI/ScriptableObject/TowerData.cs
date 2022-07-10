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
    //TODO:[SerializeField] private GameObject pref;


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

}