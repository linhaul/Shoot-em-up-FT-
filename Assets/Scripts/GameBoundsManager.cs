using UnityEngine;

public class GameBoundsManager : MonoBehaviour
{
    public static GameBoundsManager Instance { get; private set; }

    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public Camera mainCamera;

    public float MinX { get; private set; }
    public float MaxX { get; private set; }
    public float MinY { get; private set; }
    public float MaxY { get; private set; }

    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CalculateBounds();
    }

    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            CalculateBounds();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }


    public void CalculateBounds()
    {
        Vector3[] leftCorners = new Vector3[4];
        leftPanel.GetWorldCorners(leftCorners);
        float leftPanelRightEdgeX = leftCorners[3].x;

        Vector3[] rightCorners = new Vector3[4];
        rightPanel.GetWorldCorners(rightCorners);
        float rightPanelLeftEdgeX = rightCorners[0].x;

        float camHeight = 2f * mainCamera.orthographicSize;
        float camY = mainCamera.transform.position.y;

        MinX = leftPanelRightEdgeX;
        MaxX = rightPanelLeftEdgeX;

        MinY = camY - camHeight / 2f;
        MaxY = camY + camHeight / 2f;
    }
}
