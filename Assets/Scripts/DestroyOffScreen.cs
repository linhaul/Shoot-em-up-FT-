using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    private Camera mainCamera;
    private float buffer = 1f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);

        if (screenPos.x < -buffer || screenPos.x > 1 + buffer ||
            screenPos.y < -buffer || screenPos.y > 1 + buffer)
        {
            Destroy(gameObject);
        }
    }
}
