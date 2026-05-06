using UnityEngine;

public class SnowBump : MonoBehaviour
{
    private bool xafat = false;
    private RetrackGameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<RetrackGameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (xafat) return;

        if (collision.CompareTag("Player"))
        {
            xafat = true;

            Debug.Log("Bony xafat!");

            if (manager != null)
            {
                manager.BonyXafat();
            }

            gameObject.SetActive(false);
        }
    }

    public void ResetBony()
    {
        xafat = false;
        gameObject.SetActive(true);
    }
}