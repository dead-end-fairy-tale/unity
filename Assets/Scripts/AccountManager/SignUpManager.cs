using TMPro;
using UnityEngine;

public class SignUpManager : MonoBehaviour, IAccountManager
{
    [Header("SignUp")] public TMP_InputField registerIdInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField emailVerifyCodeInputField;
    public TMP_InputField registerPasswordInputField;
    public TMP_InputField registerCheckPasswordInputField;

    private bool _isEmailVerified = false;

    void Awake()
    {
        ResetRegisterText();
    }

    public void OnSendEmailVerificationButtonClick()
    {
        string email = emailInputField.text;

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
            }
            else
            {
                AlertSystem.Instance.Error(message);
            }
        }));

    }

    public void OnVerifyEmailButtonClick()
    {
        string email = emailInputField.text;
        string verifyCode = emailVerifyCodeInputField.text;

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


    public void OnSignUpButtonClick()
    {
        string username = registerIdInputField.text;
        string email = emailInputField.text;
        string password = registerPasswordInputField.text;
        string checkPassword = registerCheckPasswordInputField.text;

        if (IsNull(email) || IsNull(username) || IsNull(password) || IsNull(checkPassword))
        {
            AlertSystem.Instance.Error("All " + NotificationTexts.TextNullError);
            return;
        }

        if (!_isEmailVerified)
        {
            AlertSystem.Instance.Error(NotificationTexts.EmailVerifyError);
            return;
        }

        if (password != checkPassword)
        {
            AlertSystem.Instance.Error(NotificationTexts.PasswordError);
            return;
        }


        StartCoroutine(API_SignUp.Send(username, password, email, (status, message) =>
        {
            Debug.Log(status);

            if (status)
            {
                Debug.Log("SignUp successful");
                AlertSystem.Instance.Notice(message);
                //튜토리얼 씬으로 이동

            }
            else
            {
                AlertSystem.Instance.Error(message);
                Debug.LogWarning($"Login failed (status = {status}): {message}");
            }

            ResetRegisterText();
        }));
    }


    public void ResetRegisterText()
    {
        registerIdInputField.text = "";
        emailInputField.text = "";
        emailVerifyCodeInputField.text = "";
        registerPasswordInputField.text = "";
        registerCheckPasswordInputField.text = "";
    }

    public bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
}
