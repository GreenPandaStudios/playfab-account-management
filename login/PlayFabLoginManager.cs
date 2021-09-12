using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using System.Text;
public class PlayFabLoginManager : MonoBehaviour
{

    [Header("Title indormation")]
    [Tooltip("Set from the PlayFab GameManager site")]
    [SerializeField] string titleId = "";

    private static string userIdKey = "UserID";

    public static string USER_ID_KEY { get => userIdKey; }


    public static Action<UserAccountInfo> AccontInfoChange;

    public static event Action<LoginResult> OnLogin;

    // Start is called before the first frame update
    void Start()
    {
        
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = titleId;
        }
        




        
    }

    /// <summary>
    /// Links the account to a custom user id, saves this to the device
    /// and then calls the OnLogin event
    /// </summary>
    private void FinalizeLogin(LoginResult result, bool rememberDevice)
    {
        if (!rememberDevice) { currentDriver.InvokeLoginSuccess();  OnLogin?.Invoke(result); return; }
        //generate a new userID
        var _id = AccountData.GenerateNewRandomID();
        PlayerPrefs.SetString(USER_ID_KEY,_id);
        PlayerPrefs.Save();
        currentDriver.Display("Remembering your device...");

        PlayFabClientAPI.LinkCustomID(new LinkCustomIDRequest
        {
            CustomId = _id
        },
           (_) =>
           {
               currentDriver.InvokeLoginSuccess();
               OnLogin?.Invoke(result);
           }
        ,(error)=>
        {
            Debug.LogError(error.GenerateErrorReport());
            currentDriver.Display("We were unable to remeber your device." +
            "\nYou must log in again manually next time");   
        });
    }


    private static LoginDriver currentDriver = null;
    /// <summary>
    /// This is the ONLY safe method for logging in to playfab
    /// Provide an implmentation of <see cref="LoginDriver"/> and set the appropriate
    /// <see cref="LoginDriver.LoginType"/>
    /// </summary>
    /// <param name="driver"></param>
    
   public void LogInWithDriver(LoginDriver driver)
    {
        currentDriver = driver;
        switch (driver.Type)
        {
            case LoginDriver.LoginType.LOGIN_WITH_SAVED_TOKEN:
                LoginWithSavedToken((driver as LoginWithTokenDriver));
                break;
            case LoginDriver.LoginType.LOGIN_WITH_EMAIL:
                LoginWithEmail((driver as LoginWithEmailDriver));
                break;
            case LoginDriver.LoginType.REGISTER_WITH_EMAIL_USERNAME:
                RegisterWithEmailUsername((driver as RegisterWithEmailUsername));
                break;
            default: driver.InvokeLoginFailure();  currentDriver = null;  break;


        }
    }
    void OnError(string error)
    {
        if (currentDriver)
        {
            currentDriver.Display(error);
        }
        Debug.LogError(error);
    }

    private void LoginWithSavedToken(LoginWithTokenDriver tokenDriver)
    {
        tokenDriver.SetLoggingInStatus(true);

        //login with a token, may be saved or provided, id
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CreateAccount = true,
            CustomId = tokenDriver.GetToken(),
            TitleId = titleId
        },
        (result) =>
        {
            tokenDriver.SetLoggingInStatus(false);
            currentDriver.Display("Logged in under token");
            FinalizeLogin(result, tokenDriver.RememberDevice);
        },
        (error) => {
            tokenDriver.SetLoggingInStatus(false);
            OnError(error.GenerateErrorReport());
        });

    }

    private void RegisterWithEmailUsername(RegisterWithEmailUsername registerWithUsernameEmailDriver)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Email = registerWithUsernameEmailDriver.Email,
            Password = registerWithUsernameEmailDriver.Password,
            Username = registerWithUsernameEmailDriver.Username,
            TitleId = titleId
        },
        (result) =>
        {
            registerWithUsernameEmailDriver.SetLoggingInStatus(false);
            currentDriver.Display("Account created with email address");
            FinalizeLogin(
                //wrap the result as a login result
                new LoginResult
                {

                    PlayFabId = result.PlayFabId,
                    NewlyCreated = true,
                    SettingsForUser = result.SettingsForUser,
                    AuthenticationContext = result.AuthenticationContext,
                    CustomData = result.CustomData,
                    SessionTicket = result.SessionTicket,
                    EntityToken = result.EntityToken,

                },
            registerWithUsernameEmailDriver.RememberDevice);
        }, (error) => {
            registerWithUsernameEmailDriver.InvokeLoginFailure();
            registerWithUsernameEmailDriver.SetLoggingInStatus(false);
            OnError(error.GenerateErrorReport());
        });
    }

    private void LoginWithEmail(LoginWithEmailDriver emailDriver)
    {

        emailDriver.SetLoggingInStatus(true);

        PlayFabClientAPI.LoginWithEmailAddress(
            new LoginWithEmailAddressRequest
            {

                Email = emailDriver.Email,
                Password = emailDriver.Password,

                TitleId = titleId
            },
            (result) =>
            {
                emailDriver.SetLoggingInStatus(false);
                currentDriver.Display("Logged in with email");
                FinalizeLogin(result, emailDriver.RememberDevice);
            },

                (error) =>
                {
                    emailDriver.InvokeLoginFailure();
                    emailDriver.SetLoggingInStatus(false);
                    OnError(error.GenerateErrorReport());


                }
            );

    }

    void OnError(PlayFabError error) {
        OnError("Something went wrong, check your internet connection and try again.");
        Debug.LogError(error.GenerateErrorReport());
        
    }
}
