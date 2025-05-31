using UnityEngine;

public class WarningLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private Vector2 startPos;
    private float length;
    private float warningDuration;
    private float elapsed = 0f;

    private bool isTracking = true;
    private bool isActive = false;

    public float fadeDuration = 0.5f;
    private float fadeElapsed = 0f;

    private Color originalColor;

    void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        originalColor = lineRenderer.startColor;
    }

    public void Setup(Vector2 start, float laserLength, float duration)
    {
        startPos = start;
        length = laserLength;
        warningDuration = duration;

        elapsed = 0f;
        isTracking = true;
        isActive = true;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos + Vector2.down * length);
    }

    void Update()
    {
        if (!isActive) return;

        if (isTracking)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= warningDuration)
            {
                isTracking = false;
                return;
            }

            if (ShipMovement.Instance != null)
            {
                Vector2 dirToPlayer = ((Vector2)ShipMovement.Instance.transform.position - startPos).normalized;
                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, startPos + dirToPlayer * length);
            }
        }
    }

    public void StartFadeAndDestroy()
    {
        if (!isActive) return;

        isActive = false;
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        fadeElapsed = 0f;
        Color startColor = originalColor;
        Color endColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float t = fadeElapsed / fadeDuration;
            Color curColor = Color.Lerp(startColor, endColor, t);
            lineRenderer.startColor = curColor;
            lineRenderer.endColor = curColor;
            yield return null;
        }

        Destroy(gameObject);
    }
}
