using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class WipeGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float timeLimit = 5f;
    [SerializeField] private float requiredCleanPercent = 50f;

    [Header("References")]
    public Image timeBarFill;
    public WipeCleaner cleaner;
    public TextMeshProUGUI resultText;
    public GameObject resultOverlay;

    [Header("Transició")]
    [SerializeField] private float tempsEsperaDespresResultat = 1.5f;

    private float currentTime;
    private bool finished = false;
    private bool wonGame = false;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        Time.timeScale = 1f;
        finished = false;
        wonGame = false;

        if (GameFlowManager.Instance != null)
        {
            MinijocRuntimeConfig config = GameFlowManager.Instance.GetConfigActual();

            timeLimit = config.temps;

            if (cleaner != null)
            {
                cleaner.SetLevel(config.dificultat);
            }

            Debug.Log("Config wipe1 - Temps: " + config.temps +
                      " | Percentatge necessari: " + requiredCleanPercent +
                      " | Dificultat: " + config.dificultat +
                      " | Vides: " + config.vides);
        }

        currentTime = timeLimit;

        if (timeBarFill != null)
        {
            timeBarFill.fillAmount = 1f;
        }

        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }

        if (resultOverlay != null)
        {
            resultOverlay.SetActive(false);
        }
    }

    void Update()
    {
        if (finished) return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
        {
            currentTime = 0f;
        }

        if (timeBarFill != null)
        {
            timeBarFill.fillAmount = currentTime / timeLimit;
        }

        if (cleaner != null && cleaner.GetCleanPercent() >= requiredCleanPercent)
        {
            FinishGame(true);
            return;
        }

        if (currentTime <= 0f)
        {
            FinishGame(false);
            return;
        }
    }

    void FinishGame(bool won)
    {
        if (finished) return;

        finished = true;
        wonGame = won;

        if (cleaner != null)
        {
            cleaner.enabled = false;
        }

        if (resultOverlay != null)
        {
            resultOverlay.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = won ? "Has guanyat!" : "Has perdut!";
        }

        if (won)
        {
            Debug.Log("Has guanyat el minijoc wipe1!");
            Invoke(nameof(NotificarVictoriaAlGameManager), tempsEsperaDespresResultat);
        }
        else
        {
            Debug.Log("Has perdut el minijoc wipe1!");
            Invoke(nameof(NotificarDerrotaAlGameManager), tempsEsperaDespresResultat);
        }
    }

    void NotificarVictoriaAlGameManager()
    {
        GameFlowManager.Instance.MinijocGuanyat();
    }

    void NotificarDerrotaAlGameManager()
    {
        GameFlowManager.Instance.MinijocPerdut();
    }
}