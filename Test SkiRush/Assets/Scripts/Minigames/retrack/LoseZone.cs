using UnityEngine;

public class LoseZone : MonoBehaviour
{
    private RetrackGameManager manager;

    [SerializeField] private float tempsMargeInicial = 2f;
    private float timer;

    private void Start()
    {
        manager = FindObjectOfType<RetrackGameManager>();
        timer = tempsMargeInicial;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer > 0f) return;

        SnowBump bump = collision.GetComponent<SnowBump>();

        if (bump != null)
        {
            if (manager != null)
            {
                manager.BonyPerdut();
            }

            collision.gameObject.SetActive(false);
        }
    }
}