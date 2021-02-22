using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{

    public Transform hoursTransform, minutesTransform, secondsTransform;
    const float degreesPerHour = 30f, degreesPerMinute = 6f, degreesPerSecond = 6f, degreesPerDayCycle = 15f;
    private DateTime timeDiscrete;
    private TimeSpan timeContinuous;
    public bool continuous = true;
    public Transform dayCycle;

    public Text textAMPM, projectText;
    private bool isAM;

    //[Range(0,23)]
    //public int hours;

    private float hours, minutes, hoursTemp;

    //update hours to float and dayCycle.localRotation to continuous time
    //update non-realtime to change clock as well
    //move functionality to dragging on clock hands
    //set physics layers so dont delete parts of clock with left click
    //change paint feature to left click so it works on mobile version

	// Use this for initialization

	void Awake ()
    {
        
        timeDiscrete = DateTime.Now;
        timeContinuous = DateTime.Now.TimeOfDay;
        hours = (float)timeContinuous.TotalHours;
        minutes = (float)timeContinuous.TotalMinutes;
        hoursTransform.localRotation = Quaternion.Euler(0f, hours * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, minutes * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)timeContinuous.TotalSeconds * degreesPerSecond, 0f);
        dayCycle.localRotation = Quaternion.Euler(hours * degreesPerDayCycle, 0f, 0f);

        if (hours < 12)
        {
            isAM = true;
            textAMPM.text = "AM";
        }
        else
        {
            isAM = false;
            textAMPM.text = "PM";
        }
        if(hours < 6 || hours > 18) //no sun, dark out
        {
            projectText.color = Color.white;
        }
        else //sun is out, text should be black
        {
            projectText.color = Color.black;
        }
    }

    private void Update()
    {
        timeContinuous = DateTime.Now.TimeOfDay;
        //hours = (float)timeContinuous.TotalHours; //commented out for UpdateAMPM
        minutes = (float)timeContinuous.TotalMinutes;
        
        minutesTransform.localRotation = Quaternion.Euler(0f, minutes * degreesPerMinute, 0f);
        //Debug.Log(hours);
        //hours = (float)timeContinuous.TotalHours;
        hoursTransform.localRotation = Quaternion.Euler(0f, hours * degreesPerHour, 0f);
        dayCycle.localRotation = Quaternion.Euler(hours * degreesPerDayCycle, 0f, 0f); //divide by two because its 24 hrs per rotation not 12 hrs

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

    public void UpdateTime(float clickedHourRotation) //click on an hour indicator
    {
        //if (clickedHourRotation < 0) clickedHourRotation += 360;
        //realTime = false;
        hoursTemp = ((float)timeContinuous.TotalHours - (int)timeContinuous.TotalHours) + (clickedHourRotation / degreesPerHour);
        if (!isAM) hoursTemp += 12f;
        if((int)hoursTemp < (int)hours)
        {
            hours = hoursTemp;
            UpdateAMPM();
        }
        else hours = hoursTemp;

        //userSetHour = partialHourOffset + clickedHourRotation;
        //dayCycleOffset = dayCycleOffset + (clickedHourRotation - dayCycleOffset);
    }

    public void UpdateAMPM()
    {
        if(isAM) //AM to PM
        {
            isAM = false;
            textAMPM.text = "PM";
            hours += 12f;
            projectText.color = Color.black;
        }
        else //PM to AM
        {
            isAM = true;
            textAMPM.text = "AM";
            hours -= 12f;
            projectText.color = Color.white;
        }
    }

    public float GetHours() //reading a private variable
    {
        return hours;
    }

    public void SetHours(float temp) //writing to a private variable
    {
        hours = temp;
    }
}
