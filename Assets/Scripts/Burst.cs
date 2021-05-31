using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour {

	float LifeTime;

	public GameObject player;

	//private ParticleSystem Particle;

	// Use this for initialization
	void Start () {

		LifeTime = 200;
		player = GameObject.Find ("Player").gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {

		//if (Particle.isStopped) {
			//Destroy (this.gameObject, 0);
		//}//???

		LifeTime-=80*Time.deltaTime;
		if (LifeTime < 0) {
			Destroy (this.gameObject, 0);
		}

		Plyer2 ply = player.GetComponent<Plyer2> ();
		float distance = Mathf.Sqrt (Mathf.Pow(player.transform.position.x-this.transform.position.x,2)+Mathf.Pow(player.transform.position.z-this.transform.position.z,2));
		if (distance < 5 && ply.isDamage==0) {
			ply.AtkFlame = 0;
			ply.isDamage = 50;
			ply.HP -= 50;
			ply.ImpactVector = new Vector3 (player.transform.position.x-this.transform.position.x,0,player.transform.position.z-this.transform.position.z).normalized;
		}
		
	}
}
