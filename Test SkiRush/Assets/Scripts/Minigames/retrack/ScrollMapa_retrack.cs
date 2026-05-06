using UnityEngine;

public class ScrollMapa_retrack : MonoBehaviour
{
    [SerializeField] private float velocitat = 2f;

    private void Update()
    {
        transform.Translate(Vector3.down * velocitat * Time.deltaTime);
    }
}