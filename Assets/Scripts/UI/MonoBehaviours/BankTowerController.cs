using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BankTowerController : MonoBehaviour
{
    [SerializeField]
    private TowerData towerData;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private Image iconSprite;
    
    private void Start()
    {
        Init(); 
    }
    public TowerData TowerData
    {   
        get => towerData;
        set
        {
            towerData = value;
            Init();
        }
        
    }
    
    public void Init()
    {
        iconSprite.sprite = TowerData.Icon;
        priceText.text = TowerData.Price.ToString();
        nameText.text = TowerData.NameTower;
    }
}