using UnityEngine;

public class SeguirToque : MonoBehaviour
{
    private Camera camaraPrincipal;
    private bool arrossegant = false;
    private Vector3 offset;
    private Collider2D meuCollider;
    private bool jocAcabat = false;

    [SerializeField] private float tempsInvulnerableInicial = 3f;
    private float timerInvulnerable;
    [SerializeField] private GameObject textPerdut;
    private void Start()
    {
        camaraPrincipal = Camera.main;
        meuCollider = GetComponent<Collider2D>();
        Time.timeScale = 1f;
        timerInvulnerable = tempsInvulnerableInicial;
    }

    private void Update()
    {
        if (jocAcabat) return;

        if (timerInvulnerable > 0f)
        {
            timerInvulnerable -= Time.deltaTime;
        }

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
            transform.position = new Vector3(novaPos.x, transform.position.y, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            arrossegant = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timerInvulnerable > 0f) return;

        Debug.Log("He xocat amb: " + collision.name);
        arrossegant = false;
        jocAcabat = true;
        Time.timeScale = 0f;
        textPerdut.SetActive(true);
    }
}