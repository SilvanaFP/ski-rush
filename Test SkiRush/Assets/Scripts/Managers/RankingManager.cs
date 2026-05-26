using System.Collections.Generic;
using UnityEngine;

public static class RankingManager
{
    private const int MAX_SCORES = 10;

    public static void SaveScore(int score)
    {
        Debug.Log("SAVE SCORE: " + score);
        
        List<int> scores = LoadScores();

        scores.Add(score);

        scores.Sort((a, b) => b.CompareTo(a));

        if (scores.Count > MAX_SCORES)
        {
            scores.RemoveRange(MAX_SCORES, scores.Count - MAX_SCORES);
        }

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt("Score_" + i, scores[i]);
        }

        PlayerPrefs.SetInt("ScoreCount", scores.Count);

        PlayerPrefs.Save();
    }

    public static List<int> LoadScores()
    {
        List<int> scores = new List<int>();

        int count = PlayerPrefs.GetInt("ScoreCount", 0);

        for (int i = 0; i < count; i++)
        {
            scores.Add(PlayerPrefs.GetInt("Score_" + i));
        }

        return scores;
    }
}