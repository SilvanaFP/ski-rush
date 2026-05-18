using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private Transform scoresContainer;
    [SerializeField] private GameObject scoreItemPrefab;

    private void Start()
    {
        List<int> scores = RankingManager.LoadScores();

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject obj =
                Instantiate(scoreItemPrefab, scoresContainer);

            TextMeshProUGUI text =
                obj.GetComponentInChildren<TextMeshProUGUI>();

            string ordinal = GetOrdinalCatalan(i + 1);

            text.text =
                ordinal + ". " + scores[i];
        }
    }

    private string GetOrdinalCatalan(int position)
    {
        switch (position)
        {
            case 1:
                return "1r";

            case 2:
                return "2n";

            case 3:
                return "3r";

            case 4:
                return "4t";

            default:
                return position + "è";
        }
    }
}
