using UnityEngine;

public class TargetFps : MonoBehaviour
{
    [SerializeField]
    private int target = 60;

    void Awake()
    {
        Application.targetFrameRate = target;
    }

    void Update()
    {
        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;
    }
}