using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private LoginUI loginUI;

    public void PlayGame()
    {
        if (!SessionManager.IsLoggedIn())
        {
            if (loginUI != null)
            {
                loginUI.MostrarMissatge("Login/Register first");
            }

            Debug.Log("No es pot jugar sense iniciar sessió.");
            return;
        }

        GameFlowManager.Instance.IniciarPartida();
    }

    public void OpenRanking()
    {
        SceneManager.LoadScene("Ranking");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Sortir del Joc");
    }
}