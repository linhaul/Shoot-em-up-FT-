using System.Xml.Serialization;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float moveSpeed = 1f;
    private Vector3 bottomLeft, topRight;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;

        float shipHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        float shipHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        pos.x = Mathf.Clamp(pos.x, bottomLeft.x + shipHalfWidth, topRight.x - shipHalfWidth);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y + shipHalfHeight, topRight.y - shipHalfHeight);

        transform.position = pos;
    }


    private void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 playerShipDirection = new Vector2(moveX, moveY).normalized;

        _rb.linearVelocity = playerShipDirection * moveSpeed;

        ClampPosition();
    }

}
