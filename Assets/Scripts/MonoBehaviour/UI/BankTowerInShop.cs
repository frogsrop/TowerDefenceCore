using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BankTowerInShop : MonoBehaviour
{
    [SerializeField]
    private TowerData _towerData;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private Image iconSprite;
    
    private void Start()
    {
        init(); 
    }
    public TowerData towerData
    {   
        get => _towerData;
        set
        {
            _towerData = value;
            init();
        }
    }
    
    public void init()
    {
        iconSprite.sprite = towerData.Icon;
        priceText.text = towerData.Price.ToString();
        nameText.text = towerData.NameTower;
    }
}