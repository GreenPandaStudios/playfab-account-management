using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class RegisterWithEmailUsername : LoginDriver
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField emailInput;

    [SerializeField] TMP_InputField passwordInput;

    public string Email { get => emailInput.text; }
    public string Password { get => passwordInput.text; }
    public string Username { get => usernameInput.text; }

    public override bool AllFieldsCompleted()
    {
        return (emailInput.text.Contains("@") && emailInput.text.Contains(".") && 
            !string.IsNullOrEmpty(passwordInput.text) && Password.Length >= 8 
            && Username.Length >= 3 && Username.IsAlphaNum());
    }

    public override void SetLoggingInStatus(bool logginIn)
    {
        loginButton.interactable = !logginIn;
    }

}
