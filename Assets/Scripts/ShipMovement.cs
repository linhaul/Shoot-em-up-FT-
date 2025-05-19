using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float moveSpeed = 1f;
    private Vector3 bottomLeft, topRight;
    private void Start()
    {
        bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
    }

    private void Update()
{
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    Vector3 playerShipDirection = new Vector3(moveX, moveY, 0f).normalized; // Нормализуем

    transform.position += playerShipDirection * moveSpeed * Time.deltaTime;

    Vector3 clampedPosition = transform.position;
    float shipHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    float shipHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
    clampedPosition.x = Mathf.Clamp(clampedPosition.x, bottomLeft.x + shipHalfWidth, topRight.x - shipHalfWidth);
    clampedPosition.y = Mathf.Clamp(clampedPosition.y, bottomLeft.y + shipHalfHeight, topRight.y - shipHalfHeight);
    transform.position = clampedPosition;
}

}
