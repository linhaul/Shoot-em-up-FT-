using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class Cursosmovement : MonoBehaviour
{
    public Camera mainCamera;
    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        mousePosition.x = Mathf.Clamp(mousePosition.x, bottomLeft.x, topRight.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, bottomLeft.y, topRight.y);

        transform.position = mousePosition;
    }
}
