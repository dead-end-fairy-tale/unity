using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        StartCoroutine(API_FindID.Send(email, (status,message) =>
        {
            if (status)
            {
                AlertSystem.Instance.Notice($"Your ID: {message}");
            }
            else
            {
                AlertSystem.Instance.Notice(message);
            }
        }));
    }
    
    public void OnResetPasswordButtonClick()
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
                AlertSystem.Instance.Notice(message);
            }
        }));
    }

    public void OnLoginButtonClick()
    {
        string username = idInputField.text;
        string password = passwordInputField.text;

        if (IsNull(username) || IsNull(password))
        {
            AlertSystem.Instance.Notice("All " + NotificationTexts.TextNullError);
            return;
        }

        StartCoroutine(API_Login.Send(username, password, (status, message) =>
            {
                Debug.Log(status);

                if (status)
                {
                    Debug.Log("Login successful");
                    SceneManager.LoadScene("LobbyScene");
                }
                else
                {
                    AlertSystem.Instance.Notice(message);
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
