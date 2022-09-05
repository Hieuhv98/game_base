using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamee_Hiukka.Data;
using System;
using Notification;

public class CustomNotiTime : MonoBehaviour
{
    public NotificationConsole console;

    public void SendNotification() 
    {
        var timeToNewDay = (int)( new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1) - DateTime.Now).TotalMinutes;
        if(timeToNewDay > 0) console.UpdateDeliveryTimeByIncremental("channel_noti", 0, timeToNewDay);
    }
}
