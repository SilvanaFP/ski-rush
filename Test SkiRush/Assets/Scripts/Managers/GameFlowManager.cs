using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    private List<string> minijocsOriginals = new List<string> { "swipe1", "drag1", "wipe1" };
    private List<string> cuaMinijocs = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PrepararCua();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PrepararCua()
    {
        cuaMinijocs = new List<string>(minijocsOriginals);

        for (int i = 0; i < cuaMinijocs.Count; i++)
        {
            int randomIndex = Random.Range(i, cuaMinijocs.Count);
            string temp = cuaMinijocs[i];
            cuaMinijocs[i] = cuaMinijocs[randomIndex];
            cuaMinijocs[randomIndex] = temp;
        }
    }

    public void CarregarSeguentMinijoc()
    {
        if (cuaMinijocs.Count == 0)
        {
            PrepararCua();
        }

        string seguent = cuaMinijocs[0];
        cuaMinijocs.RemoveAt(0);

        SceneManager.LoadScene(seguent);
    }

    public void TornarMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}