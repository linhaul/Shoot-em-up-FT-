using Unity.Mathematics;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject playerBullet;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 0.2f;
    public int bulletsPerShot = 3;

    private float fireCooldown = 0f;

    private enum fireMode { LinearShot, SpreadShot };
    private fireMode currentMode = fireMode.LinearShot;

    private void Update()
    {
        HandleShootingInput();
        HandleModeSwitchInput();
    }

    void HandleShootingInput()
    {
        fireCooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Z) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    void HandleModeSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentMode = currentMode == fireMode.LinearShot ? fireMode.SpreadShot : fireMode.LinearShot;
        }
    }

    void Shoot()
    {
        switch (currentMode)
        {
            case fireMode.LinearShot:
                FireLinearShot();
                break;
            case fireMode.SpreadShot:
                FireSpreadShot();
                break;
        }
    }

    void FireLinearShot()
    {
        float spacing = 0.5f;
        float startx = -(bulletsPerShot - 1) * spacing / 2f;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 spawnPos = firePoint.position + new Vector3(startx + i * spacing, 0f, 0f);
            GameObject bullet = Instantiate(playerBullet, spawnPos, quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * bulletSpeed;
        }
    }

    void FireSpreadShot()
    {
        int spreadBullets = bulletsPerShot + 2; // увеличиваем на 2 только для спреда

        float angleStep = 30f / (spreadBullets - 1); // допустим, веер в 30 градусов
        float startAngle = -15f; // веер от -15 до +15 градусов

        for (int i = 0; i < spreadBullets; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

            GameObject bullet = Instantiate(playerBullet, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        }
    }
}
