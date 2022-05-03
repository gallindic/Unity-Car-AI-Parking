using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class spawnParkedCars : MonoBehaviour
{
    public GameObject[] parkedCars;
    public GameObject[] parkingSpots;
    public GameObject targetSpot;
    private List<int> used = new List<int>{};
    private System.Random rnd = new System.Random();

    void Start()
    {
        foreach (GameObject parkedCar in parkedCars) {
	  GameObject parkingSpot = getRandomSpot();
	  parkedCar.transform.position = parkingSpot.transform.position;
	  
	  int rotate = rnd.Next(0, 2);
	  
	  if(rotate == 1) {
	  	parkedCar.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
	  }
	}
	
	GameObject targetPos = getRandomSpot();
	targetSpot.transform.position = targetPos.transform.position;
	targetSpot.transform.position = new Vector3(targetSpot.transform.position.x, targetSpot.transform.position.y + 0.025f, targetSpot.transform.position.z);
    }
    
    GameObject getRandomSpot() {
    	int spot = rnd.Next(0, 13);
    	
    	while(used.Contains(spot)) {
    	    spot = rnd.Next(0, 13);
    	}
    	
    	used.Add(spot);
    	
    	return parkingSpots[spot];
    }


    bool containsInt(int num) {
    	foreach(int n in used) {
    		if(n == num) {
    			return true;
    		}
    	}
    	
    	return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
