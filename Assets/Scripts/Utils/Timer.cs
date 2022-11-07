using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float fullTimeout;
    private float currentTimeout;

    public Timer(float timout)
    {
        fullTimeout = timout;
        currentTimeout = timout;
    }

    public Boolean refreshTimerAndCheckFinish()
    {
        currentTimeout -= Time.deltaTime;
        if (currentTimeout <= 0)
        {
            currentTimeout = fullTimeout;
            return true;
        }

        return false;
    }
}