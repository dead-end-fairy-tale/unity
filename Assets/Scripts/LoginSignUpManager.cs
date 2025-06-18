using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoginSignUpManager : MonoBehaviour
{
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
        //로그인 메뉴 끄고 회원가입 메뉴 띄우기
        ResetLoginText();
        GoToRegisterPanel();
    }

    public void OnReturnLoginButtonClick()
    {
        //회원가입 메뉴 끄고 로그인 메뉴 띄우기
        ResetRegisterText();
        GoToLoginPanel();
    }

    public void OnSignUpButtonClick()
    {
        //password, password 확인 칸이 둘이 같은지 확인 후
        //true  -   다음로직으로 |    false   -   오류 메세지 팝업
        if (registerPasswordInputField.text != registerCheckPasswordInputField.text) 
            StartCoroutine(Error(_passwordError));
        
        
        //만일 이미 존재하는 id인지 확인이 필요하다면 서버에 보내서 확인
        //true  -   다음로직으로 |    false   -   오류 메세지 팝업
        
        // id, password 서버에 등록 
        
        // 튜토리얼 씬으로 이동
    }

    public IEnumerator Error(string error)
    {
        errorText.text = error;
        errorObj.SetActive(true);
        
         yield return new WaitForSeconds(1f);
         
         errorObj.SetActive(false);
         ResetErrorText();
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
