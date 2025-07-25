using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxLifes = 3;
    public int currentLifes;
    public PlayerUIManager uiManager;

    public float iFrameDuration = 2f;
    public float flashInterval = 0.1f;
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentLifes = maxLifes;
        uiManager.UpdateLifes(currentLifes);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        currentLifes--;

        if (currentLifes <= 0)
        {
            Death();
        }

        uiManager.UpdateLifes(currentLifes);
        StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        while (elapsed < iFrameDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    void Death()
    {
        var gameOverController = FindFirstObjectByType<GameOverMenuController>();
        if (gameOverController != null)
        {
            gameOverController.ShowGameOver();

            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PlayDeathMusic();
            }
        }
        else
        {
            Debug.LogWarning("GameOverMenuController не найден в сцене!");
        }
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}
