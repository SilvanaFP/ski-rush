using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableGear : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string gearID;
    public Transform correctSlot;
    public bool isPlaced = false;

    [Header("Snap settings")]
    public float snapDistance = 160f;

    [Header("Visual settings")]
    public Color placedColor = Color.green;
    public Color normalColor = Color.white;
    public float dragScale = 1.2f;
    public float placedScale = 0.8f;
    public float normalScale = 1f;

    private Vector3 startPosition;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();

        startPosition = rectTransform.position;

        if (image != null)
        {
            image.color = normalColor;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        canvasGroup.blocksRaycasts = false;
        rectTransform.localScale = Vector3.one * dragScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        canvasGroup.blocksRaycasts = true;

        float distance = Vector3.Distance(rectTransform.position, correctSlot.position);

        if (distance <= snapDistance)
        {
            SnapToSlot();

            GearGameManager manager = FindAnyObjectByType<GearGameManager>();
            if (manager != null)
            {
                manager.CheckWin();
            }
        }
        else
        {
            ReturnToStart();
        }
    }

    public void SnapToSlot()
    {
        rectTransform.position = correctSlot.position;
        rectTransform.localScale = Vector3.one * placedScale;
        isPlaced = true;

        if (image != null)
        {
            image.color = placedColor;
        }
    }

    public void ReturnToStart()
    {
        rectTransform.position = startPosition;
        rectTransform.localScale = Vector3.one * normalScale;

        if (image != null)
        {
            image.color = normalColor;
        }
    }
}