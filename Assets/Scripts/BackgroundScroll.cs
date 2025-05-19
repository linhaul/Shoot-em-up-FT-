using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // скорость движения фона
    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset.y += scrollSpeed * Time.deltaTime;
        rend.material.mainTextureOffset = offset;
    }
}
