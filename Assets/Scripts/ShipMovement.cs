using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class ShipMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private float minX, maxX, minY, maxY;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateBoundsFromManager();
    }

    private void UpdateBoundsFromManager()
    {
        if (GameBoundsManager.Instance != null)
        {
            minX = GameBoundsManager.Instance.MinX;
            maxX = GameBoundsManager.Instance.MaxX;
            minY = GameBoundsManager.Instance.MinY;
            maxY = GameBoundsManager.Instance.MaxY;
        }
    }

    private void FixedUpdate()
    {
        UpdateBoundsFromManager();

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(moveX, moveY).normalized;

        _rb.linearVelocity = direction * moveSpeed;

        Vector3 pos = transform.position;

        float halfWidth = _spriteRenderer.bounds.extents.x;
        float halfHeight = _spriteRenderer.bounds.extents.y;

        pos.x = Mathf.Clamp(pos.x, minX + halfWidth, maxX - halfWidth);
        pos.y = Mathf.Clamp(pos.y, minY + halfHeight, maxY - halfHeight);

        transform.position = pos;
    }
}
