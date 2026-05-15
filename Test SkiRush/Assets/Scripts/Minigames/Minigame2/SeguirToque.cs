using UnityEngine;
using UnityEngine.UI;

public class SeguirToque : MonoBehaviour
{
    private Camera camaraPrincipal;
    private bool arrossegant = false;
    private Vector3 offset;
    private Collider2D meuCollider;
    private bool jocAcabat = false;

    [Header("Invulnerabilitat inicial")]
    [SerializeField] private float tempsInvulnerableInicial = 3f;
    private float timerInvulnerable;

    [Header("UI")]
    [SerializeField] private GameObject textPerdut;
    [SerializeField] private GameObject textVictoria;
    [SerializeField] private Slider barraTemps;

    [Header("Temps")]
    [SerializeField] private float tempsVictoria = 15f;
    private float timerVictoria = 0f;

    [Header("Transició")]
    [SerializeField] private float tempsEsperaDespresResultat = 1.5f;

    private void Start()
    {
        camaraPrincipal = Camera.main;
        meuCollider = GetComponent<Collider2D>();

        Time.timeScale = 1f;

        timerInvulnerable = tempsInvulnerableInicial;
        timerVictoria = 0f;
        jocAcabat = false;

        if (GameFlowManager.Instance != null)
        {
            MinijocRuntimeConfig config = GameFlowManager.Instance.GetConfigActual();
            tempsVictoria = config.temps;

            Debug.Log("Config swipe1 - Temps: " + config.temps + 
                      " | Velocitat: " + config.velocitat + 
                      " | Dificultat: " + config.dificultat +
                      " | Vides: " + config.vides);
        }

        if (barraTemps != null)
        {
            barraTemps.value = 0f;
        }

        if (textPerdut != null)
        {
            textPerdut.SetActive(false);
        }

        if (textVictoria != null)
        {
            textVictoria.SetActive(false);
        }
    }

    private void Update()
    {
        if (jocAcabat) return;

        ActualitzarTempsVictoria();
        ActualitzarInvulnerabilitat();
        GestionarArrossegament();
    }

    private void ActualitzarTempsVictoria()
    {
        timerVictoria += Time.deltaTime;

        if (barraTemps != null)
        {
            barraTemps.value = Mathf.Clamp01(timerVictoria / tempsVictoria);
        }

        if (timerVictoria >= tempsVictoria)
        {
            Victoria();
        }
    }

    private void ActualitzarInvulnerabilitat()
    {
        if (timerInvulnerable > 0f)
        {
            timerInvulnerable -= Time.deltaTime;
        }
    }

    private void GestionarArrossegament()
    {
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
        if (jocAcabat) return;
        if (timerInvulnerable > 0f) return;

        Debug.Log("He xocat amb: " + collision.name);

        Derrota();
    }

    private void Victoria()
    {
        jocAcabat = true;
        arrossegant = false;

        Debug.Log("Has guanyat!");

        if (textVictoria != null)
        {
            textVictoria.SetActive(true);
        }

        Invoke(nameof(NotificarVictoriaAlGameManager), tempsEsperaDespresResultat);
    }

    private void Derrota()
    {
        jocAcabat = true;
        arrossegant = false;

        Debug.Log("Has perdut!");

        if (textPerdut != null)
        {
            textPerdut.SetActive(true);
        }

        Invoke(nameof(NotificarDerrotaAlGameManager), tempsEsperaDespresResultat);
    }

    private void NotificarVictoriaAlGameManager()
    {
        GameFlowManager.Instance.MinijocGuanyat();
    }

    private void NotificarDerrotaAlGameManager()
    {
        GameFlowManager.Instance.MinijocPerdut();
    }
}