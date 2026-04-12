using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<string> minijocs = new List<string>()
    {
        "swipe1",
        "drag1",
        "wipe1"
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        CarregarMinijocAleatori();
    }

    public void Win()
    {
        Debug.Log("Minijoc superat!");
        CarregarMinijocAleatori();
    }

    public void Lose()
    {
        Debug.Log("Has perdut partida!");
        SceneManager.LoadScene("MainMenu"); // o GameOver
    }

    private void CarregarMinijocAleatori()
    {
        int index = Random.Range(0, minijocs.Count);
        SceneManager.LoadScene(minijocs[index]);
    }
}