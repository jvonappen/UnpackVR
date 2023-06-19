using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    /*---Time Scale Info --------------------------*/
    private const int maxIRLSeconds = 86400;        /* IRL Day = 86400 seconds.     */
    [Tooltip("Real world seconds per in-game day")]
    [SerializeField] int daySpeed = 1;             /* IRL seconds per In-Game Day  */
    /*---------------------------------------------*/

    [SerializeField] private TMP_Text timeText;

    [Header("Debug - Current Time/Date")]
    [SerializeField] private double minute, hour, day;
    private double second;

    public double currentHour
    {
        get { return hour; }
        set { hour = value; }
    }

    void Start()
    {
        day = 1;
        hour = 9;
    }

    void Update()
    {
        CalculateTime();
    }

    void DisplayTime()
    {
        timeText.text = string.Format("{0:00}:{1:00}", hour, minute);
    }

    void CalculateTime()
    {
        int timescale = maxIRLSeconds / daySpeed;
        second += Time.deltaTime * timescale;

        if (second >= 60)
        {
            minute++;
            second = 0;
            DisplayTime();
        }
        else if (minute >= 60)
        {
            hour++;
            minute = 0;
            DisplayTime();
        }
        else if (hour >= 24)
        {
            day++;
            hour = 0;
            DisplayTime();
        }

    }
}

