using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic1 : MonoBehaviour {

	public float MoveSpeed;

	private ParticleSystem Particle;

	public GameObject Landing;

	float SaveTime = 0;

	// Use this for initialization
	void Start () {

		Particle = transform.Find ("ETF_LightningSpark").gameObject.GetComponent<ParticleSystem> ();
		//Particle.main.simulationSpeed = 0.5f;

	}
	
	// Update is called once per frame
	void Update () {

		//Vector3 up = new Vector3 (0, 1, 0);

		//this.transform.position = this.transform.position + up/10;

		if (SaveTime < 10f) {
			SaveTime+=80*Time.deltaTime;
		} else {
			transform.Translate (Vector3.forward * MoveSpeed * Time.deltaTime);
		}
		if (Particle.time>2.5f) {
			//Instantiate (Landing, this.transform.position, this.transform.rotation);
			Destroy (this.gameObject, 0);
		}
		
	}

	void OnTriggerStay(Collider other) {

		if (other.gameObject.tag == "Enemy") {
			Enemy En = other.GetComponent<Enemy> ();
			if (En.RestTime<=0 || En.RestTime>30) {
				En.MoveType = 7;
				En.RestTime = 30;
				En.HP -= 50;
			}
			Destroy (this.gameObject, 0);
			Instantiate (Landing, this.transform.position, this.transform.rotation);
		}

		if (other.gameObject.tag == "Boss") {
			BossContl Bos = other.GetComponent<BossContl> ();
			if (Bos.HP > 0) {
				Bos.HP -= 50;
				if (Bos.DistanceToPlayer > 70 && Bos.MoveType != 7) {
					Bos.MoveType = 7;
					Bos.MoveTime = 0;
				}
				Destroy (this.gameObject, 0);
				Instantiate (Landing, this.transform.position, this.transform.rotation);
			}
		}

		if (other.gameObject.tag == "Start_Boss") {
			Destroy (this.gameObject, 0);
			Instantiate (Landing, this.transform.position, this.transform.rotation);
		}

	}
}
