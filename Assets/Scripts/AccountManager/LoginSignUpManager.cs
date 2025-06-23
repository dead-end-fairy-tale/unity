using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoginSignUpManager : MonoBehaviour
{
    // //추후 로그인이나 에러 로그 띄울때 TA에셋 받아서 적용 및 error, notice에 적용 후 실행
    //
    // [Header("Login")]
    // public TMP_InputField idInputField;
    // public TMP_InputField passwordInputField;
    //
    // [Header("Reset Password")]
    // public TMP_InputField resetPasswordEmailInputField;
    // public TMP_InputField resetPasswordVerifyCodeInputField;
    //
    // [Header("SignUp")]
    // public TMP_InputField registerIdInputField;
    // public TMP_InputField emailInputField;
    // public TMP_InputField emailVerifyCodeInputField;
    // public TMP_InputField registerPasswordInputField;
    // public TMP_InputField registerCheckPasswordInputField;
    //
    // private bool _isEmailVerified = false;
    //
    //
    // void Awake()
    // {
    //     ResetLoginText();
    //     ResetRegisterText();
    // }
    //
    //
    // public void OnLoginButtonClick()
    // {
    //     string username = idInputField.text;
    //     string password = passwordInputField.text;
    //
    //     if (IsNull(username) || IsNull(password))
    //     {
    //         AlertSystem.Instance.Error("All " + NotificationTexts.TextNullError);
    //         return;
    //     }
    //
    //     StartCoroutine(Api_Login.Send(username, password, (status, message) =>
    //         {
    //             Debug.Log(status);
    //
    //             if (status)
    //             {
    //                 Debug.Log("Login successful");
    //                 //로비씬으로 이동
    //             }
    //             else
    //             {
    //                 AlertSystem.Instance.Error(message);
    //                 Debug.LogWarning($"Login failed (status = {status}): {message}");
    //             }
    //
    //             ResetLoginText();
    //         }
    //
    //     ));
    // }
    //
    // public void OnFindPasswordButtonClick()
    // {
    //     string email = resetPasswordEmailInputField.text;
    // }
    //
    // public void OnSendEmailVerificationButtonClick()
    // {
    //     string email = emailInputField.text;
    //
    //     if (IsNull(email))
    //     {
    //         AlertSystem.Instance.Error("email " + NotificationTexts.TextNullError);
    //         return;
    //     }
    //
    //     StartCoroutine(Api_SendEmailVerification.Send(email, (status, message) =>
    //     {
    //         if (status)
    //         {
    //             AlertSystem.Instance.Notice(message);
    //         }
    //         else
    //         {
    //             AlertSystem.Instance.Error(message);
    //         }
    //     }));
    //
    // }
    //
    // public void OnVerifyEmailButtonClick()
    // {
    //     string email = emailInputField.text;
    //     string verifyCode = emailVerifyCodeInputField.text;
    //     
    //     if (IsNull(email) || IsNull(verifyCode))
    //     {
    //         AlertSystem.Instance.Error("email or verifyCode " + NotificationTexts.TextNullError);
    //         return;
    //     }
    //
    //     StartCoroutine(Api_VerifyEmail.Send(email, verifyCode, (status, message) =>
    //     {
    //         if (status)
    //         {
    //             _isEmailVerified = true;
    //             AlertSystem.Instance.Notice(message);
    //         }
    //         else
    //         {
    //             _isEmailVerified = false;
    //             AlertSystem.Instance.Error(message);
    //         }
    //     }));
    // }
    //
    //
    // public void OnSignUpButtonClick()
    // {
    //     string username = registerIdInputField.text;
    //     string email = emailInputField.text;
    //     string password = registerPasswordInputField.text;
    //     string checkPassword = registerCheckPasswordInputField.text;
    //
    //     if (IsNull(email) || IsNull(username) || IsNull(password) || IsNull(checkPassword))
    //     {
    //         AlertSystem.Instance.Error("All " + NotificationTexts.TextNullError);
    //         return;
    //     }
    //
    //     if (!_isEmailVerified)
    //     {
    //         AlertSystem.Instance.Error(NotificationTexts.EmailVerifyError);
    //         return;
    //     }
    //     
    //     if (password != checkPassword)
    //     {
    //         AlertSystem.Instance.Error(NotificationTexts.PasswordError);
    //         return;
    //     }
    //     
    //
    //     StartCoroutine(Api_SignUp.Send(username, password, email, (status, message) =>
    //     {
    //         Debug.Log(status);
    //
    //         if (status)
    //         {
    //             Debug.Log("SignUp successful");
    //             AlertSystem.Instance.Notice(message);
    //             //튜토리얼 씬으로 이동
    //
    //         }
    //         else
    //         {
    //             AlertSystem.Instance.Error(message);
    //             Debug.LogWarning($"Login failed (status = {status}): {message}");
    //         }
    //         
    //         ResetRegisterText();
    //     }));
    // }
    //
    //
    // public bool IsNull(string text)
    // {
    //     return string.IsNullOrEmpty(text);
    // }
    //
    // #region ResetText
    // public void ResetLoginText()
    // {
    //     idInputField.text = "";
    //     passwordInputField.text = "";
    // }
    //
    // public void ResetRegisterText()
    // {
    //     registerIdInputField.text = "";
    //     emailInputField.text = "";
    //     emailVerifyCodeInputField.text = "";
    //     registerPasswordInputField.text = "";
    //     registerCheckPasswordInputField.text = "";
    // }
    //
    // public void ResetResetPasswordEmailText()
    // {
    //     resetPasswordEmailInputField.text = "";
    //     resetPasswordVerifyCodeInputField.text = "";
    // }
    //
    // #endregion
    //
    //
    //
}
