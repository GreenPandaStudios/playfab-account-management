using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LoginWithEmailDriver : LoginDriver
{
    [SerializeField] TMP_InputField emailInput;
   
    [SerializeField] TMP_InputField passwordInput;
    
    public string Email { get => emailInput.text; }
    public string Password { get => passwordInput.text; }

    public override bool AllFieldsCompleted()
    {
        return (emailInput.text.Contains("@") && emailInput.text.Contains(".")
            && !string.IsNullOrEmpty(passwordInput.text)
            && Password.Length >= 8);
    }

    public override void SetLoggingInStatus(bool logginIn)
    {
        loginButton.interactable = !logginIn;
    }
}
