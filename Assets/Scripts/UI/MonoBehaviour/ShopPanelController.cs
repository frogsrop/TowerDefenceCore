using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : MonoBehaviour
{
    private Animator anim;
    private Image img;
    private bool IsOpened = false;
    [SerializeField]
    private Sprite iconOpen;
    [SerializeField]
    private Sprite iconClose;

    private void Start()
    {
        anim = GetComponent<Animator>();
        img = GetComponent<Image>();
    }

    public void ControllShopPanel()
    {
        if (!IsOpened)
        {
            IsOpened = true;
            anim.SetBool("IsOpened", true);
            img.sprite = iconClose;
            
        }
        else
        {
            IsOpened = false;
            anim.SetBool("IsOpened", false);
            img.sprite = iconOpen;
        }
    }
}