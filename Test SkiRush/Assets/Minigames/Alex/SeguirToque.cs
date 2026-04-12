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
    [SerializeField] private float tempsVictoria = 15f;
    private float timerVictoria = 0f;
    [SerializeField] private GameObject textVictoria;
    [SerializeField] private UnityEngine.UI.Slider barraTemps;

    private void Start()
    {
        camaraPrincipal = Camera.main;
        meuCollider = GetComponent<Collider2D>();
        Time.timeScale = 1f;
        timerInvulnerable = tempsInvulnerableInicial;
        barraTemps.value = 0f;
    }

    private void Update()
    {
        timerVictoria += Time.deltaTime;

        if (timerVictoria >= tempsVictoria && !jocAcabat)
        {
            Victoria();
            barraTemps.value = timerVictoria / tempsVictoria;
        }
        
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
        //UnityEngine.SceneManagement.SceneManager.LoadScene("NomSeguentMinijoc");
        textPerdut.SetActive(true);
    }

    private void Victoria()
    {
        jocAcabat = true;
        Debug.Log("Has guanyat!");

        Time.timeScale = 0f;
        textVictoria.SetActive(true);
    }
}