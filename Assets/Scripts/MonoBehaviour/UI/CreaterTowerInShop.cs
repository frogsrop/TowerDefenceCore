using UnityEngine;

[ExecuteInEditMode]
public class CreaterTowerInShop : MonoBehaviour
{
    [SerializeField]
    private Transform Grid;
    [SerializeField]
    private GameObject Pref;
    [SerializeField]
    private TowerData[] MyScriptableObjects;

    private void Start()
    {
        for (int i = 0; i < MyScriptableObjects.Length; i++)
        {
            var node = Instantiate(Pref, Grid);
            node.transform.localScale = Vector3.one;
            node.GetComponent<BankTowerInShop>().towerData = MyScriptableObjects[i];
        }
    }
}