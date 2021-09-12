using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Contains the base functionality for all login drivers
/// Login Drivers MUST implement this class
/// </summary>
public abstract class LoginDriver : MonoBehaviour
{
    /// <summary>
    /// Called when the login was a success. Use for scene-specific hooks. For code hooks see
    /// <seealso cref="PlayFabLoginManager.OnLogin"/>
    /// </summary>
    [Tooltip("Called when the login was a success")]

    [SerializeField] UnityEvent loginSuccess;
    /// <summary>
    /// <see cref="loginSuccess"/>
    /// </summary>
    public void InvokeLoginSuccess() => loginSuccess?.Invoke();
    /// <summary>
    /// Called when the login was a success. Use for scene-specific hooks. For code hooks see
    /// </summary>
    [Tooltip("Called when the login was a failure")]
    [SerializeField] UnityEvent loginFail;
    /// <summary>
    /// <see cref="loginFail"/>
    /// </summary>
    public void InvokeLoginFailure() => loginFail?.Invoke();

    public enum LoginType
    {
        LOGIN_WITH_EMAIL,
        LOGIN_WITH_SAVED_TOKEN,
        REGISTER_WITH_EMAIL_USERNAME,
    }




    [SerializeField] PlayFabLoginManager loginManager;
    [Tooltip("Should we associate this device with a login token?")]
    [SerializeField] bool rememberDevice;
    /// <summary>
    /// Should we associate this device with a login token?
    /// </summary>
    public bool RememberDevice { get => rememberDevice; }

    [Tooltip("What button do we press to start the login sequence?")]
    [SerializeField] protected Button loginButton;

    [Tooltip("What kind of login do we want to attempt?")]
    [SerializeField]
    LoginType loginType;

    [SerializeField]
    [Tooltip("Where should messages regarding the login be displayed?")]
    TextMeshProUGUI loginStatusDisplay;
    /// <summary>
    /// The type of login we are attempting to initiate
    /// </summary>
    public LoginType Type { get => loginType; }

    private void Awake()
    {
        if (loginButton)
            loginButton.onClick.AddListener(Login);
    }
    private void OnDestroy()
    {
        if (loginButton)
            loginButton.onClick.RemoveListener(Login);
    }

    public abstract void SetLoggingInStatus(bool logginIn);
    public void Login()
    {
        if (AllFieldsCompleted())
            loginManager.LogInWithDriver(this);
        else Display("Please complete all required fields");
    }


    public void Display(string text)
    {
        loginStatusDisplay.text = text;
    }

    /// <summary>
    /// Determine if all rewuired fields for login are completed
    /// </summary>
    /// <returns>true if all fields have been filled out, false otherwise</returns>
    public abstract bool AllFieldsCompleted();

}
