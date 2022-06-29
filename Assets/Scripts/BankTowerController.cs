using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BankTowerController : MonoBehaviour
{
    [SerializeField]
    private TowerData _towerData;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private Image iconSprite;

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
        Debug.Log("Hello");
    }
}
