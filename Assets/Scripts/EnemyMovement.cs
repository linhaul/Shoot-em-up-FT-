using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType { StraightDown, SineWave, ZigZag, HoverThenRush };
    public MovementType movementType = MovementType.StraightDown;

    public float speed = 2f;
    public float sineFrequency = 2f;
    public float sineAmplitude = 1f;

    public float hoverTime = 1.5f;
    private float hoverTimer = 0f;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        switch (movementType)
        {
            case MovementType.StraightDown:
                StraightDownMovement();
                break;
            case MovementType.SineWave:
                SineWaveMovement();
                break;
            case MovementType.ZigZag:
                ZigZagMovement();
                break;
            case MovementType.HoverThenRush:
                HoverThenRushMovement();
                break;
        }
    }

    void StraightDownMovement()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void SineWaveMovement()
    {
        float sineX = Mathf.Sin(Time.time * sineFrequency) * sineAmplitude;
        transform.position += new Vector3(sineX, -speed * Time.deltaTime, 0f);
    }

    void ZigZagMovement()
    {
        float zigzagX = Mathf.PingPong(Time.time * sineFrequency, sineAmplitude * 2) - sineAmplitude;
        transform.position += new Vector3(zigzagX, -speed * Time.deltaTime, 0f);
    }

    void HoverThenRushMovement()
    {
        hoverTimer += Time.deltaTime;
        if (hoverTimer < hoverTime) return;
        transform.Translate(Vector2.down * speed * 2f * Time.deltaTime);
    }
}
