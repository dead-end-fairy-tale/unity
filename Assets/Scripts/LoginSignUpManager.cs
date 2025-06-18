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
    
    public TMP_InputField idInputField;
    public TMP_InputField passwordInputField;
    
    public TMP_InputField registerIdInputField;
    public TMP_InputField registerPasswordInputField;
    public TMP_InputField registerCheckPasswordInputField;

    private string _passwordError = "Passwords must be same";


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

    public void OnLoginButtonClick()
    {
        string username = idInputField.text;
        string password = passwordInputField.text;

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
                    // StartCoroutine(Error(message));
                    Debug.LogWarning($"Login failed (status = {status}): {message}");
                }

                ResetLoginText();
            }

        ));
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

    public void OnSignUpButtonClick()
    {
        string username = registerIdInputField.text;
        string password = registerPasswordInputField.text;
        string checkPassword = registerCheckPasswordInputField.text;
        
        if (password != checkPassword)
        {
            StartCoroutine(Error(_passwordError));
            return;
        }
        

        StartCoroutine(Api_SignUp.Send(username, password, (status, message) =>
        {
            Debug.Log(status);

            if (status)
            {
                Debug.Log("SignUp successful");
                // StartCoroutine(SignUpComplete());
                //튜토리얼 씬으로 이동

            }
            else
            {
                // StartCoroutine(Error(message));
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
    
    public IEnumerator SignUpComplete()
    {
        //환영메세지? 키기
        
        yield return new WaitForSeconds(1f);
         
        //끄기
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
        registerPasswordInputField.text = "";
        registerCheckPasswordInputField.text = "";
    }

    private void ResetErrorText()
    {
        errorText.text = "";
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
