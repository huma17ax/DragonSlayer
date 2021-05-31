using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float MoveSpeed;
	public float GravityY;//落下速度(これにrigidbodyの落下加速度を足す)
	public float DistanceToPlayer2D;//平面2d距離

	public byte MoveType = 1;
	public float MoveTime;
	Vector3 aim;

	CharacterController controller;

	public GameObject target;
	public GameObject spawn;
	public GameObject shibire;
	public GameObject bakuhatsu;

	Animator anim;

	public float RestTime=0;
	public int HP;

	// Use this for initialization
	void Start () {

		controller = GetComponent<CharacterController> ();

		target = GameObject.Find ("Player").gameObject;
		spawn = GameObject.Find ("SpawnEnemy").gameObject;

		anim = transform.Find ("dszako").gameObject.GetComponent<Animator> ();
		shibire = transform.Find ("shibire").gameObject;

		HP = 150;
		
	}
	
	// Update is called once per frame
	void Update () {

		Move ();

	}

	//enemy moving process(called once per frame)
	void Move(){

		Vector3 GoMove = new Vector3 (0, 0, 0);

		GravityY += Physics.gravity.y * Time.deltaTime / 2;
		if (controller.isGrounded) {
			GravityY = 0;
		}

		DistanceToPlayer2D = Vector2.Distance (new Vector2(this.transform.position.x,this.transform.position.z), new Vector2(target.transform.position.x,target.transform.position.z));
		//DistanceToPlayer2D = 100;

		if (MoveType != 7 && MoveTime<=0) {//行動モード変更　7（被ダメージ）以外の時のみ
			if (DistanceToPlayer2D > 50) {
				if (MoveType != 1 && MoveType != 2 && MoveType != 3) {
					MoveType = 3;
				}
				if (MoveType == 1 && Random.Range (0.0f, 1.0f) < 0.04f) {
					MoveType = 3;
				}
				if (MoveType == 2 && Random.Range (0.0f, 1.0f) < 0.04f) {
					MoveType = 3;
				}
				if (MoveType == 3 && Random.Range (0.0f, 1.0f) < 0.01f) {
					MoveType = 1;
				}
				if (MoveType == 3 && Random.Range (0.0f, 1.0f) > 0.99f) {
					MoveType = 2;
				}
				anim.SetInteger ("Motion", 1);
			}
			if (DistanceToPlayer2D > 10 && DistanceToPlayer2D < 50) {
				if (MoveType != 5) MoveType = 0;
				if (MoveType == 5 && DistanceToPlayer2D > 20) {
					MoveType = 0;
				}
				anim.SetInteger ("Motion", 1);
			}
			if (DistanceToPlayer2D > 2 && DistanceToPlayer2D <= 10) {
				if (MoveType != 4 && MoveType != 5) {
					MoveType = 4;
					anim.SetInteger ("Motion", 1);
				}
				if (MoveType == 4 && Random.Range (0.0f, 1.0f) < 0.005f) {
					aim = new Vector3 (target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z);
					MoveType = 5;
					GravityY = 0.8f;
					anim.SetInteger ("Motion", 3);
				}
			}
			if (DistanceToPlayer2D <= 2) {
				if (MoveType != 5) {
					MoveType = 6;
					anim.SetInteger ("Motion", 2);
				}
			}
		}
		if (MoveTime > 0) MoveTime-=80*Time.deltaTime;
			
		if (MoveType == 1) {//回転
			transform.Rotate(new Vector3(0,1,0));
		}
		if (MoveType == 2) {//回転
			transform.Rotate(new Vector3(0,-1,0));
		}
		if (MoveType == 3) {//前進
			GoMove.z = 1;
		}
		if (MoveType == 0) {//プレイヤー発見
			aim = new Vector3 (target.transform.position.x-this.transform.position.x,0,target.transform.position.z-this.transform.position.z);
			this.transform.forward = aim;
			GoMove.z = 1;
		}
		if (MoveType == 4) {//プレイヤー周囲
			aim = new Vector3 (target.transform.position.x-this.transform.position.x,0,target.transform.position.z-this.transform.position.z);
			this.transform.forward = aim;
			//GoMove.x = 0.4f;
			anim.SetInteger ("Motion", 4);
		}
		if (MoveType == 5) {//突進
			GoMove.z = 5;
			Plyer2 ply = target.GetComponent<Plyer2> ();
			if (DistanceToPlayer2D < 1 && ply.isDamage == 0) {
				ply.AtkFlame = 0;
				ply.isDamage = 50;
				ply.HP -= 50;
				ply.ImpactVector = new Vector3 (target.transform.position.x-this.transform.position.x,0,target.transform.position.z-this.transform.position.z).normalized;
			}
		}
		if (MoveType == 6) {//プレイヤー近づきすぎ
			Plyer2 ply = target.GetComponent<Plyer2> ();
			if (ply.isDamage == 0) {
				ply.AtkFlame = 0;
				ply.isDamage = 50;
				ply.HP -= 50;
				ply.ImpactVector = new Vector3 (target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z).normalized;
			}
			MoveTime = 10;
			MoveType = 4;
		}
		if (MoveType == 7) {//被ダメージ
			if (RestTime > 299) {
				//Vector3 Pos = new Vector3 (this.transform.position.x, 2, this.transform.position.z);
				//Quaternion Rot = Quaternion.Euler (-90,Random.Range (0f, 1f)*360,0);
				//Instantiate (shibire, Pos, this.transform.rotation);
				shibire.SetActive(true);
			}
			RestTime-=80*Time.deltaTime;
			GoMove.z = 0;
			if (RestTime < 0) {
				MoveType = 3;
				shibire.SetActive(false);
			}
			anim.SetInteger ("Motion", 4);
		}

		GoMove.y = GravityY;

		controller.Move (transform.rotation * GoMove * MoveSpeed * Time.deltaTime);

		if (HP <= 0) {
			Instantiate (bakuhatsu, transform.position, transform.rotation);
			SpawnEnemy SE = spawn.GetComponent<SpawnEnemy> ();
			SE.ReserveNum++;
			SE.CrushNum++;
			Destroy (this.gameObject, 0);
		}

	}
}
