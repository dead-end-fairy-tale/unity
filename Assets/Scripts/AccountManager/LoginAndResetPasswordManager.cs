using System;
using TMPro;
using UnityEngine;

public class LoginAndResetPasswordManager : MonoBehaviour, IAccountManager
{
    [Header("Login")]
    public TMP_InputField idInputField;
    public TMP_InputField passwordInputField;
    
    [Header("Reset Password")]
    public TMP_InputField resetPasswordEmailInputField;

    private void Awake()
    {
        ResetLoginText();
        ResetResetPasswordEmailText();
    }
    

    public void OnFindPasswordButtonClick()
    {
        string email = resetPasswordEmailInputField.text;
        
        StartCoroutine(Api_ResetPassword.Send(email, (status, message) =>
        {
            if (status)
            {
                AlertSystem.Instance.Notice(message);
            }
            else
            {
                AlertSystem.Instance.Error(message);
            }
        }));
    }

    public void OnLoginButtonClick()
    {
        string username = idInputField.text;
        string password = passwordInputField.text;

        if (IsNull(username) || IsNull(password))
        {
            AlertSystem.Instance.Error("All " + NotificationTexts.TextNullError);
            return;
        }

        StartCoroutine(Api_Login.Send(username, password, (status, message) =>
            {
                Debug.Log(status);

                if (status)
                {
                    Debug.Log("Login successful");
                    //로비씬으로 이동
                }
                else
                {
                    AlertSystem.Instance.Error(message);
                    Debug.LogWarning($"Login failed (status = {status}): {message}");
                }

                ResetLoginText();
            }

        ));
    }


    public void ResetLoginText()
    {
        idInputField.text = "";
        passwordInputField.text = "";
    }
    public void ResetResetPasswordEmailText()
    {
        resetPasswordEmailInputField.text = "";
        resetPasswordVerifyCodeInputField.text = "";
    }
    
    public bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}
