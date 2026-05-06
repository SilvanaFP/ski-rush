using UnityEngine;
using UnityEngine.UI;

public class RetrackGameManager : MonoBehaviour
{
    [Header("Configuració")]
    [SerializeField] private int maxBonysPerduts = 4;
    [SerializeField] private int bonysMinimsPerGuanyar = 10;
    [SerializeField] private float tempsJoc = 20f;

    [Header("UI")]
    [SerializeField] private Slider barraTemps;
    [SerializeField] private GameObject textVictoria;
    [SerializeField] private GameObject textPerdut;

    private int bonysXafats = 0;
    private int bonysPerduts = 0;
    private float tempsActual;
    private bool jocAcabat = false;

    private void Start()
    {
        tempsActual = tempsJoc;
        Time.timeScale = 1f;

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
        Debug.Log("Bonys perduts: " + bonysPerduts);

        if (bonysPerduts >= maxBonysPerduts)
        {
            Perdre();
        }
    }

    private void ComprovarFinal()
    {
        if (bonysXafats >= bonysMinimsPerGuanyar)
        {
            Guanyar();
        }
        else
        {
            Perdre();
        }
    }

    private void Guanyar()
    {
        jocAcabat = true;
        Debug.Log("Has guanyat el minijoc retrack!");

        if (textVictoria != null)
        {
            textVictoria.SetActive(true);
        }

        Invoke(nameof(CarregarSeguent), 2f);
    }

    private void Perdre()
    {
        jocAcabat = true;
        Debug.Log("Has perdut el minijoc retrack!");

        if (textPerdut != null)
        {
            textPerdut.SetActive(true);
        }

        Invoke(nameof(TornarMenu), 2f);
    }

    private void CarregarSeguent()
    {
        GameFlowManager.Instance.CarregarSeguentMinijoc();
    }

    private void TornarMenu()
    {
        GameFlowManager.Instance.TornarMenu();
    }
}