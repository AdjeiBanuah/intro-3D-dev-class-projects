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
    [Range(0,23)]
    public int hours;

    //update hours to float and dayCycle.localRotation to continuous time
    //update non-realtime to change clock as well
    //move functionality to dragging on clock hands
    //set physics layers so dont delete parts of clock with left click
    //change paint feature to left click so it works on mobile version

	// Use this for initialization
	void Awake ()
    {
        timeDiscrete = DateTime.Now;
        Debug.Log(DateTime.Now.TimeOfDay.Hours);
        hoursTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Hour * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Minute * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Second * degreesPerSecond, 0f);
        dayCycle.localRotation = Quaternion.Euler(timeDiscrete.TimeOfDay.Hours * degreesPerDayCycle, 0f, 0f);
        hours = timeDiscrete.TimeOfDay.Hours;
    }

    private void Update()
    {
        timeContinuous = DateTime.Now.TimeOfDay;
        hoursTransform.localRotation = Quaternion.Euler(0f, (float)timeContinuous.TotalHours * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, (float)timeContinuous.TotalMinutes * degreesPerMinute, 0f);
        if (realTime)
        {
            hours = timeDiscrete.TimeOfDay.Hours;
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
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)timeContinuous.TotalSeconds * degreesPerSecond, 0f);
    }

    void UpdateDiscrete()
    {
        timeDiscrete = DateTime.Now;
        secondsTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Second * degreesPerSecond, 0f);
    }
}
