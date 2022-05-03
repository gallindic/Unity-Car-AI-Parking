using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agent : MonoBehaviour
{
    	public GameObject targetSpot;
    	
        [Header("Sensors")]
	[SerializeField] private float sensorLength;
	
	public Transform frontCenterSensorPosition;
	public Transform frontLeftSensorPosition;
	public Transform frontRightSensorPosition;
	public Transform rearCenterSensorPosition;
	public Transform rearLeftSensorPosition;
	public Transform rearRightSensorPosition;
	public Transform leftSensorPosition;
	public Transform rightSensorPosition;


	void FixedUpdate() {
		Sensors();
	}
	
	private void singleSensor(Transform sensorPos) {
		RaycastHit hit;
		Vector3 sensorStartPos = sensorPos.position;
		Vector3 direction = sensorPos.rotation * Vector3.forward;

		if(Physics.Raycast(sensorStartPos, direction, out hit, sensorLength)) {
			Debug.DrawLine(sensorStartPos, hit.point);
		}
	}
	
	private void Sensors() {
		singleSensor(frontLeftSensorPosition);
		singleSensor(frontCenterSensorPosition);
		singleSensor(frontRightSensorPosition);
		singleSensor(rearCenterSensorPosition);
		singleSensor(rearLeftSensorPosition);
		singleSensor(rearRightSensorPosition);
		singleSensor(leftSensorPosition);
		singleSensor(rightSensorPosition);
	}

}
