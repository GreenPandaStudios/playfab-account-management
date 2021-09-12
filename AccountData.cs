using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayFab;
using PlayFab.ClientModels;
/*
using Photon.Pun;
using Newtonsoft.Json;
using Photon;
using ExitGames.Client.Photon;
*/
public class AccountData
{
    private static string playfabID;
    public static string PlayFabID
    {
        set
        {
            if (playfabID != value)
            {
                playfabID = value;
            }
        }
        get => playfabID;
    }
    private static string username;
    public static string Username
    {
        
        get {

            return username;
        }
    }
    private static string randomID = "null";

    /// <summary>
    /// Generates a new random id and stores it for global acces
    /// Can be used for random tokens
    /// <see cref="RandomID"/>
    /// </summary>
    public static string GenerateNewRandomID()
    {
        return randomID = System.Guid.NewGuid().ToString();
    }

    public static void SetDisplayName(string displayName, Action<UpdateUserTitleDisplayNameResult> result,
        Action<PlayFabError> error)
    {
        //usernames must be alphanumeric

        if (!displayName.IsAlphaNum())
        {
            error?.Invoke(new PlayFabError());
            return;
        }

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName =  displayName
        }, (_)=> { username = _.DisplayName; result?.Invoke(_); },
        error
        );
    }
   

    /// <summary>
    /// Updates the username, XP, rewards stats, and rank on the local machine
    /// MUST be called once on EVERY SIGN IN
    /// For the current signed-in player
    /// </summary>
    public static void InitializeAccountInfo(Action<string> Message,Action onUpdated = null, Action<PlayFabError> errorCallback = null)
    {
        //provide a queue for everything that needs to be initialized
        Queue<InitializeOnLogin> initializeOnLogins = new Queue<InitializeOnLogin>();
        

        //TODO: send an event out for all IntializeOnLoginObjects
       
    }



    public static event Action<string> OnUsernameChanged;
}
