using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoginWithTokenDriver : LoginDriver
{
    [SerializeField] string token;
    [SerializeField] bool loginAutomatically;

    private void OnEnable()
    {
        //see if there is a custom id saved
        if (PlayerPrefs.HasKey(PlayFabLoginManager.USER_ID_KEY) &&
            !string.IsNullOrEmpty(PlayerPrefs.GetString(PlayFabLoginManager.USER_ID_KEY)))
        {
            //this should be generated upon any succesful login
            Display("Welcome back!\nLogging you in now...");
            token = PlayFabLoginManager.USER_ID_KEY;
        }

        if (loginAutomatically) Login();
    }
    public virtual string GetToken()
    {
        return token;
    }
    public override bool AllFieldsCompleted()
    {
        return true;
    }

    public override void SetLoggingInStatus(bool logginIn)
    {
       //nothing for now
    }
}
