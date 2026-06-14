using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TransitionSceneManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI transitionText;

    [Header("Settings")]
    [SerializeField] private float waitTime = 1.5f;

    private void Start()
    {
        string nextScene =
            GameFlowManager.Instance.GetMinijocActual();

        transitionText.text =
            GetTextForMinigame(nextScene);

        Invoke(nameof(LoadNextScene), waitTime);
    }

    private string GetTextForMinigame(string sceneName)
    {
        switch (sceneName)
        {
            case "swipe1":
                return "DODGE!";

            case "drag1":
                return "FIX!";

            case "wipe1":
                return "CLEAN!";

            case "wipe2":
                return "TRACE!";

            case "retrack":
                return "SMASH!";

            case "drag2":
                return "PAIR!";

            default:
                return "GET READY!!";
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(
            GameFlowManager.Instance.GetMinijocActual()
        );
    }
}