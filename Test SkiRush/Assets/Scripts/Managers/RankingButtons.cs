using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingButtons : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}