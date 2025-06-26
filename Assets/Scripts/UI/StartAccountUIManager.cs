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

    void Awake()
    {
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
    }
    
    public void OnStartButtonClick()
    {
        GoToLoginPanel();
    }

    public void OnExitButtonClick()
    {
        GoToStartPanel();
        
        loginAndResetPasswordManager.ResetLoginText();
        signUpManager.ResetRegisterText();
    }

    public void OnGoToSingUpButtonClick()
    {
        loginAndResetPasswordManager.ResetLoginText();
        GoToRegisterPanel();
    }

    public void OnReturnLoginButtonClick()
    {
        signUpManager.ResetRegisterText();
        loginAndResetPasswordManager.ResetResetPasswordEmailText();
        GoToLoginPanel();
    }
    
    public void OnCantRememberPasswordButtonClick()
    {
        loginAndResetPasswordManager.ResetLoginText();
        GoToResetPasswordPanel();
    }
    

    
    #region GoTo
    
    private void GoToStartPanel()
    {
        startPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
    }

    private void GoToLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        startPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
    }
    
    private void GoToRegisterPanel()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
        startPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
    }

    private void GoToResetPasswordPanel()
    {
        resetPasswordPanel.SetActive(true);
        registerPanel.SetActive(false);
        loginPanel.SetActive(false);
        startPanel.SetActive(false);
    }
    
    #endregion
}
