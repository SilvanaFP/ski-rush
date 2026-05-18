using UnityEngine;
using TMPro;

public class GearGameManager : MonoBehaviour
{
    public DraggableGear[] gears;
    public GameObject winPanel;
    public GameObject losePanel;

    public TextMeshProUGUI timerText;
    public float timeLeft = 7f;

    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded)
            return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            timerText.text = "00:00";
            LoseGame();
            return;
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CheckWin()
    {
        if (gameEnded)
            return;

        foreach (DraggableGear gear in gears)
        {
            if (!gear.isPlaced)
            {
                return;
            }
        }

        WinGame();
    }

    void WinGame()
    {
        gameEnded = true;
        winPanel.SetActive(true);
        Debug.Log("Minijoc completat!");
        Invoke("CarregarSeguent", 2f);
    }

    void LoseGame()
    {
        gameEnded = true;
<<<<<<< Updated upstream
        losePanel.SetActive(true);
        Debug.Log("Has perdut!");
        Invoke("TornarMenu", 2f);
=======

        bool mostrarDerrotaLocal =
            GameFlowManager.Instance == null ||
            GameFlowManager.Instance.GetVidesActuals() > 1;

        if (mostrarDerrotaLocal && losePanel != null)
        {
            losePanel.SetActive(true);
        }

        Debug.Log("Has perdut el minijoc drag!");

        Invoke(nameof(NotificarDerrotaAlGameManager), tempsEsperaDespresResultat);
>>>>>>> Stashed changes
    }

    void CarregarSeguent()
    {
        GameFlowManager.Instance.CarregarSeguentMinijoc();
    }

    void TornarMenu()
    {
        GameFlowManager.Instance.TornarMenu();
    }
}

