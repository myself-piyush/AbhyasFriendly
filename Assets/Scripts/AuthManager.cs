using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;
    public TextMeshProUGUI warn;

    public GameObject UI_Login;
    public GameObject UI_MainChat;

    //public TextMeshProUGUI UserEmail;
    //public GameObject AddButton;

    public GameObject AddChannel;
    public GameObject AddNotification;

    public TMP_Text userProfiletext;
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies" + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LoginButton()
    {

        StartCoroutine(Login(email.text, password.text));
    }

    public IEnumerator Login(string em, string pass)
    {
        GLOBAL.EMAILID = em;
        userProfiletext.text = GLOBAL.EMAILID;
        print("jjghgjhjgh");
        
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(em, pass);
        print("a" + LoginTask.IsCompleted);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        print("b" + LoginTask.IsCompleted);
        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;

                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;

                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;

                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;

                case AuthError.UserNotFound:
                    message = "User Not Found";
                    break;
            }
            warn.text = message;
        }
        else
        {
            Authorization();
            UI_Login.SetActive(false);
            UI_MainChat.SetActive(true);
        }
    }
    public void Authorization()
    {
        if(GLOBAL.EMAILID.Contains("teacher"))
        {
            AddNotification.SetActive(true);
            AddChannel.SetActive(false);
        }
        else if(GLOBAL.EMAILID.Contains("admin"))
        {
            AddNotification.SetActive(true);
            AddChannel.SetActive(true);
        }
        else
        {
            AddChannel.SetActive(false);
            AddNotification.SetActive(false);
        }
    }

}
