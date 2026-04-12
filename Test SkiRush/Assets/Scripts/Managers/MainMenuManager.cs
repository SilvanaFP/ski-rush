using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private string[] minijocs = { "swipe1", "drag1", "wipe1" };

    public void PlayGame()
    {
        GameFlowManager.Instance.CarregarSeguentMinijoc();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}