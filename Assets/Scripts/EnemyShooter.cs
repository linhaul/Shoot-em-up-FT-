using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPref;
    public float fireInterval = 1.5f;
    public float bulletSpeed = 3f;
    public GameObject warningLaserPref;

    public enum FirePattern
    {
        SingleShot,
        SpreadShot,
        CircleShot,
        AimAtPlayer,
        FastBurst,
        AimedBurstWithWarning
    };
    public FirePattern firePattern = FirePattern.SingleShot;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            Fire();
        }
    }

    void Fire()
    {
        switch (firePattern)
        {
            case FirePattern.SingleShot:
                FireSingle();
                break;
            case FirePattern.SpreadShot:
                FireSpread();
                break;
            case FirePattern.CircleShot:
                FireCircle();
                break;
            case FirePattern.AimAtPlayer:
                FireAimAtPlayer();
                break;
            case FirePattern.FastBurst:
                StartCoroutine(FireBurst(3, 0.1f));
                break;
            case FirePattern.AimedBurstWithWarning:
                StartCoroutine(FireAimedBurstWithWarning());
                break;
        }
    }

    void FireSingle()
    {
        FireBullet(Vector2.down);
    }

    void FireAimAtPlayer()
    {
        if (ShipMovement.Instance == null) return;

        Vector2 dir = (ShipMovement.Instance.transform.position - transform.position).normalized;
        FireBullet(dir);
    }

    IEnumerator FireAimedBurstWithWarning()
    {
        if (ShipMovement.Instance == null) yield break;

        WarningLaser laserScript = null;
        if (warningLaserPref != null)
        {
            GameObject laserObj = Instantiate(warningLaserPref);
            laserScript = laserObj.GetComponent<WarningLaser>();
            laserScript.Setup(transform.position, 10f, 1f);
        }

        yield return new WaitForSeconds(1f);

        Vector2 dirToPlayer = (ShipMovement.Instance.transform.position - transform.position).normalized;
        for (int i = 0; i < 5; i++)
        {
            FireBullet(dirToPlayer);
            yield return new WaitForSeconds(0.1f);
        }

        if (laserScript != null)
        {
            laserScript.StartFadeAndDestroy();
        }
    }




    IEnumerator FireBurst(int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            FireBullet(Vector2.down);
            yield return new WaitForSeconds(delay);
        }
    }

    void FireSpread()
    {
        float[] angles = { -15f, 0f, 15f };
        foreach (float angle in angles)
        {
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.down;
            FireBullet(dir);
        }
    }

    void FireCircle()
    {
        int count = 12;
        for (int i = 0; i < count; i++)
        {
            float angle = 360f * i / count;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
            FireBullet(dir);
        }
    }

    void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * bulletSpeed;
        }
    }
}
