using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class carController : MonoBehaviour
{
	private const string HORIZONTAL = "Horizontal";
	private const string VERTICAL = "Vertical";

	private float horizontalInput;
	private float verticalInput;
	private float steeringAngle;
	private bool isBreaking;
	
	[SerializeField] private float motorForce;
	[SerializeField] private float breakForce;
	[SerializeField] private float maxSteeringAngle;
	
	[Header("Wheels")]
	[SerializeField] private WheelCollider frontLeftWheelCollider;
	[SerializeField] private WheelCollider frontRightWheelCollider;
	[SerializeField] private WheelCollider rearLeftWheelCollider;
	[SerializeField] private WheelCollider rearRightWheelCollider;
	
	[SerializeField] private Transform frontLeftWheelTransform;
	[SerializeField] private Transform frontRightWheelTransform;
	[SerializeField] private Transform rearLeftWheelTransform;
	[SerializeField] private Transform rearRightWheelTransform;
	
	[Header("UI Text")]
	[SerializeField] private Text throtle;
	[SerializeField] private Text steering;
	[SerializeField] private Text handbrake;
	

	private void FixedUpdate() {
		GetInput();
		HandleMotor(verticalInput, isBreaking);
		HandleSteering(horizontalInput);
		UpdateWheels();

		throtle.text = "Throtle: " + verticalInput.ToString("0.0");
		steering.text = "Steering: " + horizontalInput.ToString("0.0");
		handbrake.text = "Handbrake: " + (isBreaking ? "Yes" : "No");
	}
	
	public void HandleMotor(float input, bool isBreaking) {
		frontLeftWheelCollider.motorTorque = input * motorForce * 0.5f;
		frontRightWheelCollider.motorTorque = input * motorForce * 0.5f;
		
		if(isBreaking) {
			ApplyBreaking(breakForce);
		} else {
			ApplyBreaking(0f);
		}
	}
	
	public void ApplyBreaking(float force) {
		frontLeftWheelCollider.brakeTorque = force;
		frontRightWheelCollider.brakeTorque = force;
		rearLeftWheelCollider.brakeTorque = force;
		rearRightWheelCollider.brakeTorque = force;
	}
	
	public void HandleSteering(float input) {
		steeringAngle = maxSteeringAngle * input;
		frontLeftWheelCollider.steerAngle = steeringAngle;
		frontRightWheelCollider.steerAngle = steeringAngle;
	}
	
	public void UpdateWheels() {
		UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
		UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
		UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
		UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
	}
	
	public void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
		horizontalInput = Input.GetAxis(HORIZONTAL);
		verticalInput = Input.GetAxis(VERTICAL);
		isBreaking = Input.GetKey(KeyCode.Space);
	}
	
	private void GetInput() {
		horizontalInput = Input.GetAxis(HORIZONTAL);
		verticalInput = Input.GetAxis(VERTICAL);
		isBreaking = Input.GetKey(KeyCode.Space);
	}

	float GetNormalizedValue(float currentValue, float minValue, float maxValue) {
        return (currentValue - minValue) / (maxValue - minValue);
    }
}
