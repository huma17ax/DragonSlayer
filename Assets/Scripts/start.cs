using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour {

	Animator anim;

	public GameObject magic1;

	public float time;
	float pretime;

	// Use this for initialization
	void Start () {

		anim = GetComponent < Animator > ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		time = anim.GetCurrentAnimatorStateInfo (0).normalizedTime;
		time = time % 1f;

		if (pretime<0.2f && time>=0.2f) {
			Vector3 Pos = transform.position;
			Pos.y += 1;
			Instantiate (magic1, transform.forward * 2 + Pos, transform.rotation);
		}

		pretime = time;

		
	}
}
