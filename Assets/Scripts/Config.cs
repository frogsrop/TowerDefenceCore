using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Config : MonoBehaviour
{
    void Start()
    {
       // Application.targetFrameRate = 60;
    }

    [SerializeField] private TMP_Text fpsText;
    private float frameCount = 0;
    private float pollingTime = 3f;
    private float time;
    void Update()
    {
        time += Time.deltaTime;
        frameCount += 1;
        if (time >= pollingTime)
        {
            int fps = Mathf.RoundToInt(frameCount / time);
            fpsText.text = fps.ToString();
            time -= pollingTime;
            frameCount = 0;
        }
    }
}
