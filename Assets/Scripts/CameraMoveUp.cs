using UnityEngine;

public class CameraMoveUp : MonoBehaviour
{
    public float scrollSpeed = 1f;
    float endY = 5f;
    public void Start()
    {

    }
    private void Update()
    {
        if (transform.position.y < endY)
            transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
    }
}
