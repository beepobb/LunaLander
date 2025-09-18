using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour 
{
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;

    private Rigidbody2D landerRigidBody2D;
    private float fuelAmount = 10f;

    // Runs only once before Start
    private void Awake() 
    {
        landerRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        Debug.Log(fuelAmount);
        if (fuelAmount <= 0f)
            return;

        
        if (Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
            ConsumeFuel();

        if (Keyboard.current.upArrowKey.isPressed) {
            float force = 700f;
            landerRigidBody2D.AddForce(force * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.leftArrowKey.isPressed) {
            float turnSpeed = +100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.isPressed) {
            float turnSpeed = -100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D) {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad)) {
            Debug.Log("Crashed on the terrain!");
            return;
        }
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude) {
            Debug.Log("Hard landing");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector) {
            Debug.Log("Landed on a too steep angle!");
        }

        Debug.Log("Successful Landing");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude * relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        int score = Mathf.RoundToInt((landingSpeedScore + landingAngleScore) * landingPad.GetScoreMultiplier());

        Debug.Log(score);
    }

    private void ConsumeFuel() {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }
}
