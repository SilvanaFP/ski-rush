using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearGameManager : MonoBehaviour
{
    [Header("Peces")]
    public DraggableGear[] gears;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI timerText;
    public Image timeBarFill;

    [Header("Temps")]
    [SerializeField] private float timeLeft = 7f;
    private float tempsInicial;

    [Header("Transició")]
    [SerializeField] private float tempsEsperaDespresResultat = 1.5f;

    private bool gameEnded = false;

    private void Start()
    {
        Time.timeScale = 1f;
        gameEnded = false;

        if (GameFlowManager.Instance != null)
        {
            MinijocRuntimeConfig config = GameFlowManager.Instance.GetConfigActual();
            timeLeft = config.temps;

            Debug.Log("Config drag - Temps: " + config.temps +
                      " | Dificultat: " + config.dificultat +
                      " | Vides: " + config.vides);
        }

        tempsInicial = timeLeft;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        ActualitzarUI();
    }

    private void Update()
    {
        if (gameEnded) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            ActualitzarUI();
            LoseGame();
            return;
        }

        ActualitzarUI();
    }

    private void ActualitzarUI()
    {
        ActualitzarTimerText();
        ActualitzarBarraTemps();
    }

    private void ActualitzarTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void ActualitzarBarraTemps()
    {
        if (timeBarFill == null) return;
        if (tempsInicial <= 0f) return;

        timeBarFill.fillAmount = Mathf.Clamp01(timeLeft / tempsInicial);
    }

    public void CheckWin()
    {
        if (gameEnded) return;

        foreach (DraggableGear gear in gears)
        {
            if (!gear.isPlaced)
            {
                return;
            }
        }

        WinGame();
    }

    private void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Debug.Log("Minijoc drag completat!");

        Invoke(nameof(NotificarVictoriaAlGameManager), tempsEsperaDespresResultat);
    }

    private void LoseGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        bool mostrarDerrotaLocal =
            GameFlowManager.Instance == null ||
            GameFlowManager.Instance.GetVidesActuals() > 1;

        if (mostrarDerrotaLocal && losePanel != null)
        {
            losePanel.SetActive(true);
        }

        Debug.Log("Has perdut el minijoc drag!");

        Invoke(nameof(NotificarDerrotaAlGameManager), tempsEsperaDespresResultat);

    }

    private void NotificarVictoriaAlGameManager()
    {
        GameFlowManager.Instance.MinijocGuanyat();
    }

    private void NotificarDerrotaAlGameManager()
    {
        GameFlowManager.Instance.MinijocPerdut();
    }
}