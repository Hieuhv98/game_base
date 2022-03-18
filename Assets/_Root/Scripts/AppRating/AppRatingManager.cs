using Gamee_Hiukka.Pattern;
using UnityEngine;

public class AppRatingManager : Singleton<AppRatingManager>
{
    private string appId = "com.gamee.huggytimepin";
    private string url;

    public void Start()
    {
        appId = Application.identifier;
#if UNITY_EDITOR
        url = "https://play.google.com/store/apps/details?id=" + appId;
#elif UNITY_ANDROID
        try 
        {
            url = "market://details?id=" + appId;
        }
        catch (System.Exception e)
        {
            url = "https://play.google.com/store/apps/details?id=" + appId;
        }
#elif UNITY_IOS
        try 
        {
            url = "market://details?id=" + appId;
        }
        catch (System.Exception e)
        {
            url = "https://apps.apple.com/us/app/id1562329957";
        }
#endif
    }

    public void OpenApp()
    {
        Application.OpenURL(url);
    }
}
