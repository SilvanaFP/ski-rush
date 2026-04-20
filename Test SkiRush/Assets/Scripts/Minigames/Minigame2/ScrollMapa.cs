using UnityEngine;

public class ScrollMapa : MonoBehaviour
{
    [SerializeField] private float velocitat = 2f;

    private void Update()
    {
        transform.Translate(Vector3.up * velocitat * Time.deltaTime);
    }
}