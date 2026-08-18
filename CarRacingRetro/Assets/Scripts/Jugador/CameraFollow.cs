using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;   // menor = más pegado al jugador
    public float zoom = 6f;
    public Vector2 offset = Vector2.zero;

    private Vector3 velocity = Vector3.zero;

    void Start() { if (Camera.main) Camera.main.orthographicSize = zoom; }

    void LateUpdate()
    {
        if (!target) return;

        Vector3 goal = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z);

        transform.position = Vector3.SmoothDamp(
            transform.position, goal, ref velocity, smoothTime);
    }
}