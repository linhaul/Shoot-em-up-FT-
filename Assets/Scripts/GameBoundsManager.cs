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

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));

        MinX = leftPanelRightEdgeX;
        MaxX = rightPanelLeftEdgeX;

        MinY = bottomLeft.y;
        MaxY = topRight.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(MinX, MinY), new Vector3(MinX, MaxY));
        Gizmos.DrawLine(new Vector3(MaxX, MinY), new Vector3(MaxX, MaxY));
        Gizmos.DrawLine(new Vector3(MinX, MinY), new Vector3(MaxX, MinY));
        Gizmos.DrawLine(new Vector3(MinX, MaxY), new Vector3(MaxX, MaxY));
    }
}
