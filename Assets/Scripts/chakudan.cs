using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chakudan : MonoBehaviour {

	float LifeTime;

	// Use this for initialization
	void Start () {
		LifeTime = 75;
	}
	
	// Update is called once per frame
	void Update () {
		LifeTime-=80*Time.deltaTime;
		if (LifeTime < 0) {
			Destroy (this.gameObject, 0);
		}
	}
}
