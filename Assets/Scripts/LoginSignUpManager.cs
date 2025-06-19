using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoginSignUpManager : MonoBehaviour
{
    //추후 로그인이나 에러 로그 띄울때 TA에셋 받아서 적용 및 error 코루틴에 적용 후 실행
    
    public GameObject startPanel;
    public GameObject loginPanel;
    public GameObject registerPanel;
    
    public GameObject errorObj;
    public TextMeshProUGUI errorText;
    
    public GameObject noticeObj;
    public TextMeshProUGUI noticeText;
    
    public TMP_InputField idInputField;
    public TMP_InputField passwordInputField;
    
    public TMP_InputField registerIdInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField emailVerifyCodeInputField;
    public TMP_InputField registerPasswordInputField;
    public TMP_InputField registerCheckPasswordInputField;

    private bool _isEmailVerified = false;


    void Awake()
    {
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        errorObj.SetActive(false);

        ResetLoginText();
        ResetRegisterText();
    }

    public void OnStartButtonClick()
    {
        GoToLoginPanel();
    }

    public void OnExitButtonClick()
    {
        GoToStartPanel();
        
        ResetLoginText();
        ResetRegisterText();
    }

    public void OnGoToSingUpButtonClick()
    {
        ResetLoginText();
        GoToRegisterPanel();
    }

    public void OnReturnLoginButtonClick()
    {
        ResetRegisterText();
        GoToLoginPanel();
    }
    
    public void OnLoginButtonClick()
    {
        string username = idInputField.text;
        string password = passwordInputField.text;

        if (username == "" || password == "")
        {
            
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
                    StartCoroutine(Error(message));
                    Debug.LogWarning($"Login failed (status = {status}): {message}");
                }

                ResetLoginText();
            }

        ));
    }

    public void OnSendEmailVerificationButtonClick()
    {
        string email = emailInputField.text;

        if (IsNull(email))
        {
            StartCoroutine(Error("email " + NotificationTexts.TextNullError));
            return;
        }

        StartCoroutine(Api_SendEmailVerification.Send(email, (status, message) =>
        {
            if (status)
            {
                StartCoroutine(Notice(message));
            }
            else
            {
                StartCoroutine(Error(message));
            }
        }));

    }

    public void OnVerifyEmailButtonClick()
    {
        string email = emailInputField.text;
        string verifyCode = emailVerifyCodeInputField.text;
        
        if (IsNull(email) || IsNull(verifyCode))
        {
            StartCoroutine(Error("email or verifyCode " + NotificationTexts.TextNullError));
            return;
        }

        StartCoroutine(Api_VerifyEmail.Send(email, verifyCode, (status, message) =>
        {
            if (status)
            {
                _isEmailVerified = true;
                StartCoroutine(Notice(message));
            }
            else
            {
                _isEmailVerified = false;
                StartCoroutine(Error(message));
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
            StartCoroutine(Error("All " + NotificationTexts.TextNullError));
            return;
        }

        if (!_isEmailVerified)
        {
            StartCoroutine(Error(NotificationTexts.EmailVerifyError));
            return;
        }
        
        if (password != checkPassword)
        {
            StartCoroutine(Error(NotificationTexts.PasswordError));
            return;
        }
        

        StartCoroutine(Api_SignUp.Send(username, password, email, (status, message) =>
        {
            Debug.Log(status);

            if (status)
            {
                Debug.Log("SignUp successful");
                StartCoroutine(Notice(message));
                //튜토리얼 씬으로 이동

            }
            else
            {
                StartCoroutine(Error(message));
                Debug.LogWarning($"Login failed (status = {status}): {message}");
            }
            
            ResetRegisterText();
        }));
    }

    public IEnumerator Error(string error)
    {
        errorText.text = error;
        errorObj.SetActive(true);
        
         yield return new WaitForSeconds(1f);
         
         errorObj.SetActive(false);
         ResetErrorText();
    }
    
    public IEnumerator Notice(string notice)
    {
        noticeText.text = notice;
        noticeObj.SetActive(true);
        
        yield return new WaitForSeconds(1f);
         
        noticeObj.SetActive(false);
        ResetNoticeText();
    }

    public bool IsNull(string text)
    {
        return string.IsNullOrEmpty(text);
    }
    
    #region ResetText
    private void ResetLoginText()
    {
        idInputField.text = "";
        passwordInputField.text = "";
    }

    private void ResetRegisterText()
    {
        registerIdInputField.text = "";
        emailInputField.text = "";
        emailVerifyCodeInputField.text = "";
        registerPasswordInputField.text = "";
        registerCheckPasswordInputField.text = "";
    }

    private void ResetErrorText()
    {
        errorText.text = "";
    }
    
    private void ResetNoticeText()
    {
        noticeText.text = "";
    }
    
    #endregion

    #region GoTo
    
    private void GoToStartPanel()
    {
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
    }

    private void GoToLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        startPanel.SetActive(false);
    }
    
    private void GoToRegisterPanel()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
        startPanel.SetActive(false);
    }
    
    #endregion
    
}
