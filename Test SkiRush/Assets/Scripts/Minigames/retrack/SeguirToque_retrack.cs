using UnityEngine;

public class SeguirToque_retrack : MonoBehaviour
{
    private Camera camaraPrincipal;
    private bool arrossegant = false;
    private Vector3 offset;

    [Header("Posició")]
    [SerializeField] private float posicioYFixe = -4f;

    [Header("Límits laterals")]
    [SerializeField] private float limitXEsquerra = -2.5f;
    [SerializeField] private float limitXDreta = 2.5f;

    [Header("Mapa")]
    [SerializeField] private LoopMapa_retrack loopMapa;

    private bool jocAturat = false;

    private void Start()
    {
        camaraPrincipal = Camera.main;

        Time.timeScale = 1f;

        transform.position = new Vector3(
            transform.position.x,
            posicioYFixe,
            transform.position.z
        );
    }

    private void Update()
    {
        if (jocAturat) return;

        transform.position = new Vector3(
            transform.position.x,
            posicioYFixe,
            transform.position.z
        );

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

            float xLimitada = Mathf.Clamp(
                novaPos.x,
                limitXEsquerra,
                limitXDreta
            );

            transform.position = new Vector3(
                xLimitada,
                posicioYFixe,
                transform.position.z
            );
        }

        if (Input.GetMouseButtonUp(0))
        {
            arrossegant = false;
        }
    }

    public void AturarJoc()
    {
        jocAturat = true;
        arrossegant = false;

        if (loopMapa != null)
        {
            loopMapa.AturarMoviment();
        }
    }
}