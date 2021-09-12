using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayFab;
using PlayFab.ClientModels;

public abstract class InitializeOnLogin
{
    
    public static void AddToInitializationQueue(Queue<InitializeOnLogin> queue)
    {
       
    }
    public static void Initialize(Action<string> Message, Action onCompleted = null, Action<PlayFabError> onFailed = null) { }
}
