using TMPro;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject loggedPanel;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("Textos")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI loggedUserText;

    private void Start()
    {
        SessionManager.LoadSession();
        ActualitzarUI();
    }

    public void Login()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MostrarMissatge("Set user and password.");
            return;
        }

        MostrarMissatge("Logging in...");

        StartCoroutine(ApiClient.Instance.Login(
            username,
            password,
            user =>
            {
                SessionManager.SetSession(user.id, user.username);
                MostrarMissatge("Login correct.");
                ActualitzarUI();
            },
            error =>
            {
                MostrarMissatge(error);
            }
        ));
    }

    public void Register()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MostrarMissatge("Set user and password.");
            return;
        }

        MostrarMissatge("Creating user...");

        StartCoroutine(ApiClient.Instance.Register(
            username,
            password,
            user =>
            {
                SessionManager.SetSession(user.id, user.username);
                MostrarMissatge("User created successfuly.");
                ActualitzarUI();
            },
            error =>
            {
                MostrarMissatge(error);
            }
        ));
    }

    public void Logout()
    {
        SessionManager.ClearSession();
        MostrarMissatge("Log out correct.");
        ActualitzarUI();
    }

    public void MostrarMissatge(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }

        Debug.Log(message);
    }

    private void ActualitzarUI()
    {
        bool logged = SessionManager.IsLoggedIn();

        if (loginPanel != null)
        {
            loginPanel.SetActive(!logged);
        }

        if (loggedPanel != null)
        {
            loggedPanel.SetActive(logged);
        }

        if (loggedUserText != null)
        {
            loggedUserText.text = "Welcome, " + SessionManager.Username.ToUpper();
        }

        if (usernameInput != null)
        {
            usernameInput.text = "";
        }

        if (passwordInput != null)
        {
            passwordInput.text = "";
        }
    }
}