using UnityEngine;
using UnityEngine.UI;
public class ShopWindowController : MonoBehaviour
{
    private Animator anim;
    private Image img;
    private bool IsOpened = false;
    [SerializeField]
    private Sprite iconOpen;
    [SerializeField]
    private Sprite iconClose;
    [SerializeField]
    private GameObject panel;

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
            panel.GetComponent<Animator>().SetBool("IsOpened", true);
        }
        else
        {
            IsOpened = false;
            anim.SetBool("IsOpened", false);
            img.sprite = iconOpen;
            panel.GetComponent<Animator>().SetBool("IsOpened", false);
        }
    }
}
