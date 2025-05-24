using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    private float buffer = 0.5f; 

    private void Update()
    {
        if (GameBoundsManager.Instance == null) return;

        float minX = GameBoundsManager.Instance.MinX;
        float maxX = GameBoundsManager.Instance.MaxX;
        float minY = GameBoundsManager.Instance.MinY;
        float maxY = GameBoundsManager.Instance.MaxY;

        Vector3 pos = transform.position;

        if (pos.x < minX - buffer || pos.x > maxX + buffer || pos.y < minY - buffer || pos.y > maxY + buffer)
        {
            Destroy(gameObject);
        }
    }
}
