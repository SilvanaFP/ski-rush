using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
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