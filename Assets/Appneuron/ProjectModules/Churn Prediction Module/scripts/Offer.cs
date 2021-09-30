using System;
using UnityEngine;
using UnityEngine.UI;

public class Offer : MonoBehaviour
{
    public Text FirstPrice;
    public Text NewPrice;
    public Image GiftImage;
    public Text LastTime;

    private int days;
    private int hours;
    private int minutes;
    private int seconds;

    private double LastDateTime;

    private void Start()
    {
        days = 0;
        hours = 0;
        minutes = 0;
        seconds = 0;
        LastDateTime = 0;
    }

    public void ChangeLastDateTime(DateTime startTime, DateTime endTime)
    {
        TimeSpan span = endTime.Subtract(startTime);
        LastDateTime = span.TotalSeconds;
    }

    void Update()
    {
        if (LastDateTime > 0)
        {
            LastDateTime -= Time.deltaTime;
            CalculateLastTimeForTextUI();
        }
    }

    private void CalculateLastTimeForTextUI()
    {

        days = Mathf.FloorToInt((float)(LastDateTime / 86400));
        if (days < 1)
        {
            days = 0;
            hours = Mathf.FloorToInt((float)(LastDateTime / 3600));
            if (hours < 1)
            {
                hours = 0;
                minutes = Mathf.FloorToInt((float)(LastDateTime / 60));
                seconds = Mathf.FloorToInt((float)(LastDateTime % 60));
                LastTime.text = $"{days}:{hours}:{minutes}:{seconds}";
                return;
            }
            minutes = Mathf.FloorToInt((float)(LastDateTime % 3600)) / 60;
            seconds = Mathf.FloorToInt((Mathf.FloorToInt((float)(LastDateTime % 3600)) % 60));
            LastTime.text = $"{days}:{hours}:{minutes}:{seconds}";
            return;
        }
        var LastedSecondFromDay = Mathf.FloorToInt((float)(LastDateTime % 86400));
        hours = Mathf.FloorToInt((LastedSecondFromDay / 3600)); 
        var LastedSecondFromHour = Mathf.FloorToInt((LastedSecondFromDay % 3600));
        minutes = Mathf.FloorToInt((LastedSecondFromHour / 60));
        seconds = Mathf.FloorToInt((LastedSecondFromHour % 60));
        LastTime.text = $"{days}:{hours}:{minutes}:{seconds}";
    }
}
