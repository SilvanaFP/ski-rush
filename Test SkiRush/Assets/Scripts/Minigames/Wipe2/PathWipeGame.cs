using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using TMPro;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PathGameManager : MonoBehaviour
{
    [Header("References")]
    public LineRenderer pathCore;
    public LineRenderer pathGlow;
    public LineRenderer playerTrace;

    public Transform treesParent;
    public Camera cam;

    [Header("UI")]
    public Image timeBarFill;
    public TextMeshProUGUI resultText;
    public GameObject resultOverlay;

    [Header("Settings")]
    public float startTolerance = 120f;
    public float eraseRadius = 50f;

    [Header("Time Settings")]
    public float timeLimit = 5f;

    public enum PathPattern
    {
        Sinusoide,
        ZigZag,
        Corba,
        FormaS
    }

    public PathPattern currentPattern;

    private List<Vector2> pathPoints = new List<Vector2>();
    private List<Vector2> playerPoints = new List<Vector2>();
    private List<GameObject> trees = new List<GameObject>();

    private bool gameStarted = false;
    private bool gameFinished = false;
    private bool startedTouch = false;
    private bool wonGame = false;

    private float currentTime;

    float startY;
    float endY;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        foreach (Transform t in treesParent)
            trees.Add(t.gameObject);

        pathCore.positionCount = 0;
        pathGlow.positionCount = 0;
        playerTrace.positionCount = 0;

        currentTime = timeLimit;

        if (timeBarFill != null)
            timeBarFill.fillAmount = 1f;

        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (resultOverlay != null)
            resultOverlay.SetActive(false);

        StartGame();
    }

    void Update()
    {
        if (!gameFinished)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0f)
                currentTime = 0f;

            if (timeBarFill != null)
                timeBarFill.fillAmount = currentTime / timeLimit;

            if (currentTime <= 0f)
            {
                FinishGame(false);
                return;
            }

            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    HandleInput(touch.screenPosition);
                }
            }
        }
        else
        {
            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    ContinueAfterResult();
                    return;
                }
            }
        }
    }

    void HandleInput(Vector2 screenPos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
        worldPos.z = 0;

        playerTrace.positionCount++;
        playerTrace.SetPosition(playerTrace.positionCount - 1, worldPos);

        playerPoints.Add(screenPos);

        CheckProgress(screenPos);
    }

    void CheckProgress(Vector2 screenPos)
    {
        if (!startedTouch)
        {
            float safeStartY = pathPoints[0].y - startTolerance;

            if (screenPos.y >= safeStartY)
            {
                startedTouch = true;
                Debug.Log("START DETECTED");
            }
            return;
        }

        float progress = Mathf.InverseLerp(startY, endY, screenPos.y);

        if (progress >= 0.95f)
        {
            Debug.Log("FINISH");
            FinishGame(true);
        }
    }

    void FinishGame(bool won)
    {
        gameFinished = true;
        wonGame = won;

        if (won)
        {
            foreach (var tree in trees)
            {
                if (tree == null || !tree.activeSelf) continue;

                RectTransform rect = tree.GetComponent<RectTransform>();
                Vector2 treeScreen = RectTransformUtility.WorldToScreenPoint(cam, rect.position);

                bool shouldDelete = false;

                for (int i = 0; i < playerPoints.Count - 2; i++)
                {
                    float dynamicRadius = eraseRadius;

                    if (i > 0)
                    {
                        Vector2 dir1 = (playerPoints[i] - playerPoints[i - 1]).normalized;
                        Vector2 dir2 = (playerPoints[i + 1] - playerPoints[i]).normalized;

                        float angle = Vector2.Angle(dir1, dir2);

                        if (angle > 20f)
                            dynamicRadius *= 1.5f;
                    }

                    float dist = DistanceToSegment(treeScreen, playerPoints[i], playerPoints[i + 1]);

                    if (dist < dynamicRadius)
                    {
                        shouldDelete = true;
                        break;
                    }
                }

                if (!shouldDelete)
                {
                    for (int i = 0; i < pathPoints.Count - 1; i++)
                    {
                        float dist = DistanceToSegment(treeScreen, pathPoints[i], pathPoints[i + 1]);

                        if (dist < eraseRadius * 0.8f)
                        {
                            shouldDelete = true;
                            break;
                        }
                    }
                }

                if (shouldDelete)
                {
                    Image img = tree.GetComponent<Image>();
                    if (img != null)
                        img.enabled = false;

                    CanvasGroup cg = tree.GetComponent<CanvasGroup>();
                    if (cg == null)
                        cg = tree.AddComponent<CanvasGroup>();

                    cg.blocksRaycasts = false;
                    cg.interactable = false;
                }
            }
        }
<<<<<<< HEAD
        Invoke("CarregarSeguent", 2f);
=======

        if (resultOverlay != null)
            resultOverlay.SetActive(true);

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = won ? "Has guanyat!" : "Has perdut!";
        }
    }

    void ContinueAfterResult()
    {
        if (wonGame)
            GameFlowManager.Instance.CarregarSeguentMinijoc();
        else
            GameFlowManager.Instance.TornarMenu();
>>>>>>> 5f3f3f4 (Jocs nous junts)
    }

    float DistanceToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Vector2.Dot(p - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        Vector2 closest = a + t * ab;
        return Vector2.Distance(p, closest);
    }

    void StartGame()
    {
        gameStarted = true;

        currentPattern = (PathPattern)Random.Range(0, 4);

        GeneratePath();
        DrawPath();
    }

    void GeneratePath()
    {
        pathPoints.Clear();

        switch (currentPattern)
        {
            case PathPattern.Sinusoide:
                GenerateSin();
                break;

            case PathPattern.ZigZag:
                GenerateZigZag();
                break;

            case PathPattern.Corba:
                GenerateCurve();
                break;

            case PathPattern.FormaS:
                GenerateS();
                break;
        }

        startY = pathPoints[0].y;
        endY = pathPoints[pathPoints.Count - 1].y;
    }

    void GenerateSin()
    {
        float h = Screen.height;
        float w = Screen.width * 0.6f;

        for (int i = 0; i < 10; i++)
        {
            float t = i / 9f;
            float y = Mathf.Lerp(h, 0, t);
            float x = Screen.width / 2f + Mathf.Sin(t * Mathf.PI * 2f) * w * 0.5f;

            pathPoints.Add(new Vector2(x, y));
        }
    }

    void GenerateZigZag()
    {
        float h = Screen.height;

        for (int i = 0; i < 10; i++)
        {
            float t = i / 9f;
            float y = Mathf.Lerp(h, 0, t);
            float x = (i % 2 == 0) ? Screen.width * 0.2f : Screen.width * 0.8f;

            pathPoints.Add(new Vector2(x, y));
        }
    }

    void GenerateCurve()
    {
        float h = Screen.height;

        for (int i = 0; i < 10; i++)
        {
            float t = i / 9f;
            float y = Mathf.Lerp(h, 0, t);
            float x = Mathf.Lerp(Screen.width * 0.3f, Screen.width * 0.7f, t);

            pathPoints.Add(new Vector2(x, y));
        }
    }

    void GenerateS()
    {
        float h = Screen.height;
        float w = Screen.width * 0.6f;

        for (int i = 0; i < 12; i++)
        {
            float t = i / 11f;
            float y = Mathf.Lerp(h, 0, t);
            float x = Screen.width / 2f + Mathf.Sin(t * Mathf.PI * 4f) * w * 0.3f;

            pathPoints.Add(new Vector2(x, y));
        }
    }

    void DrawPath()
    {
        pathCore.positionCount = pathPoints.Count;
        pathGlow.positionCount = pathPoints.Count;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 pos = cam.ScreenToWorldPoint(new Vector3(pathPoints[i].x, pathPoints[i].y, 10f));
            pos.z = 0;

            pathCore.SetPosition(i, pos);
            pathGlow.SetPosition(i, pos);
        }
    }
<<<<<<< HEAD

    void CarregarSeguent()
        {
            GameFlowManager.Instance.CarregarSeguentMinijoc();
        }

    void TornarMenu()
    {
        GameFlowManager.Instance.TornarMenu();
    }
=======
>>>>>>> 5f3f3f4 (Jocs nous junts)
}
