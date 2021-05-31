using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossContl : MonoBehaviour {

	Animator anim;

	public GameObject FOrigin;
	public GameObject FPrev;

	public GameObject BossFire;
	public GameObject BossShock;

	public GameObject Clear;
	SpawnEnemy spawn;

	AnimatorClipInfo ClipInfo;
	CharacterController controller;
	AudioSource BossAudio;

	public int MoveType = 0;
	public float MoveTime = 0;
	int FireCount = 0;
	float MoveSpeed = 5;
	public float DistanceToPlayer;

	public int HP;
	int RotateMode;
	private GameObject player;

	Vector3 ToTarget;

	float AnimTime;
	float PreAnimTime;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
		BossAudio = GetComponent<AudioSource> ();
		spawn = GameObject.Find ("SpawnEnemy").gameObject.GetComponent<SpawnEnemy>();

		player = GameObject.Find ("Player").gameObject;
		HP = 2000;
		MoveType = 0;

	}
	
	// Update is called once per frame
	void Update () {

		AnimTime = anim.GetCurrentAnimatorStateInfo (0).normalizedTime;
		AnimTime = AnimTime % 1;

		//ClipInfo = anim.GetCurrentAnimatorClipInfo (0) [0];
		/*
		if (ClipInfo.clip.name == "アーマチュア|bless") {
			//Debug.Log ("これ");
			FireCount++;
			if (FireCount == 1) FireCount = 1 + (int)(Random.Range (0f, 1f) * 10);
		} else {
			FireCount = 0;
		}
		if (FireCount % 10 == 1) {
			//FPrev.transform.LookAt (FOrigin.transform);
			//FPrev.transform.Rotate (0f, 180f, 0f);
			FPrev.transform.forward= new Vector3(FPrev.transform.position.x-FOrigin.transform.position.x,FPrev.transform.position.y-FOrigin.transform.position.y,FPrev.transform.position.z-FOrigin.transform.position.z);
			Instantiate (BossFire, FPrev.transform.position, FPrev.transform.rotation);
		}
		*/

		Vector3 GoMove = new Vector3 (0, 0, 0);
		Vector3 aim = new Vector3 (0, 0, 0);

		DistanceToPlayer = Vector2.Distance (new Vector2 (this.transform.position.x, this.transform.position.z), new Vector2 (player.transform.position.x, player.transform.position.z));
		if (HP > 0) {
			if (MoveType == 0) {//待機
				anim.SetInteger ("Motion", 0);
				MoveTime+=80*Time.deltaTime;
				if (MoveTime > 50 && DistanceToPlayer > 30) {
					if (Random.Range (0.0f, 1.0f) > 0.9f && DistanceToPlayer > 70) {
						MoveType = 8;//回転
						MoveTime = 0;
						if (Random.Range (0.0f, 1.0f) > 0.5f) {
							RotateMode = 1;
						} else {
							RotateMode = 0;
						}
					}
					if (Random.Range (0.0f, 1.0f) > 0.8f) {
						MoveType = 5;//歩行
						MoveTime = 0;
					}
					if (Random.Range (0.0f, 1.0f) > 0.9f) {
						MoveType = 2;//飛び上がり
						MoveTime = 0;
					}
				}
				if (DistanceToPlayer < 70) {
					this.transform.forward = new Vector3 (player.transform.position.x - this.transform.position.x, 0, player.transform.position.z - this.transform.position.z);
					this.transform.forward = -this.transform.forward;
				}
				if (MoveTime > 30 && DistanceToPlayer > 5 && DistanceToPlayer < 30 && Random.Range (0.0f, 1.0f) > 0.8f) {
					MoveType = 1;//ブレス
					MoveTime = 0;
				}
				if (DistanceToPlayer < 5) {
					MoveType = 6;//地震
					MoveTime = 0;
					anim.SetFloat ("JishinSP", 1f);
				}
			}
			if (MoveType == 8) {//回転
				anim.SetInteger ("Motion", 5);
				if (RotateMode == 1) {
					transform.eulerAngles += new Vector3 (0f, 1f, 0f);
				} else {
					transform.eulerAngles -= new Vector3 (0f, 1f, 0f);
				}
				MoveTime+=80*Time.deltaTime;
				if (MoveTime > 50) {
					MoveType = 5;//歩行
				}
			}
			if (MoveType == 5) {//歩行
				anim.SetInteger ("Motion", 5);
				GoMove.z = -1;
				MoveTime+=80*Time.deltaTime;
				if (MoveTime > 150 && Random.Range (0.0f, 1.0f) > 0.9f) {
					MoveType = 0;//待機
					MoveTime = 0;
				}
				if (DistanceToPlayer < 70 && DistanceToPlayer > 30) {
					this.transform.forward = new Vector3 (player.transform.position.x - this.transform.position.x, 0, player.transform.position.z - this.transform.position.z);
					this.transform.forward = -this.transform.forward;
				}
			}
			if (MoveType == 1) {//ブレス攻撃
				anim.SetInteger ("Motion", 1);
				MoveTime+=80*Time.deltaTime;
				/*
				FireCount++;
				if (FireCount == 1) {
					FireCount = 1 + (int)(Random.Range (0f, 1f) * 10);
				}
				*/
				if (AnimTime%0.05f<PreAnimTime%0.05f && MoveTime > 70/* && MoveTime<150*/) {
					FPrev.transform.forward = new Vector3 (FPrev.transform.position.x - FOrigin.transform.position.x, FPrev.transform.position.y - FOrigin.transform.position.y, FPrev.transform.position.z - FOrigin.transform.position.z);
					Instantiate (BossFire, FPrev.transform.position, FPrev.transform.rotation);
					//Quaternion Rot = FPrev.transform.rotation;
					//Rot.y += 3;
					//FPrev.transform.forward = new Vector3 (FPrev.transform.position.x - FOrigin.transform.position.x, FPrev.transform.position.y - FOrigin.transform.position.y, FPrev.transform.position.z - FOrigin.transform.position.z);
					//Instantiate (BossFire, FPrev.transform.position, Rot);
				}
				if (MoveTime > 240) {
					FireCount = 0;
					MoveType = 0;//待機
					MoveTime = 0;
				}
			}
			if (MoveType == 2) {//飛び上がり
				if (MoveTime == 0) {
					BossAudio.Play ();
				}
				if (MoveTime <= 100) {
					anim.SetInteger ("Motion", 2);
				}
				if (MoveTime > 100) {
					GoMove.y = 5;
					anim.SetInteger ("Motion", 3);
				}
				MoveTime+=80*Time.deltaTime;
				if (MoveTime > 200) {
					MoveType = 3;//飛行
					MoveTime = 0;
				}
			}
			if (MoveType == 3) {//飛行
				anim.SetInteger ("Motion", 3);
				GoMove.z = -7f;
				if (MoveTime == 0) {
					ToTarget = player.transform.position;
					this.transform.forward = new Vector3 (ToTarget.x - this.transform.position.x, 0, ToTarget.z - this.transform.position.z);
					this.transform.forward = -this.transform.forward;
				}
				float distance = Vector2.Distance (new Vector2 (this.transform.position.x, this.transform.position.z), new Vector2 (ToTarget.x, ToTarget.z));
				if (distance < 10) {
					MoveType = 4;//下降
					MoveTime = 0;
				}
				MoveTime+=80*Time.deltaTime;
			}
			if (MoveType == 4) {//下降
				anim.SetInteger ("Motion", 4);
				MoveTime+=80*Time.deltaTime;
				GoMove.y = -10;
				if (transform.position.y < 0.2) {
					Vector3 pos = transform.position;
					pos.y = 0;
					transform.position = pos;
					BossAudio.Stop ();
					MoveType = 0;//待機
					MoveTime = 0;
				}
			}
			if (MoveType == 6) {//地震
				anim.SetInteger ("Motion", 6);
				MoveTime+=80*Time.deltaTime;
				if (MoveTime > 200) {
					MoveType = 0;//待機
					MoveTime = 0;
				}
				if (AnimTime>=0.5 && PreAnimTime<0.5) {
					anim.SetFloat ("JishinSP", 2f);
				}
				if (AnimTime>=0.7 && PreAnimTime<0.7) {
					Quaternion Rot = Quaternion.Euler (-90, Random.Range (0f, 1f) * 360, 0);
					Instantiate (BossShock, this.transform.position + transform.right * 10 + transform.forward * 2, Rot);
					Rot = Quaternion.Euler (-90, Random.Range (0f, 1f) * 360, 0);
					Instantiate (BossShock, this.transform.position - transform.right * 10 + transform.forward * 2, Rot);
				}
				if (AnimTime>=0.7 && PreAnimTime<0.7 && DistanceToPlayer < 40) {
					Plyer2 ply = player.GetComponent<Plyer2> ();
					if (ply.isDamage == 0) {
						ply.AtkFlame = 0;
						ply.isDamage = 50;
						ply.HP -= 100;
						ply.ImpactVector = new Vector3 (player.transform.position.x - this.transform.position.x, 0, player.transform.position.z - this.transform.position.z).normalized;
					}
				}
			}
			if (MoveType == 7) {//突撃
				anim.SetInteger ("Motion", 7);
				if (MoveTime < 30) {
				} else {
					GoMove.z = -15;
				}
				MoveTime+=80*Time.deltaTime;
				if (MoveTime < 2) {
					this.transform.forward = new Vector3 (player.transform.position.x - this.transform.position.x, 0, player.transform.position.z - this.transform.position.z);
					this.transform.forward = -this.transform.forward;
				}
				if (MoveTime > 200) {
					MoveType = 0;
					MoveTime = 0;
				}
				if (DistanceToPlayer < 30) {
					Plyer2 ply = player.GetComponent<Plyer2> ();
					if (ply.isDamage == 0) {
						ply.AtkFlame = 0;
						ply.isDamage = 50;
						ply.HP -= 100;
						ply.ImpactVector = new Vector3 (player.transform.position.x - this.transform.position.x, 0, player.transform.position.z - this.transform.position.z).normalized;
					}
				}
			}
			
			controller.Move (transform.rotation * GoMove * MoveSpeed * Time.deltaTime);
		}

		if (HP <= 0) {
			anim.SetInteger ("Motion", 8);
			if (MoveTime >= 0) {
				MoveTime = -1;
				spawn.CrushNum=-10;
				spawn.isBoss = false;
				Quaternion Rot = Quaternion.Euler (0, 0, 0);
				Vector3 Pos = new Vector3 (1300, 0, 1400);
				Instantiate (Clear, Pos, Rot);
			} else {
				MoveTime--;
			}
		}

		PreAnimTime = AnimTime;

	}

	void OnTriggerEnter(Collider other){//おそらく使ってない

		if (other.gameObject.tag == "Player") {
			MoveType = 6;//地震
			MoveTime = 0;
			anim.SetFloat ("JishinSP", 1f);
		}
		
	}
	void OnGUI () {
		//GUI.Label(new Rect(0, 30, 300, 300), "Boss HP:"+HP);
	}
}
