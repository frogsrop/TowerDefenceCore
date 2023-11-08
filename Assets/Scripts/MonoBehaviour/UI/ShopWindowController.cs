using UnityEngine;
using UnityEngine.UI;
public class ShopWindowController : MonoBehaviour
{
    private Animator _anim;
    private Image _img;
    private bool _isOpened = false;
    [SerializeField]
    private Sprite iconOpen;
    [SerializeField]
    private Sprite iconClose;
    [SerializeField]
    private GameObject panel;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _img = GetComponent<Image>();
    }

    public void ControlShopPanel()
    {
        if (!_isOpened)
        {
            _isOpened = true;
            _anim.SetBool("IsOpened", true);
            _img.sprite = iconClose;
            panel.GetComponent<Animator>().SetBool("IsOpened", true);
        }
        else
        {
            _isOpened = false;
            _anim.SetBool("IsOpened", false);
            _img.sprite = iconOpen;
            panel.GetComponent<Animator>().SetBool("IsOpened", false);
        }
    }
}
