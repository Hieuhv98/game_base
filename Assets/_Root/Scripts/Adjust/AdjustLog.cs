using com.adjust.sdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdjustLog
{
    public static void AdjustLogEventPurchaseItem(string token, double revenue, string currency, string transactionId)
    {
        AdjustEvent adjustEvent = new AdjustEvent(token);
        adjustEvent.setRevenue(revenue, currency);
        adjustEvent.setTransactionId(transactionId);

        Adjust.trackEvent(adjustEvent);
    }

    public static void AdjustLogEventFirstOpen()
    {
        AdjustEvent adjustEvent = new AdjustEvent("rcysq6");
        Adjust.trackEvent(adjustEvent);
    }

    public static void AdjustLogEventPlayLevel1()
    {
        AdjustEvent adjustEvent = new AdjustEvent("irnetq");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel2()
    {
        AdjustEvent adjustEvent = new AdjustEvent("17u1o7");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel3()
    {
        AdjustEvent adjustEvent = new AdjustEvent("dxfs0y");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel4()
    {
        AdjustEvent adjustEvent = new AdjustEvent("u8c9pq");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel5()
    {
        AdjustEvent adjustEvent = new AdjustEvent("5c5394");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel6()
    {
        AdjustEvent adjustEvent = new AdjustEvent("52valq");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel7()
    {
        AdjustEvent adjustEvent = new AdjustEvent("za33xz");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel8()
    {
        AdjustEvent adjustEvent = new AdjustEvent("6hxf97");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel9()
    {
        AdjustEvent adjustEvent = new AdjustEvent("y1kj4i");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel10()
    {
        AdjustEvent adjustEvent = new AdjustEvent("ftq785");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel15()
    {
        AdjustEvent adjustEvent = new AdjustEvent("kcv4gj");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel20()
    {
        AdjustEvent adjustEvent = new AdjustEvent("pptjwj");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel25()
    {
        AdjustEvent adjustEvent = new AdjustEvent("8qd820");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel30()
    {
        AdjustEvent adjustEvent = new AdjustEvent("bv897v");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel35()
    {
        AdjustEvent adjustEvent = new AdjustEvent("n6mlfk");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel40()
    {
        AdjustEvent adjustEvent = new AdjustEvent("itahwp");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel45()
    {
        AdjustEvent adjustEvent = new AdjustEvent("vizbjv");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventPlayLevel50()
    {
        AdjustEvent adjustEvent = new AdjustEvent("b3o8wl");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventDailyReward()
    {
        AdjustEvent adjustEvent = new AdjustEvent("du56bv");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventBuySkin20000()
    {
        AdjustEvent adjustEvent = new AdjustEvent("1xd5sg");
        Adjust.trackEvent(adjustEvent);
    }
    public static void AdjustLogEventBuySkin50000()
    {
        AdjustEvent adjustEvent = new AdjustEvent("dwf5da");
        Adjust.trackEvent(adjustEvent);
    }
}
    
