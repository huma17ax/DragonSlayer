using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

	public GameObject Enem;
	public GameObject thunder;
	public GameObject Boss;

	public int ReserveNum;
	public int CrushNum;

	public bool isBoss;

	// Use this for initialization
	void Start () {

		ReserveNum = 5;
		CrushNum = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (ReserveNum>0 && Random.Range (0f, 1f) < 0.01) {

			ReserveNum--;

			float angle = Random.Range (0.0f, 1.0f) * 2 * Mathf.PI;
			float range;
			if (Random.Range (0.0f, 1.0f) > 0.5f) {
				range = Random.Range (0f, 1f) * 50;
			} else {
				if (Random.Range (0.0f, 1.0f) > 0.5f) {
					range = Random.Range (0f, 1f) * 200;
				} else {
					range = Random.Range (0f, 1f) * 100;
				}
			}
			Vector3 Pos = new Vector3 (this.transform.position.x+range * Mathf.Cos(angle), 20, this.transform.position.z+range*Mathf.Sin(angle));
			Quaternion Rot = Quaternion.Euler (0,Random.Range (0f, 1f)*360,0);

			Instantiate (Enem, Pos, Rot);

		}
		/*
		if (Random.Range (0f, 1f) < 0.05) {
			float random = Random.Range (0f, 1f);
			Vector3 Pos = new Vector3(0,0,0);
			if (random < 0.25) {
				Pos = new Vector3 (-10, 10, Random.Range (0f, 1f) * 300);
			} else if (random >= 0.25 && random < 0.5) {
				Pos = new Vector3 (310, 10, Random.Range (0f, 1f) * 300);
			} else if (random >= 0.5 && random < 0.75) {
				Pos = new Vector3 (Random.Range (0f, 1f) * 300, 10, -10);
			} else if (random >= 0.75 && random < 1) {
				Pos = new Vector3 (Random.Range (0f, 1f) * 300, 10, 310);
			}

			Quaternion Rot = Quaternion.Euler (-90,Random.Range (0f, 1f)*360,0);

			Instantiate (thunder, Pos, Rot);
		}
		*/
		if (CrushNum == 10 && isBoss==false) {
			isBoss = true;
			Quaternion Rot = Quaternion.Euler (0, Random.Range (0f, 1f) * 360, 0);
			Instantiate (Boss, new Vector3(this.transform.position.x,0,this.transform.position.z), Rot);
		}
		if (CrushNum <0) {
			foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
				if (obj.activeInHierarchy && obj.tag == "Enemy") {
					Destroy (obj.gameObject, 0);
				}
			}
		}

	}
}
