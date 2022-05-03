using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	

	private void FixedUpdate() {
		GetInput();
		HandleMotor();
		HandleSteering();
		UpdateWheels();
	}
	
	private void HandleMotor() {
		frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
		frontRightWheelCollider.motorTorque = verticalInput * motorForce;
		breakForce = isBreaking ? breakForce : 0f;
		
		if(isBreaking) {
			ApplyBreaking();
		}
	}
	
	private void ApplyBreaking() {
		frontLeftWheelCollider.brakeTorque = breakForce;
		frontRightWheelCollider.brakeTorque = breakForce;
		rearLeftWheelCollider.brakeTorque = breakForce;
		rearRightWheelCollider.brakeTorque = breakForce;
	}
	
	private void HandleSteering() {
		steeringAngle = maxSteeringAngle * horizontalInput;
		frontLeftWheelCollider.steerAngle = steeringAngle;
		frontRightWheelCollider.steerAngle = steeringAngle;
	}
	
	private void UpdateWheels() {
		UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
		UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
		UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
		UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
	}
	
	private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
		Vector3 pos;
		Quaternion rot;
		wheelCollider.GetWorldPose(out pos, out rot);
		wheelTransform.rotation = rot;
		wheelTransform.position = pos;
	}
	
	private void GetInput() {
		horizontalInput = Input.GetAxis(HORIZONTAL);
		verticalInput = Input.GetAxis(VERTICAL);
		isBreaking = Input.GetKey(KeyCode.Space);
	}
}
