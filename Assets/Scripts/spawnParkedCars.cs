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
    private List<int> used;
    private System.Random rnd = new System.Random();
    
    private int spots = 12;

	public void RandomizeCars() {
		used = new List<int>{};
		
		foreach (GameObject parkedCar in parkedCars) {
			GameObject parkingSpot = getRandomSpot();
			parkedCar.transform.position = parkingSpot.transform.position;
			
			parkedCar.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
		}
	
		GameObject targetPos = getRandomSpot();
		targetSpot.transform.position = targetPos.transform.position;
		targetSpot.transform.position = new Vector3(targetSpot.transform.position.x, targetSpot.transform.position.y + 0.2f, targetSpot.transform.position.z);
	}
    
    GameObject getRandomSpot() {
    	int spot = rnd.Next(0, spots);
    	
    	while(used.Contains(spot)) {
    	    spot = rnd.Next(0, spots);
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
}
