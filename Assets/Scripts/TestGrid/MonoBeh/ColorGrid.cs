using UnityEngine;

public class ColorGrid : MonoBehaviour
{
    void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter");
    }

    void OnMouseExit()
    {
        Debug.Log("OnMouseExit");
            transform.position = new Vector3(100, 100, 0);
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}