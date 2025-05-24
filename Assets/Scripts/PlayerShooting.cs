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

    public UnityEngine.UI.Image superMeterFillImage;
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
    }

    private void Update()
    {
        HandleModeSwitchInput();

        if (Input.GetKey(KeyCode.Z) && !isShooting)
        {
            StartCoroutine(ShootBurst());
        }

        if (Input.GetKeyDown(KeyCode.Space) && superReady)
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
        if (superMeterFillImage != null)
        {
            superMeterFillImage.fillAmount = superMeterCurrent / superMeterMax;
        }
    }

    public void AddSuperMeter(float amount)
    {
        superMeterCurrent += amount;
        if (superMeterCurrent > superMeterMax)
            superMeterCurrent = superMeterMax;

        if (superMeterFillImage != null)
            superMeterFillImage.fillAmount = superMeterCurrent / superMeterMax;
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
        if (Input.GetKeyDown(KeyCode.X))
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
            Shoot();

            if (i < bulletsPerBurst - 1)
                yield return new WaitForSeconds(burstInterval);
        }

        yield return new WaitForSeconds(burstCooldown);

        isShooting = false;
    }

    void Shoot()
    {
        switch (currentMode)
        {
            case FireMode.LinearShot:
                FireLinearShot();
                break;
            case FireMode.SpreadShot:
                FireSpreadShot();
                break;
        }
    }

    void FireLinearShot()
    {
        int rows = 3; 
        int[] bulletsInRow = { 5, 3, 1 }; 
        float rowHeight = 0.3f;               
        float spacing = 0.3f;                

        Vector3 basePos = firePoint.position;

        for (int row = 0; row < rows; row++)
        {
            int count = bulletsInRow[row];
            float startX = -(count - 1) * spacing / 2f;
            float yOffset = row * rowHeight;

            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPos = basePos + new Vector3(startX + i * spacing, yOffset, 0f);
                GameObject bullet = Instantiate(playerBullet, spawnPos, Quaternion.identity);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.up * bulletSpeed;
                }
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
