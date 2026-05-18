using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

<<<<<<< Updated upstream
    private List<string> minijocsOriginals = new List<string> { "swipe1", "drag1", "wipe1", "wipe2", "retrack", "drag2" };
    private List<string> cuaMinijocs = new List<string>();
=======
    [Header("Minijocs")]
    [SerializeField] private List<string> minijocsOriginals = new List<string>()
    {
        "swipe1",
        "drag1",
        "wipe1",
        "wipe2",
        "retrack",
        "drag2"
    };

    [SerializeField] private List<MinijocConfig> configuracionsMinijocs =
        new List<MinijocConfig>();

    private List<string> cuaMinijocs = new List<string>();

    private string minijocActual;

    [Header("Partida")]
    [SerializeField] private int videsInicials = 3;

    [SerializeField] private int pujarDificultatCada = 3;

    private int videsActuals;

    private int dificultatActual = 1;

    private int minijocsCompletats = 0;

    private int minijocsJugats = 0;

    private bool partidaIniciada = false;
>>>>>>> Stashed changes

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TextMeshProUGUI scoreText;

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

<<<<<<< Updated upstream
    void PrepararCua()
=======
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas.ForceUpdateCanvases();

        if (gameOverPanel != null)
        {
            RectTransform rect =
                gameOverPanel.GetComponent<RectTransform>();

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            rect.localScale = Vector3.one;
        }
    }

    private void PrepararCua()
>>>>>>> Stashed changes
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
        if (!partidaIniciada)
        {
            IniciarPartida();

            return;
        }

>>>>>>> Stashed changes
        if (cuaMinijocs.Count == 0)
        {
            PrepararCua();
        }

<<<<<<< Updated upstream
        string seguent = cuaMinijocs[0];
        cuaMinijocs.RemoveAt(0);

        SceneManager.LoadScene(seguent);
=======
        minijocActual = cuaMinijocs[0];

        cuaMinijocs.RemoveAt(0);

        Debug.Log("Carregant minijoc: " + minijocActual);

        SceneManager.LoadScene(minijocActual);
    }

    public void MinijocGuanyat()
    {
        minijocsCompletats++;

        minijocsJugats++;

        Debug.Log(
            "Minijoc guanyat. Completats: " +
            minijocsCompletats
        );

        ComprovarPujadaDificultat();

        CarregarSeguentMinijoc();
    }

    public void MinijocPerdut()
    {
        videsActuals--;

        minijocsJugats++;

        Debug.Log(
            "Minijoc perdut. Vides restants: " +
            videsActuals
        );

        if (videsActuals <= 0)
        {
            FiPartida();
        }
        else
        {
            ComprovarPujadaDificultat();

            CarregarSeguentMinijoc();
        }
    }

    private void ComprovarPujadaDificultat()
    {
        if (
            minijocsJugats > 0 &&
            minijocsJugats % pujarDificultatCada == 0
        )
        {
            dificultatActual++;

            Debug.Log(
                "Dificultat pujada a nivell: " +
                dificultatActual
            );
        }
    }

    private void FiPartida()
    {
        partidaIniciada = false;
        
        RankingManager.SaveScore(minijocsCompletats);

        Debug.Log("Fi de partida.");

        Debug.Log(
            "Minijocs completats: " +
            minijocsCompletats
        );

        Debug.Log(
            "Dificultat final: " +
            dificultatActual
        );

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (scoreText != null)
        {
            scoreText.text =
                "Puntuación: " +
                minijocsCompletats;
        }

        Time.timeScale = 0f;
    }

    public void ReiniciarPartida()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        IniciarPartida();
>>>>>>> Stashed changes
    }

    public void TornarMenu()
    {
<<<<<<< Updated upstream
        SceneManager.LoadScene("MainMenu");
    }
=======
        Time.timeScale = 1f;

        partidaIniciada = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        SceneManager.LoadScene("MainMenu");
    }

    public MinijocRuntimeConfig GetConfigActual()
    {
        MinijocConfig configBase =
            BuscarConfigMinijoc(minijocActual);

        float tempsCalculat =
            configBase.tempsBase -
            (
                (dificultatActual - 1) *
                configBase.reduccioTempsPerDificultat
            );

        tempsCalculat =
            Mathf.Max(
                tempsCalculat,
                configBase.tempsMinim
            );

        float velocitatCalculada =
            configBase.velocitatBase +
            (
                (dificultatActual - 1) *
                configBase.incrementVelocitatPerDificultat
            );

        velocitatCalculada =
            Mathf.Min(
                velocitatCalculada,
                configBase.velocitatMaxima
            );

        int errorsCalculats =
            configBase.errorsPermesosBase -
            (
                (dificultatActual - 1) /
                configBase.baixarErrorsCadaNivells
            );

        errorsCalculats =
            Mathf.Max(
                errorsCalculats,
                configBase.errorsMinims
            );

        return new MinijocRuntimeConfig(
            minijocActual,
            tempsCalculat,
            velocitatCalculada,
            errorsCalculats,
            dificultatActual,
            videsActuals,
            minijocsCompletats,
            minijocsJugats
        );
    }

    private MinijocConfig BuscarConfigMinijoc(
        string nomEscena
    )
    {
        for (int i = 0; i < configuracionsMinijocs.Count; i++)
        {
            if (
                configuracionsMinijocs[i].nomEscena ==
                nomEscena
            )
            {
                return configuracionsMinijocs[i];
            }
        }

        Debug.LogWarning(
            "No s'ha trobat configuració per al minijoc: " +
            nomEscena +
            ". S'utilitzarà una configuració per defecte."
        );

        MinijocConfig configDefecte =
            new MinijocConfig();

        configDefecte.nomEscena = nomEscena;

        return configDefecte;
    }

    public string GetMinijocActual()
    {
        return minijocActual;
    }

    public int GetVidesActuals()
    {
        return videsActuals;
    }

    public int GetDificultatActual()
    {
        return dificultatActual;
    }

    public int GetMinijocsCompletats()
    {
        return minijocsCompletats;
    }
}

[System.Serializable]
public class MinijocConfig
{
    [Header("Identificació")]
    public string nomEscena;

    [Header("Temps")]
    public float tempsBase = 10f;

    public float tempsMinim = 5f;

    public float reduccioTempsPerDificultat = 0f;

    [Header("Velocitat")]
    public float velocitatBase = 1f;

    public float velocitatMaxima = 8f;

    public float incrementVelocitatPerDificultat = 0.25f;

    [Header("Errors")]
    public int errorsPermesosBase = 3;

    public int errorsMinims = 1;

    public int baixarErrorsCadaNivells = 2;
}

public class MinijocRuntimeConfig
{
    public string nomEscena;

    public float temps;

    public float velocitat;

    public int errorsPermesos;

    public int dificultat;

    public int vides;

    public int minijocsCompletats;

    public int minijocsJugats;

    public MinijocRuntimeConfig(
        string nomEscena,
        float temps,
        float velocitat,
        int errorsPermesos,
        int dificultat,
        int vides,
        int minijocsCompletats,
        int minijocsJugats
    )
    {
        this.nomEscena = nomEscena;

        this.temps = temps;

        this.velocitat = velocitat;

        this.errorsPermesos = errorsPermesos;

        this.dificultat = dificultat;

        this.vides = vides;

        this.minijocsCompletats = minijocsCompletats;

        this.minijocsJugats = minijocsJugats;
    }
>>>>>>> Stashed changes
}