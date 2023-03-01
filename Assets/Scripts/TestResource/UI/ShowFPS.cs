using UnityEngine;
using TMPro;
using System;

public class showFPS : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _meanFps;
    [SerializeField]
    private TextMeshProUGUI _minFps;
    [SerializeField]
    private TextMeshProUGUI _maxFps;
    [SerializeField] 
    private TextMeshProUGUI _averageFps;
    [SerializeField]
    private TextMeshProUGUI _medianFps;

    private int _meanCount;
    private int _maxCount;
    private int _minCount;
    private int _averageCount;
    private int _medianCount;

    private float _poolingTime = 1f;
    private float _time;
    private int _waitOutput;
    private int[] _fpsArray;
    private int _lengthArray;
    private int _indexArray;

    private void Start()
    {
        _minCount = 60;
        _indexArray = 0;
        _lengthArray = 10;
        _fpsArray = new int[_lengthArray];
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _meanCount++;
        if (_time >= _poolingTime)
        {
            _waitOutput++;
            meanFps();
            minFps(); 
            maxFps(); 
            averageFps();
            medianFps();
            _time -= _poolingTime;
            _meanCount = 0;
        }        
    }

    private void medianFps()
    {
        _medianCount = GetMedian(_fpsArray);
        _medianFps.text = "MED: " + _medianCount.ToString();
    }

    private void meanFps()
    {
        _meanCount = Mathf.RoundToInt(_meanCount / _time);
        _meanFps.text = "FPS: " + _meanCount.ToString();
    }

    private void minFps()
    {
        if (_minCount >= _meanCount && _waitOutput > 10)
        {
            _minCount = _meanCount;
            _minFps.text = "MIN: " + _minCount.ToString();

        }
    }

    private void maxFps()
    {
        if (_maxCount < _meanCount)
        {
            _maxCount = _meanCount;
            _maxFps.text = "MAX: " + _maxCount.ToString();
        }
    }

    private void averageFps()
    {
        _fpsArray[_indexArray] = _meanCount;
        var sum = SumArray(_fpsArray);
        _averageCount = sum / _fpsArray.Length;
        _indexArray++;
        if (_indexArray >= _fpsArray.Length)
        {
            _lengthArray += 10;
            Array.Resize(ref _fpsArray, _lengthArray);
        }
        _averageFps.text = "AVR: " + _averageCount.ToString();
    }

    private int SumArray(int[] toBeSummed)
    {
        int sum = 0;
        foreach (int item in toBeSummed)
        {
            sum += item;
        }
        return sum;
    }

    private int GetMedian(int[] sourceNumbers)
    {
        int[] sortedPNumbers = (int[])sourceNumbers.Clone();
        Array.Sort(sortedPNumbers);
        int size = sortedPNumbers.Length;
        int mid = size / 2;
        int median = (size % 2 != 0) ? (int)sortedPNumbers[mid] : ((int)sortedPNumbers[mid] + (int)sortedPNumbers[mid - 1]) / 2;
        return median;
    }
}