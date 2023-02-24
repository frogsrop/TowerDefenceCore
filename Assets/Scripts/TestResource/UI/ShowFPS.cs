using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class showFPS : MonoBehaviour
{
    public TextMeshProUGUI FpsText;

    private float poolingTime = 1f; 
    private float time;
    private int frameCount;

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= poolingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = "FPS: " + frameRate.ToString();

            time -= poolingTime;
            frameCount = 0;
        }
    }
}