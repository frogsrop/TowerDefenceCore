using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BankTowerController : MonoBehaviour
{
    [SerializeField]
    private int _towerData;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private Image iconSprite;

    public int TowerData
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
        iconSprite.sprite = iconSprite.sprite;
        priceText.text = TowerData.ToString();
        nameText.text = TowerData.ToString();
    }
}
