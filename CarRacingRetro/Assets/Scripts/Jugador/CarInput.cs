using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarInput : MonoBehaviour
{
    private CarController car;
    private CarControls controls;
    private Vector2 moveValue;

    void Awake()
    {
        car = GetComponent<CarController>();
        controls = new CarControls();
    }

    void OnEnable()  => controls.Car.Enable();
    void OnDisable() => controls.Car.Disable();

    void Update()
    {
        moveValue = controls.Car.Move.ReadValue<Vector2>();
        // x = steer (izq/der), y = throttle (adelante/atrás)
        car.SetInput(moveValue.y, moveValue.x);
    }
}