using UnityEngine;

public class SeguirToque_retrack : MonoBehaviour
{
    private Camera camaraPrincipal;
    private bool arrossegant = false;
    private Vector3 offset;

    [SerializeField] private float posicioYFixe = -4f;

    private void Start()
    {
        camaraPrincipal = Camera.main;
        Time.timeScale = 1f;

        transform.position = new Vector3(transform.position.x, posicioYFixe, transform.position.z);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, posicioYFixe, transform.position.z);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = -camaraPrincipal.transform.position.z;

            Vector3 posMouse = camaraPrincipal.ScreenToWorldPoint(screenPos);
            posMouse.z = 0f;

            arrossegant = true;
            offset = transform.position - posMouse;
        }

        if (Input.GetMouseButton(0) && arrossegant)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = -camaraPrincipal.transform.position.z;

            Vector3 posMouse = camaraPrincipal.ScreenToWorldPoint(screenPos);
            posMouse.z = 0f;

            Vector3 novaPos = posMouse + offset;
            transform.position = new Vector3(novaPos.x, posicioYFixe, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            arrossegant = false;
        }
    }
}