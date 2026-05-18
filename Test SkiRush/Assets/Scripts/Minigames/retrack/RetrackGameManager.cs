using UnityEngine;
using UnityEngine.UI;

public class RetrackGameManager : MonoBehaviour
{
    [Header("Configuració")]
    [SerializeField] private int maxBonysPerduts = 4;
    [SerializeField] private float tempsJoc = 20f;

    [Header("UI")]
    [SerializeField] private Slider barraTemps;
    [SerializeField] private GameObject textVictoria;
    [SerializeField] private GameObject textPerdut;

    [Header("Transició")]
    [SerializeField] private float tempsEsperaDespresResultat = 1.5f;

    [Header("Mapa")]
    [SerializeField] private LoopMapa_retrack loopMapa;

    private int bonysXafats = 0;
    private int bonysPerduts = 0;
    private float tempsActual;
    private bool jocAcabat = false;

    private void Start()
    {
        Time.timeScale = 1f;

        bonysXafats = 0;
        bonysPerduts = 0;
        jocAcabat = false;

        if (GameFlowManager.Instance != null)
        {
            MinijocRuntimeConfig config = GameFlowManager.Instance.GetConfigActual();

            tempsJoc = config.temps;
            maxBonysPerduts = config.errorsPermesos;

            Debug.Log("Config retrack - Temps: " + config.temps +
                      " | Velocitat: " + config.velocitat +
                      " | Errors permesos: " + config.errorsPermesos +
                      " | Dificultat: " + config.dificultat +
                      " | Vides: " + config.vides);
        }

        tempsActual = tempsJoc;

        if (barraTemps != null)
        {
            barraTemps.value = 0f;
        }

        if (textVictoria != null)
        {
            textVictoria.SetActive(false);
        }

        if (textPerdut != null)
        {
            textPerdut.SetActive(false);
        }
    }

    private void Update()
    {
        if (jocAcabat) return;

        tempsActual -= Time.deltaTime;

        if (barraTemps != null)
        {
            barraTemps.value = 1f - Mathf.Clamp01(tempsActual / tempsJoc);
        }

        if (tempsActual <= 0f)
        {
            ComprovarFinal();
        }
    }

    public void BonyXafat()
    {
        if (jocAcabat) return;

        bonysXafats++;
        Debug.Log("Bonys xafats: " + bonysXafats);
    }

    public void BonyPerdut()
    {
        if (jocAcabat) return;

        bonysPerduts++;
        Debug.Log("Bonys perduts: " + bonysPerduts + " / " + maxBonysPerduts);

        if (bonysPerduts >= maxBonysPerduts)
        {
            Perdre();
        }
    }

    private void ComprovarFinal()
    {
        Guanyar();
    }

    private void Guanyar()
    {
        if (jocAcabat) return;

        jocAcabat = true;
        AturarJoc();
        Debug.Log("Has guanyat el minijoc retrack!");

        if (textVictoria != null)
        {
            textVictoria.SetActive(true);
        }

        Invoke(nameof(NotificarVictoriaAlGameManager), tempsEsperaDespresResultat);
    }

    private void Perdre()
    {
        if (jocAcabat) return;

        jocAcabat = true;
        AturarJoc();
        Debug.Log("Has perdut el minijoc retrack!");

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

    private void AturarJoc()
    {
        if (loopMapa != null)
        {
            loopMapa.AturarMoviment();
        }
    }
}