using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

    public Transform hoursTransform, minutesTransform, secondsTransform;
    const float degreesPerHour = 30f, degreesPerMinute = 6f, degreesPerSecond = 6f, degreesPerDayCycle = 15f;
    private DateTime timeDiscrete;
    private TimeSpan timeContinuous;
    public bool continuous, realTime = true;
    public Transform dayCycle;
    [Range(0,24)]
    public int hours;

	// Use this for initialization
	void Awake ()
    {
        timeDiscrete = DateTime.Now;
        Debug.Log(DateTime.Now.TimeOfDay.Hours);
        hoursTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Hour * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Minute * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Second * degreesPerSecond, 0f);
        dayCycle.localRotation = Quaternion.Euler(timeDiscrete.TimeOfDay.Hours * degreesPerDayCycle, 0f, 0f);
    }

    private void Update()
    {
        timeDiscrete = DateTime.Now;
        hoursTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Hour * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Minute * degreesPerMinute, 0f);
        if (realTime)
        {
            dayCycle.localRotation = Quaternion.Euler(timeDiscrete.TimeOfDay.Hours * degreesPerDayCycle, 0f, 0f);
        }
        else
        {
            dayCycle.localRotation = Quaternion.Euler(hours * degreesPerDayCycle, 0f, 0f);
        }

        if (continuous)
        {
            UpdateContinuous();
        }
        else
        {
            UpdateDiscrete();
        }
    }

    // Update is called once per frame
    void UpdateContinuous ()
    {
        timeContinuous = DateTime.Now.TimeOfDay;
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)timeContinuous.TotalSeconds * degreesPerSecond, 0f);
    }

    void UpdateDiscrete()
    {
        secondsTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Second * degreesPerSecond, 0f);
    }
}
