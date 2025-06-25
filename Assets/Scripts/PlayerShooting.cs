using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public GameObject playerBullet;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    public int bulletsPerBurst = 3;
    public float burstInterval = 0.1f;
    public float burstCooldown = 0.5f;

    private bool isShooting = false;

    private enum FireMode { LinearShot, SpreadShot };
    private FireMode currentMode = FireMode.LinearShot;
    public PlayerUIManager uiManager;

    public float superMeterMax = 100f;
    private float superMeterCurrent = 0f;

    public UnityEngine.UI.Slider superMeterSlider;
    public GameObject superAttackPrefab;

    private bool superReady => superMeterCurrent >= superMeterMax;

    public static PlayerShooting Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();

        if (superMeterSlider != null)
        {
            superMeterSlider.minValue = 0f;
            superMeterSlider.maxValue = 1f;
            superMeterSlider.value = 0f;
        }
    }

    private void Update()
    {
        if (GameOverMenuController.IsGameOver)
            return;
            
        HandleModeSwitchInput();

        if (Input.GetKey(CombatKeybindManager.FireKey) && !isShooting)
        {
            StartCoroutine(ShootBurst());
        }

        if (Input.GetKeyDown(CombatKeybindManager.SuperKey) && superReady)
        {
            ActivateSuperAttack();
        }
    }

    void ActivateSuperAttack()
    {
        if (!superReady) return;

        superMeterCurrent = 0;
        UpdateSuperMeterUI();

        if (superAttackPrefab != null)
        {
            Instantiate(superAttackPrefab, firePoint.position, Quaternion.identity);
        }
    }

    void UpdateSuperMeterUI()
    {
        if (superMeterSlider != null)
        {
            superMeterSlider.value = superMeterCurrent / superMeterMax;
        }
    }

    public void AddSuperMeter(float amount)
    {
        superMeterCurrent += amount;
        if (superMeterCurrent > superMeterMax)
            superMeterCurrent = superMeterMax;

        if (superMeterSlider != null)
            superMeterSlider.value = superMeterCurrent / superMeterMax;
    }

    void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateShotType(currentMode.ToString());
        }
    }

    void HandleModeSwitchInput()
    {
        if (Input.GetKeyDown(CombatKeybindManager.SwitchFireModeKey))
        {
            currentMode = currentMode == FireMode.LinearShot ? FireMode.SpreadShot : FireMode.LinearShot;
            UpdateUI();
        }
    }

    IEnumerator ShootBurst()
    {
        isShooting = true;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            if (currentMode == FireMode.LinearShot)
                FireLinearShot(i); 
            else if (currentMode == FireMode.SpreadShot)
                FireSpreadShot();

            if (i < bulletsPerBurst - 1)
                yield return new WaitForSeconds(burstInterval);
        }

        yield return new WaitForSeconds(burstCooldown);
        isShooting = false;
    }

    void FireLinearShot(int burstIndex)
    {
        int bulletCount = 10;
        float baseWidth = 0.4f;
        float widthStep = 0.2f;
        float currentWidth = baseWidth + burstIndex * widthStep;

        for (int i = 0; i < bulletCount; i++)
        {
            float t = i / (float)(bulletCount - 1);
            float x = Mathf.Lerp(-1f, 1f, t);
            float y = Mathf.Sqrt(1 - x * x);

            Vector3 offset = new Vector3(x * currentWidth, y * 0.5f, 0f);
            Vector3 spawnPos = firePoint.position + offset;

            GameObject bullet = Instantiate(playerBullet, spawnPos, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.up * bulletSpeed;
            }
        }
    }

    void FireSpreadShot()
    {
        int raysCount = 7;
        int rowsPerRay = 2;
        float spreadAngle = 90f;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (raysCount - 1);

        float rowSpacing = 0.5f;

        for (int i = 0; i < raysCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

            for (int row = 0; row < rowsPerRay; row++)
            {
                Vector3 spawnPos = firePoint.position + (Vector3)(direction * row * rowSpacing);
                GameObject bullet = Instantiate(playerBullet, spawnPos, Quaternion.Euler(0, 0, angle));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
            }
        }
    }
}
