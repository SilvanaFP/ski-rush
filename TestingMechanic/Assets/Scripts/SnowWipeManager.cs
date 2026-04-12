using UnityEngine;
using UnityEngine.UI;

public class WipeGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float timeLimit = 5f;
    public float requiredCleanPercent = 50f;

    [Header("References")]
    public Image timeBarFill;
    public WipeCleaner cleaner;

    private float currentTime;
    private bool finished = false;

    void Start()
    {
        currentTime = timeLimit;

        if (timeBarFill != null)
        {
            timeBarFill.fillAmount = 1f;
        }
    }

    void Update()
    {
        Debug.Log(cleaner.GetCleanPercent());
        
        if (finished) return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
            currentTime = 0f;

        if (timeBarFill != null)
        {
            timeBarFill.fillAmount = currentTime / timeLimit;
        }

        if (cleaner != null && cleaner.GetCleanPercent() >= requiredCleanPercent)
        {
            finished = true;
            cleaner.enabled = false;
            Debug.Log("¡Nivel completado!");
        }

        if (currentTime <= 0f)
        {
            finished = true;
            cleaner.enabled = false;
            Debug.Log("Tiempo agotado");
        }
    }
}