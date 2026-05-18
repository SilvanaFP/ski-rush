using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("Minijocs")]
    [SerializeField] private List<string> minijocsOriginals = new List<string>
    {
        "swipe1",
        "drag1",
        "wipe1",
        "wipe2",
        "retrack",
        "drag2"
    };

    [SerializeField] private List<MinijocConfig> configuracionsMinijocs = new List<MinijocConfig>();

    private List<string> cuaMinijocs = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

            PrepararCua();

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PrepararCua()
    {
        cuaMinijocs = new List<string>(minijocsOriginals);

        for (int i = 0; i < cuaMinijocs.Count; i++)
        {
            int randomIndex = Random.Range(i, cuaMinijocs.Count);

            string temp = cuaMinijocs[i];

            cuaMinijocs[i] = cuaMinijocs[randomIndex];

            cuaMinijocs[randomIndex] = temp;
        }
    }

    public void IniciarPartida()
    {
        videsActuals = videsInicials;
        dificultatActual = 1;
        minijocsCompletats = 0;
        minijocsJugats = 0;
        partidaIniciada = true;

        PrepararCua();
        CarregarSeguentMinijoc();
    }

<<<<<<< Updated upstream
    public void CarregarSeguentMinijoc()
    {
=======
    public void IniciarPartida()
    {
        Time.timeScale = 1f;

        videsActuals = videsInicials;

        dificultatActual = 1;

        minijocsCompletats = 0;

        minijocsJugats = 0;

        partidaIniciada = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        PrepararCua();

        CarregarSeguentMinijoc();
    }

    public void CarregarSeguentMinijoc()
    {
        if (cuaMinijocs.Count == 0)
        {
            PrepararCua();
        }

        string seguent = cuaMinijocs[0];
        cuaMinijocs.RemoveAt(0);

        SceneManager.LoadScene(seguent);
    }

    public void TornarMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}