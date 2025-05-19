using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float backgroundHeight = 24.4f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        if (transform.position.y <= -backgroundHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + backgroundHeight * 2f, transform.position.z);
        }
    }
}
