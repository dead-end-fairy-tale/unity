using UnityEngine;
using UnityEngine.Serialization;

public class StartAccountUIManager : MonoBehaviour
{
    [FormerlySerializedAs("loginManager")] [SerializeField] private LoginAndResetPasswordManager loginAndResetPasswordManager;
    [SerializeField] private SignUpManager signUpManager;
    
    [Header("Panels")]
    public GameObject startPanel;
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject resetPasswordPanel;
    public GameObject findIDPanel;

    void Awake()
    {
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        findIDPanel.SetActive(false);
    }

    public void OnStartButtonClick()
    {
        GoToLoginPanel();
    }

    public void OnExitButtonClick()
    {
        GoToStartPanel();
        
        loginAndResetPasswordManager.ResetLoginTextAndAll();
        signUpManager.ResetRegisterText();
    }

    public void OnGoToSingUpButtonClick()
    {
        loginAndResetPasswordManager.ResetLoginTextAndAll();
        GoToRegisterPanel();
    }

    public void OnReturnLoginButtonClick()
    {
        signUpManager.ResetRegisterText();
        loginAndResetPasswordManager.ResetLoginTextAndAll();
        GoToLoginPanel();
    }
    
    public void OnCantRememberPasswordButtonClick()
    {
        loginAndResetPasswordManager.ResetLoginTextAndAll();
        GoToResetPasswordPanel();
    }

    public void OnCantRememberIdButtonClick()
    {
        loginAndResetPasswordManager.ResetLoginTextAndAll();
        GoToFindIDPanel();
    }
    

    
    #region GoTo
    
    private void GoToStartPanel()
    {
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        findIDPanel.SetActive(false);
    }

    private void GoToLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        startPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        findIDPanel.SetActive(false);
    }
    
    private void GoToRegisterPanel()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
        startPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        findIDPanel.SetActive(false);
    }

    private void GoToResetPasswordPanel()
    {
        resetPasswordPanel.SetActive(true);
        registerPanel.SetActive(false);
        loginPanel.SetActive(false);
        startPanel.SetActive(false);
        findIDPanel.SetActive(false);
    }

    private void GoToFindIDPanel()
    {
        findIDPanel.SetActive(true);
        resetPasswordPanel.SetActive(false);
        registerPanel.SetActive(false);
        loginPanel.SetActive(false);
        startPanel.SetActive(false);
    }
    
    #endregion
}
