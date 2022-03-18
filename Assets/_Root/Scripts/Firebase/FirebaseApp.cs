using Gamee_Hiukka.Control;
using Gamee_Hiukka.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseApp : Singleton<FirebaseApp>
{
    public RemoteConfig remoteConfig;
    public FirebaseCloundMessage firebaseCloundMessege;

    Firebase.FirebaseApp instance;

    public Firebase.FirebaseApp Instance => instance;
    public void Init()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("[Firebase] init completed!");
                instance = Firebase.FirebaseApp.DefaultInstance;

                remoteConfig.Init();
                firebaseCloundMessege.Init();
            }
            else
            {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
}
