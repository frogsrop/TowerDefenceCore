using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class showFPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _meanFpsText;
    [SerializeField] private TextMeshProUGUI _minFpsText;
    [SerializeField] private TextMeshProUGUI _maxFpsText;
    [SerializeField] private TextMeshProUGUI _averageFpsText;
    [SerializeField] private TextMeshProUGUI _medianFpsText;

    private int _fpsCount;

    private float _poolingTime = 1f;
    private float _time;
    private float _fps;
    private List<float> _fpsArray;
    private int _lengthArray;
    private int _indexArray;

    private void Start()
    {
        _fpsArray = new List<float>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _fpsCount++;
        if (_time >= _poolingTime)
        {
            UpdateFps();

            UpdateMinFps();
            UpdateMaxFps();
            UpdateAverageFps();
            UpdateMedianFps();
            _time -= _poolingTime;
            _fpsCount = 0;
        }
    }

    private void UpdateMedianFps()
    {
        var sortedPNumbers = _fpsArray.OrderBy(r => r).ToArray();
        var mid = sortedPNumbers.Length / 2;
        var median = sortedPNumbers.Length == 0
            ? 0
            : sortedPNumbers.Length % 2 != 0
                ? sortedPNumbers[mid]
                : (sortedPNumbers[mid] + sortedPNumbers[mid - 1]) / 2;
        _medianFpsText.text = "MED: " + Mathf.RoundToInt(median);
    }

    private void UpdateFps()
    {
        _fps = _fpsCount / _time;
        _fpsArray.Add(_fps);
        if (_fpsArray.Count > 10)
        {
            _fpsArray.RemoveAt(0);
        }

        _meanFpsText.text = "FPS: " + Mathf.RoundToInt(_fps);
    }

    private void UpdateMinFps()
    {
        _minFpsText.text = "MIN: " + Mathf.RoundToInt(_fpsArray.Min());
    }

    private void UpdateMaxFps()
    {
        _maxFpsText.text = "MAX: " + Mathf.RoundToInt(_fpsArray.Max());
    }

    private void UpdateAverageFps()
    {
        _averageFpsText.text = "AVR: " + Mathf.RoundToInt(_fpsArray.Average());
    }
}