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

    [Header("Find ID")]
    public TMP_InputField findIDEmailInputField;
    
    private void Awake()
    { 
        ResetLoginTextAndAll();
    }
   
    public void OnFindIDButtonClick()
    {
        string email = findIDEmailInputField.text;

        StartCoroutine(API_FindID.Send(email, (status, message) =>
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
    
    public void OnFindPasswordButtonClick()
    {
        string email = resetPasswordEmailInputField.text;
        
        StartCoroutine(API_ResetPassword.Send(email, (status, message) =>
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

        StartCoroutine(API_Login.Send(username, password, (status, message) =>
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
    
    public void ResetLoginTextAndAll()
    {
        ResetLoginText();
        ResetResetPasswordEmailText();
        ResetFindIDText();
    }
    
    public void ResetLoginText()
    {
        idInputField.text = "";
        passwordInputField.text = "";
    }
    public void ResetFindIDText()
    {
        findIDEmailInputField.text = "";
    }
    public void ResetResetPasswordEmailText()
    {
        resetPasswordEmailInputField.text = "";
    }
    
    public bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}
