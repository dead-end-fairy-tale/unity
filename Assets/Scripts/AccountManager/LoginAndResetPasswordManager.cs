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
    public TMP_InputField resetPasswordVerifyCodeInputField;
    
    private bool _isEmailVerified = false;
    private string _verifiedEmail;

    private void Awake()
    {
        ResetLoginText();
        ResetResetPasswordEmailText();
    }
    
    
    public void OnSendEmailVerificationButtonClick()
    {
        string email = resetPasswordEmailInputField.text;

        if (IsNull(email))
        {
            AlertSystem.Instance.Error("email " + NotificationTexts.TextNullError);
            return;
        }

        StartCoroutine(API_SendEmailVerification.Send(email, (status, message) =>
        {
            if (status)
            {
                AlertSystem.Instance.Notice(message);
                _verifiedEmail = email;
            }
            else
            {
                AlertSystem.Instance.Error(message);
                _verifiedEmail = null;
            }
        }));

    }

    public void OnVerifyEmailButtonClick()
    {
        string email = resetPasswordEmailInputField.text;
        string verifyCode = resetPasswordVerifyCodeInputField.text;

        if (IsNull(email) || IsNull(verifyCode))
        {
            AlertSystem.Instance.Error("email or verifyCode " + NotificationTexts.TextNullError);
            return;
        }

        StartCoroutine(API_VerifyEmail.Send(email, verifyCode, (status, message) =>
        {
            if (status)
            {
                _isEmailVerified = true;
                AlertSystem.Instance.Notice(message);
            }
            else
            {
                _isEmailVerified = false;
                AlertSystem.Instance.Error(message);
            }
        }));
    }

    public void OnFindPasswordButtonClick()
    {
        if (!_isEmailVerified)
        {
            AlertSystem.Instance.Error(NotificationTexts.EmailVerifyError);
            return;
        }

        string email = resetPasswordEmailInputField.text;

        if (email != _verifiedEmail)
        {
            _isEmailVerified = false;
            _verifiedEmail = null;
            AlertSystem.Instance.Error(NotificationTexts.verifiedEmailChanged);
            return;
        }

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
