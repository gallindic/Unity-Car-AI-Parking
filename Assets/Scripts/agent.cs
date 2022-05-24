using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class agent : Agent
{
	private const string HORIZONTAL = "Horizontal";
	private const string VERTICAL = "Vertical";

	private float DistanceRewardInterval = 3f;

	public Transform targetSpotTransform;
	public Transform startPosTransform;

	[SerializeField] private GameObject spawnParksController;
	[SerializeField] private GameObject carController;
	
	private Rigidbody rb;
	private float previousDistance;
	private bool isInTarget;

	[Header("Agent settings")]
	[SerializeField] private float speedThreshold;
	[SerializeField] private float rotationThreshold;
	[SerializeField] private float distanceThreshold;

	
	public void Reset() {
		spawnParksController.GetComponent<spawnParkedCars>().RandomizeCars();
		transform.position = new Vector3(startPosTransform.position.x, transform.position.y, startPosTransform.position.z);
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
		previousDistance = Vector3.Distance(this.transform.position, targetSpotTransform.position);
		isInTarget = false;
	}

	public override void OnEpisodeBegin() {
		Reset();
	}
	
	public override void Initialize() {
		rb = GetComponent<Rigidbody>();
		Reset();
	}

	public override void CollectObservations(VectorSensor sensor)
    {
        float currentSpeedKmH = rb.velocity.magnitude * 3.6f;
        Vector3 normalizedAgentPosition = (Vector3)(this.transform.position).normalized;
		Vector3 normalizedAgentRotation = GetNormalizedRotation(this.transform.rotation);

		Vector3 normalizedTargetPosition = (Vector3)(targetSpotTransform.position).normalized;
		Vector3 normalizedTargetRotation = GetNormalizedRotation(targetSpotTransform.rotation);

        sensor.AddObservation(currentSpeedKmH);
        sensor.AddObservation(normalizedAgentPosition.x);
        sensor.AddObservation(normalizedAgentPosition.z);
		sensor.AddObservation(normalizedAgentRotation.y);

		sensor.AddObservation(normalizedTargetPosition.x);
        sensor.AddObservation(normalizedTargetPosition.z);
		sensor.AddObservation(normalizedTargetRotation.y);

        sensor.AddObservation(normalizedTargetPosition.x - normalizedAgentPosition.x);
        sensor.AddObservation(normalizedTargetPosition.z - normalizedAgentPosition.z);
        sensor.AddObservation(normalizedTargetRotation.y - normalizedAgentRotation.y);
    }

	public override void Heuristic(in ActionBuffers actionsOut) {
		//Overriden
	}

	public override void OnActionReceived(ActionBuffers actionBuffers) {
		/*
		float currentThrotle = Mathf.Max(0, actionBuffers.ContinuousActions[0]);
		float currentBreaking = -Mathf.Max(0, actionBuffers.ContinuousActions[1]);
		float currentTurning = actionBuffers.ContinuousActions[2];
		int isBreaking = actionBuffers.DiscreteActions[0];
		
		carController.GetComponent<carController>().HandleMotor(currentThrotle + currentBreaking, isBreaking);
		carController.GetComponent<carController>().HandleSteering(currentTurning);
		carController.GetComponent<carController>().UpdateWheels();
		*/
		float distanceToTarget = Vector3.Distance(this.transform.position, targetSpotTransform.position);
		if (distanceToTarget < previousDistance)
		{
		    if ((int)(distanceToTarget / DistanceRewardInterval) < (int)(previousDistance / DistanceRewardInterval))
		        AddReward(0.04f);
		}
		else
        {
			AddReward(-0.06f);
        }

		float currentSpeed = Mathf.Abs(rb.velocity.magnitude * 3.6f);

		if(isInTarget) {
			Vector3 direction;
            float overlap;

            bool overlapped = Physics.ComputePenetration(
                GetComponent<Collider>(), transform.position, transform.rotation,
                targetSpotTransform.GetComponent<Collider>(), targetSpotTransform.position, targetSpotTransform.rotation,
                out direction, out overlap
            );

			float rotationDiff = Quaternion.Angle(this.transform.rotation, targetSpotTransform.rotation);

			if (rotationDiff > 90)
				rotationDiff = 180 - rotationDiff;

			AddReward(overlap);

			if(overlap > 0.5 && rotationDiff <= rotationThreshold) {
				AddReward(0.05f);
			}


			if (distanceToTarget <= distanceThreshold) {
				AddReward(0.1f);


				if (currentSpeed <= speedThreshold)
				{
					float reward = 10;
					if (rotationDiff > rotationThreshold)
						reward = (1 - GetNormalizedValue(rotationDiff, rotationThreshold, 90)) * 10;

					AddReward(reward);
					EndEpisode();
					Debug.Log("End");
					return;
				}
			} 
			
			if (distanceToTarget > distanceThreshold && currentSpeed <= 0.05f) {
				if (rotationDiff >= rotationThreshold) {
					AddReward(-0.05f);
				}
			}
		} 
		else {
			if(currentSpeed <= 0.1f) {
				AddReward(-0.02f);
			}
		}

		previousDistance = distanceToTarget;
	}

	float GetNormalizedValue(float currentValue, float minValue, float maxValue) {
        return (currentValue - minValue) / (maxValue - minValue);
    }

	Vector3 GetNormalizedRotation(in Quaternion rotation)
    {
        float normalizedX = GetNormalizedValue(rotation.eulerAngles.x, 0, 360);
        float normalizedY = GetNormalizedValue(rotation.eulerAngles.y, 0, 360);
        float normalizedZ = GetNormalizedValue(rotation.eulerAngles.z, 0, 360);

        return new Vector3(normalizedX, normalizedY, normalizedZ);
    }
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "TargetSpot"){
			isInTarget = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "TargetSpot"){
			isInTarget = false;
			AddReward(-0.05f);
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Bound") {
			AddReward(-0.04f);
		} else {
			AddReward(-1f);
		}
	}
}
