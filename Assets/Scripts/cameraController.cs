using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
	public GameObject target;
	public Vector3 myPos;
	public Vector3 myRot;

	void Start()
	{
		transform.rotation = Quaternion.Euler(myRot);;
	}

	void Update()
	{
		transform.position = target.transform.position + myPos;
	}
}
