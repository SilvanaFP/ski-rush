using UnityEngine;

public static class SessionManager
{
    public static int UserId { get; private set; } = -1;
    public static string Username { get; private set; } = "";

    public static int LastScore { get; private set; } = 0;

    public static bool IsLoggedIn()
    {
        return UserId > 0 && !string.IsNullOrEmpty(Username);
    }

    public static void SetSession(int userId, string username)
    {
        UserId = userId;
        Username = username;

        PlayerPrefs.SetInt("UserId", userId);
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.Save();
    }

    public static void LoadSession()
    {
        UserId = PlayerPrefs.GetInt("UserId", -1);
        Username = PlayerPrefs.GetString("Username", "");
        LastScore = PlayerPrefs.GetInt("LastScore", 0);
    }

    public static void SetLastScore(int score)
    {
        LastScore = score;

        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
    }

    public static void ClearSession()
    {
        UserId = -1;
        Username = "";

        PlayerPrefs.DeleteKey("UserId");
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.Save();
    }
}