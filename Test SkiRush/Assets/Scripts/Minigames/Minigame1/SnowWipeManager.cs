using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class WipeGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float timeLimit = 5f;
    public float requiredCleanPercent = 50f;

    [Header("References")]
    public Image timeBarFill;
    public WipeCleaner cleaner;
    public TextMeshProUGUI resultText;
    public GameObject resultOverlay;

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
        currentTime = timeLimit;

        if (timeBarFill != null)
            timeBarFill.fillAmount = 1f;

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (resultOverlay != null)
            resultOverlay.SetActive(false);
    }

    void Update()
    {
        if (!finished)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0f)
                currentTime = 0f;

            if (timeBarFill != null)
                timeBarFill.fillAmount = currentTime / timeLimit;

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
        else
        {
            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    ContinueAfterResult();
                    return;
                }
            }
        }
    }

    void FinishGame(bool won)
    {
        finished = true;
        wonGame = won;

        if (cleaner != null)
            cleaner.enabled = false;

        if (resultOverlay != null)
            resultOverlay.SetActive(true);

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = won ? "Has guanyat!" : "Has perdut!";
        }
    }

    void ContinueAfterResult()
    {
        if (wonGame)
            GameFlowManager.Instance.CarregarSeguentMinijoc();
        else
            GameFlowManager.Instance.TornarMenu();
    }
}