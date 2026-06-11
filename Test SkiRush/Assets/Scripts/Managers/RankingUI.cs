using TMPro;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private Transform scoresContainer;
    [SerializeField] private GameObject scoreItemPrefab;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Your Score")]
    [SerializeField] private TextMeshProUGUI yourScoreText;

    [Header("Estil")]
    [SerializeField] private float midaText = 36f;

    private bool rankingCarregat = false;

    private void Start()
    {
        SessionManager.LoadSession();

        if (rankingCarregat) return;

        rankingCarregat = true;

        ActualitzarYourScore();
        CarregarRankingOnline();
    }

    private void CarregarRankingOnline()
    {
        NetejarRanking();

        if (ApiClient.Instance == null)
        {
            MostrarMissatge("No API connection found.");
            return;
        }

        MostrarMissatge("Loading ranking...");

        StartCoroutine(ApiClient.Instance.GetRanking(
            ranking =>
            {
                MostrarMissatge("");

                if (ranking == null || ranking.Length == 0)
                {
                    MostrarMissatge("No scores yet.");
                    return;
                }

                MostrarRanking(ranking);
            },
            error =>
            {
                MostrarMissatge("Error loading ranking: " + error);
            }
        ));
    }

    private void MostrarRanking(ApiClient.RankingItem[] ranking)
    {
        for (int i = 0; i < ranking.Length; i++)
        {
            GameObject obj = Instantiate(scoreItemPrefab, scoresContainer);

            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
            {
                text.fontSize = midaText;

                text.text =
                    ranking[i].position + " - " +
                    ranking[i].username.ToUpper() + " - " +
                    ranking[i].score + " pts";
            }
        }
    }

    private void ActualitzarYourScore()
    {
        if (yourScoreText != null)
        {
            yourScoreText.text = "Your last score: " + SessionManager.LastScore + " pts";
        }
    }

    private void NetejarRanking()
    {
        foreach (Transform child in scoresContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void MostrarMissatge(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }

        Debug.Log(message);
    }
}