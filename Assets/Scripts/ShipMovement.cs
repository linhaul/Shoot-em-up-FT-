using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class ShipMovement : MonoBehaviour
{
    public static ShipMovement Instance { get; private set; }
    public float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private float minX, maxX, minY, maxY;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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

        float halfWidth = _spriteRenderer.bounds.extents.x;
        float halfHeight = _spriteRenderer.bounds.extents.y;

        float spriteHeightPixels = _spriteRenderer.sprite.rect.height;
        float pivotYPixels = _spriteRenderer.sprite.pivot.y;

        float pivotYOffsetNormalized = (pivotYPixels / spriteHeightPixels) - 0.5f;
        float pivotYOffsetWorld = pivotYOffsetNormalized * _spriteRenderer.bounds.size.y;

        Vector2 pos = _rb.position;

        pos.x = Mathf.Clamp(pos.x, minX + halfWidth, maxX - halfWidth);
        pos.y = Mathf.Clamp(pos.y, minY + halfHeight + pivotYOffsetWorld, maxY - halfHeight + pivotYOffsetWorld);

        _rb.position = pos;
        _rb.linearVelocity = direction * moveSpeed;
    }

}
