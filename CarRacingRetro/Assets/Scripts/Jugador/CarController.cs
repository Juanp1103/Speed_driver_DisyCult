using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Motor")]
    public float accelForce = 30f;
    public float maxSpeed = 12f;
    public float reverseFactor = 0.4f;
    public float brakeForce = 40f;

    [Header("Giro")]
    public float turnSpeed = 180f;        // grados/segundo a plena velocidad
    public float minTurnSpeedFactor = 0.2f; // giro mínimo casi detenido

    [Header("Derrape (grip lateral)")]
    [Range(0f, 1f)] public float driftFactor = 0.92f; // 1 = sin agarre (hielo), 0 = riel
    public float tractionMax = 2.5f;

    private Rigidbody2D rb;
    private float throttleInput; // -1..1
    private float steerInput;    // -1..1

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 3f;
    }

    public void SetInput(float throttle, float steer)
    {
        throttleInput = throttle;
        steerInput = steer;
    }

    void FixedUpdate()
    {
        ApplyEngine();
        ApplySteering();
        KillOrthogonalVelocity();
    }

    void ApplyEngine()
    {
        Vector2 forward = transform.up;
        float currentSpeed = Vector2.Dot(rb.linearVelocity, forward);

        if (throttleInput > 0f && currentSpeed < maxSpeed)
            rb.AddForce(forward * accelForce * throttleInput);
        else if (throttleInput < 0f)
            rb.AddForce(forward * accelForce * reverseFactor * throttleInput);

        // freno si vas en reversa del input
        if (Mathf.Sign(throttleInput) != Mathf.Sign(currentSpeed) && Mathf.Abs(currentSpeed) > 0.1f)
            rb.AddForce(-forward * brakeForce * Mathf.Sign(currentSpeed));
    }

    void ApplySteering()
    {
        float speed = rb.linearVelocity.magnitude;
        float speedFactor = Mathf.Clamp01(speed / maxSpeed);
        float turn = Mathf.Lerp(minTurnSpeedFactor, 1f, speedFactor);

        // solo giras si te mueves; y giro invertido en reversa
        float dir = Vector2.Dot(rb.linearVelocity, transform.up) >= 0 ? 1f : -1f;
        rb.MoveRotation(rb.rotation - steerInput * turnSpeed * turn * dir * Time.fixedDeltaTime);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVel = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 rightVel = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVel + rightVel * driftFactor;
    }

    public float CurrentSpeed => rb.linearVelocity.magnitude;
}