using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : MonoBehaviour {

	public float MoveSpeed;
	public GameObject Burst;

	public GameObject player;

	// Use this for initialization
	void Start () {

		player = GameObject.Find ("Player").gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Translate (Vector3.forward * MoveSpeed * Time.deltaTime);
			
		if (transform.position.y < -0.5f) {//一回だけ
			Quaternion Rot = Quaternion.Euler (-90, 0, Random.Range (0f, 1f) * 360);
			Instantiate (Burst, transform.position, Rot);
			Destroy (this.gameObject, 0);
		}
		
	}
}
