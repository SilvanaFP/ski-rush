using UnityEngine;
using UnityEngine.UI;

public class VidesUI : MonoBehaviour
{
    [Header("Cors")]
    [SerializeField] private Image[] cors;

    [Header("Sprites")]
    [SerializeField] private Sprite corPle;
    [SerializeField] private Sprite corBuit;

    private void Start()
    {
        ActualitzarVides();
    }

    public void ActualitzarVides()
    {
        if (cors == null || cors.Length == 0) return;

        int videsActuals = 0;

        if (GameFlowManager.Instance != null)
        {
            videsActuals = GameFlowManager.Instance.GetVidesActuals();
        }

        for (int i = 0; i < cors.Length; i++)
        {
            if (cors[i] == null) continue;

            if (i < videsActuals)
            {
                cors[i].sprite = corPle;
            }
            else
            {
                cors[i].sprite = corBuit;
            }
        }
    }
}