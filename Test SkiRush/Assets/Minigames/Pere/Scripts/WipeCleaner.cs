using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class WipeCleaner : MonoBehaviour
{
    [Header("References")]
    public RawImage dirtyImage;

    [Header("Texture Settings")]
    public int textureWidth = 512;
    public int textureHeight = 512;
    public int brushSize = 30;

    [Header("Difficulty")]
    public int level = 1;

    [Header("Spray Settings")]
    public int baseSprayCount = 200;
    public int sprayIncreasePerLevel = 80;

    [Header("Size Variation")]
    public int smallMin = 3;
    public int smallMax = 8;
    public int mediumMin = 10;
    public int mediumMax = 20;
    public int largeMin = 25;
    public int largeMax = 50;

    private Texture2D dirtTexture;
    private Color32[] pixels;

    private int initialSnowPixels = 0;
    private int currentSnowPixels = 0;

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
        GenerateSnowTexture();
    }

    void Update()
    {
        foreach (var finger in Touch.activeTouches)
        {
            TryErase(finger.screenPosition);
        }
    }

    public void GenerateSnowTexture()
    {
        dirtTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        dirtTexture.filterMode = FilterMode.Point;

        pixels = new Color32[textureWidth * textureHeight];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(255, 255, 255, 0);
        }

        int sprayCount = baseSprayCount + (level - 1) * sprayIncreasePerLevel;

        for (int i = 0; i < sprayCount; i++)
        {
            int centerX = Random.Range(0, textureWidth);
            int centerY = Random.Range(0, textureHeight);

            int radius;
            float rand = Random.value;

            if (rand < 0.5f)
                radius = Random.Range(smallMin, smallMax + 1);
            else if (rand < 0.8f)
                radius = Random.Range(mediumMin, mediumMax + 1);
            else
                radius = Random.Range(largeMin, largeMax + 1);

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    float dist = Mathf.Sqrt(x * x + y * y) / radius;
                    if (dist > 1f) continue;

                    float probability = 1f - dist;
                    if (Random.value > probability) continue;

                    int px = centerX + x;
                    int py = centerY + y;

                    if (px < 0 || px >= textureWidth || py < 0 || py >= textureHeight) continue;

                    int index = py * textureWidth + px;
                    pixels[index] = new Color32(255, 255, 255, 255);
                }
            }
        }

        initialSnowPixels = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a > 0)
                initialSnowPixels++;
        }

        currentSnowPixels = initialSnowPixels;

        dirtTexture.SetPixels32(pixels);
        dirtTexture.Apply();

        dirtyImage.texture = dirtTexture;
        dirtyImage.color = Color.white;

    }

    void TryErase(Vector2 screenPos)
    {
        RectTransform rectTransform = dirtyImage.rectTransform;
        Camera cam = null;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, cam, out localPoint))
        {
            Rect rect = rectTransform.rect;

            float xNormalized = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
            float yNormalized = Mathf.InverseLerp(rect.yMin, rect.yMax, localPoint.y);

            xNormalized = Mathf.Clamp01(xNormalized);
            yNormalized = Mathf.Clamp01(yNormalized);

            int x = Mathf.FloorToInt(xNormalized * (textureWidth - 1));
            int y = Mathf.FloorToInt(yNormalized * (textureHeight - 1));

            EraseCircle(x, y);
        }
    }

    void EraseCircle(int centerX, int centerY)
    {
        bool changed = false;

        for (int y = -brushSize; y <= brushSize; y++)
        {
            for (int x = -brushSize; x <= brushSize; x++)
            {
                if (x * x + y * y > brushSize * brushSize) continue;

                int px = centerX + x;
                int py = centerY + y;

                if (px < 0 || px >= textureWidth || py < 0 || py >= textureHeight) continue;

                int index = py * textureWidth + px;

                if (pixels[index].a > 0)
                {
                    pixels[index].a = 0;
                    currentSnowPixels--;
                    changed = true;
                }
            }
        }

        if (changed)
        {
            dirtTexture.SetPixels32(pixels);
            dirtTexture.Apply();
        }
    }

    public float GetCleanPercent()
    {
        if (initialSnowPixels <= 0) return 100f;

        float cleaned = initialSnowPixels - currentSnowPixels;
        float percent = (cleaned / initialSnowPixels) * 100f;

        return Mathf.Clamp(percent, 0f, 100f);
    }

    public void SetLevel(int newLevel)
    {
        level = Mathf.Max(1, newLevel);
        GenerateSnowTexture();
    }
}