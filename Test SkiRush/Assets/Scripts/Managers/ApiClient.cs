using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    public static ApiClient Instance;

    [Header("API")]
    [SerializeField] private string baseUrl = "http://127.0.0.1:8000";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Serializable]
    public class AuthRequest
    {
        public string username;
        public string password;

        public AuthRequest(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    [Serializable]
    public class UserResponse
    {
        public int id;
        public string username;
    }

    [Serializable]
    public class ScoreRequest
    {
        public int user_id;
        public int score;

        public ScoreRequest(int userId, int score)
        {
            this.user_id = userId;
            this.score = score;
        }
    }

    [Serializable]
    public class ScoreResponse
    {
        public int id;
        public int user_id;
        public int score;
        public string created_at;
    }

    [Serializable]
    public class RankingItem
    {
        public int position;
        public string username;
        public int score;
        public string created_at;
    }

    [Serializable]
    private class RankingWrapper
    {
        public RankingItem[] items;
    }

    [Serializable]
    private class ErrorResponse
    {
        public string detail;
    }

    public IEnumerator Register(
        string username,
        string password,
        Action<UserResponse> onSuccess,
        Action<string> onError
    )
    {
        AuthRequest data = new AuthRequest(username, password);
        string json = JsonUtility.ToJson(data);

        yield return PostRequest(
            "/register",
            json,
            response =>
            {
                UserResponse user =
                    JsonUtility.FromJson<UserResponse>(response);

                onSuccess?.Invoke(user);
            },
            onError
        );
    }

    public IEnumerator Login(
        string username,
        string password,
        Action<UserResponse> onSuccess,
        Action<string> onError
    )
    {
        AuthRequest data = new AuthRequest(username, password);
        string json = JsonUtility.ToJson(data);

        yield return PostRequest(
            "/login",
            json,
            response =>
            {
                UserResponse user =
                    JsonUtility.FromJson<UserResponse>(response);

                onSuccess?.Invoke(user);
            },
            onError
        );
    }

    public IEnumerator SaveScore(
        int userId,
        int score,
        Action<ScoreResponse> onSuccess,
        Action<string> onError
    )
    {
        ScoreRequest data = new ScoreRequest(userId, score);
        string json = JsonUtility.ToJson(data);

        yield return PostRequest(
            "/scores",
            json,
            response =>
            {
                ScoreResponse scoreResponse =
                    JsonUtility.FromJson<ScoreResponse>(response);

                onSuccess?.Invoke(scoreResponse);
            },
            onError
        );
    }

    public IEnumerator GetRanking(
        Action<RankingItem[]> onSuccess,
        Action<string> onError
    )
    {
        yield return GetRequest(
            "/ranking",
            response =>
            {
                string wrappedJson =
                    "{\"items\":" + response + "}";

                RankingWrapper wrapper =
                    JsonUtility.FromJson<RankingWrapper>(wrappedJson);

                onSuccess?.Invoke(wrapper.items);
            },
            onError
        );
    }

    public IEnumerator GetUserScores(
        string username,
        Action<RankingItem[]> onSuccess,
        Action<string> onError
    )
    {
        yield return GetRequest(
            "/users/" + username + "/scores",
            response =>
            {
                string wrappedJson =
                    "{\"items\":" + response + "}";

                RankingWrapper wrapper =
                    JsonUtility.FromJson<RankingWrapper>(wrappedJson);

                onSuccess?.Invoke(wrapper.items);
            },
            onError
        );
    }

    private IEnumerator PostRequest(
        string endpoint,
        string json,
        Action<string> onSuccess,
        Action<string> onError
    )
    {
        string url = baseUrl + endpoint;

        UnityWebRequest request =
            new UnityWebRequest(url, "POST");

        byte[] bodyRaw =
            Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
            new UploadHandlerRaw(bodyRaw);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke(GetErrorMessage(request));
        }
    }

    private IEnumerator GetRequest(
        string endpoint,
        Action<string> onSuccess,
        Action<string> onError
    )
    {
        string url = baseUrl + endpoint;

        UnityWebRequest request =
            UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke(GetErrorMessage(request));
        }
    }

    private string GetErrorMessage(UnityWebRequest request)
    {
        string response = request.downloadHandler.text;

        if (!string.IsNullOrEmpty(response))
        {
            try
            {
                ErrorResponse error =
                    JsonUtility.FromJson<ErrorResponse>(response);

                if (!string.IsNullOrEmpty(error.detail))
                {
                    return error.detail;
                }
            }
            catch
            {
                return response;
            }
        }

        return request.error;
    }
}