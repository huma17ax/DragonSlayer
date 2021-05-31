using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Plyer2 : MonoBehaviour {
	public float RotationY;
	public float RotationSpeedX;
	public float RotationSpeedY;
	public float MoveSpeed;
	public float GravityY;//落下速度(これにrigidbodyの落下加速度を足す)
	public float JumpCount=0;
	public float JumpHigh;
	public float DistancePlayerToCamera = 10;

	public float AtkFlame=0;
	public int MagicType = 0;
	public int MagicCoolTime;

	public int HP=100;

	public float isDamage;
	public Vector3 ImpactVector;

	public GameObject magic1;
	public GameObject magic2;

	CharacterController controller;
	Animator anim;
	GameObject model;
	Audio audio;

	float AnimTime;
	float PreAnimTime;

	// Use this for initialization
	void Start () {

		//Application.targetFrameRate = 80;
		//Time.captureFramerate = 25;

		controller = GetComponent<CharacterController> ();
		anim = GameObject.Find ("unitychan").GetComponent<Animator> ();
		model = GameObject.Find ("unitychan").gameObject;
		audio = transform.Find ("Main Camera/Audio").gameObject.GetComponent<Audio> ();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			if (Cursor.visible==true) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			} else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		AnimTime = anim.GetCurrentAnimatorStateInfo (0).normalizedTime;
		AnimTime = AnimTime % 1;

		if (isDamage == 0) {
			anim.SetInteger ("Motion", 1);
			if (AtkFlame == 0) Move ();
			Atk ();
		} else {
			anim.SetInteger ("Motion", 3);
			Impact ();
		}
		Cam ();

		PreAnimTime = AnimTime;

		if (HP <= 0) {
			SceneManager.LoadScene ("gameover");
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

	}

	//player moving process(called once per frame)
	void Move(){
		RotationY += (Input.GetAxisRaw ("Mouse X") * RotationSpeedX * Time.deltaTime);
		transform.rotation = Quaternion.Euler (0, RotationY, 0);

		Vector3 GoMove = new Vector3 (0, 0, 0);
		/*
		if (Input.GetAxisRaw ("Vertical") > 0) {
			GoMove.z = 1;
			model.transform.rotation = this.transform.rotation;
			model.transform.Rotate(0f,0f,0f);
		}
		if (Input.GetAxisRaw ("Vertical") < 0) {
			GoMove.z = -1;
			model.transform.rotation = this.transform.rotation;
			model.transform.Rotate(0f,180f,0f);
		}
		if (Input.GetAxisRaw ("Horizontal") > 0) {
			GoMove.x = 1;
			model.transform.rotation = this.transform.rotation;
			model.transform.Rotate(0f,90f,0f);
		}
		if (Input.GetAxisRaw ("Horizontal") < 0) {
			GoMove.x = -1;
			model.transform.rotation = this.transform.rotation;
			model.transform.Rotate(0f,270f,0f);
		}
		*/
		float CheckKeyVer = Input.GetAxisRaw ("Vertical");
		float CheckKeyHor = Input.GetAxisRaw ("Horizontal");

		//if (CheckKeyHor!=0 && CheckKeyVer!=0) model.transform.rotation = this.transform.rotation;
		if (CheckKeyVer > 0) {
			GoMove.z = 1;
			if (CheckKeyHor > 0) {
				GoMove.x = 1;
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f,45f,0f);
			} else if (CheckKeyHor<0) {
				GoMove.x = -1;
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f, 315f, 0f);
			} else {
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f,0f,0f);
			}
		} else if (CheckKeyVer<0) {
			GoMove.z = -1;
			if (CheckKeyHor > 0) {
				GoMove.x = 1;
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f,135f,0f);
			} else if (CheckKeyHor<0) {
				GoMove.x = -1;
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f,225f,0f);
			} else {
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f,180f,0f);
			}
		} else {
			if (CheckKeyHor > 0) {
				GoMove.x = 1;
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f,90f,0f);
			} else if (CheckKeyHor<0) {
				GoMove.x = -1;
				model.transform.rotation = this.transform.rotation;
				model.transform.Rotate (0f,270f,0f);
			} else {
			}
		}


		if (Input.GetAxisRaw ("Vertical") == 0 && Input.GetAxisRaw ("Horizontal") == 0) {
			anim.SetInteger ("Motion", 1);
			audio.WalkCall (false);
		} else {
			anim.SetInteger ("Motion", 2);
			audio.WalkCall (true);
		}

		GravityY += Physics.gravity.y * Time.deltaTime / 2;
		if (controller.isGrounded) {
			GravityY = 0;
			JumpCount = 0;
		}

		/*ジャンプ
		if (Input.GetButtonDown ("Jump") && JumpCount==0) {
			GravityY = JumpHigh;
			JumpCount = 1;
		}*/

		GoMove.y = GravityY;

		controller.Move (transform.rotation * GoMove * MoveSpeed * Time.deltaTime);

		if (JumpCount == 0) {
			//this.GetComponent<MeshRenderer> ().material.color = Color.red;
		} else {
			//this.GetComponent<MeshRenderer> ().material.color = Color.white;
		}

	}

	//player shooting magic process(called once per frame)
	void Atk(){

		if (Input.GetButtonDown ("Fire1") && AtkFlame == 0) {
			MagicType = 0;
			model.transform.rotation = this.transform.rotation;
			AtkFlame = 50;
			anim.SetInteger ("Motion", 4);
			var info = anim.GetCurrentAnimatorStateInfo (0);
			anim.Play (info.fullPathHash, 0, 0.0f);
		}
		if (Input.GetButtonDown ("Fire2") && AtkFlame == 0) {
			MagicType = 1;
			model.transform.rotation = this.transform.rotation;
			AtkFlame = 100;
			anim.SetInteger ("Motion", 5);
			var info = anim.GetCurrentAnimatorStateInfo (0);
			anim.Play (info.fullPathHash, 0, 0.0f);
		}

		AnimTime = anim.GetCurrentAnimatorStateInfo (0).normalizedTime;
		AnimTime = AnimTime % 1;

		if (MagicType == 0) {
			if (AtkFlame > 0) {
				AtkFlame-=80*Time.deltaTime;
				if (AtkFlame <= 0) AtkFlame = 0;
				anim.SetInteger ("Motion", 4);
			}
			if (AtkFlame>0 && AnimTime>=0.100f && PreAnimTime<0.100f) {
				if (Camera.main.transform.eulerAngles.x < 90) {
					Instantiate (magic1, transform.forward * 2 + transform.position, transform.rotation);
					audio.ShotCall ();
				}
				if (Camera.main.transform.eulerAngles.x > 270) {
					Instantiate (magic1, transform.forward * 2 + transform.position, Camera.main.transform.rotation);
					audio.ShotCall ();
				}
			}
		}
		if (MagicType == 1) {
			if (AtkFlame > 0) {
				AtkFlame-=80*Time.deltaTime;
				if (AtkFlame <= 0) AtkFlame = 0;
				anim.SetInteger ("Motion", 5);
			}
			if (AtkFlame>0 && AnimTime>=0.500f && PreAnimTime<0.500f) {
				//Vector3 Pos = new Vector3 (this.transform.position.x+range * Mathf.Cos(angle), 20, this.transform.position.z+range*Mathf.Sin(angle));
				Quaternion Rot = Quaternion.Euler (-90,Random.Range (0f, 1f)*360,0);
				Instantiate (magic2, transform.position, Rot);
				foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
					if (obj.activeInHierarchy && obj.tag == "Enemy") {
						float DistanceToEnemy = Vector2.Distance (new Vector2 (this.transform.position.x, this.transform.position.z), new Vector2 (obj.transform.position.x, obj.transform.position.z));
						if (DistanceToEnemy < 13 && obj.GetComponent<Enemy>().RestTime<=0) {
							obj.GetComponent<Enemy>().MoveType = 7;
							obj.GetComponent<Enemy>().RestTime = 300;
							obj.GetComponent<Enemy>().HP -= 50;
						}
					}
					if (obj.activeInHierarchy && obj.tag == "Boss") {
						float DistanceToEnemy = Vector2.Distance (new Vector2 (this.transform.position.x, this.transform.position.z), new Vector2 (obj.transform.position.x, obj.transform.position.z));
						if (DistanceToEnemy < 13 && obj.GetComponent<BossContl>().HP>0) {
							obj.GetComponent<BossContl>().HP -= 50;
						}
					}
				}
			}
		}

	}

	//camera moving process(called once per frame)
	void Cam(){

		Vector3 lookAt = transform.position+Vector3.up * 1.2f;
		if (Input.GetAxisRaw ("Mouse Y")<0 && (Camera.main.transform.eulerAngles.x<50 || Camera.main.transform.eulerAngles.x>270)) {
			Camera.main.transform.RotateAround (lookAt, transform.right, Input.GetAxisRaw ("Mouse Y")*-RotationSpeedY*Time.deltaTime);
		}
		if (Input.GetAxisRaw ("Mouse Y")>0 && (Camera.main.transform.eulerAngles.x>340 || Camera.main.transform.eulerAngles.x<90)) {
			Camera.main.transform.RotateAround (lookAt, transform.right, Input.GetAxisRaw ("Mouse Y")*-RotationSpeedY*Time.deltaTime);
		}
			
		for (int i = 0; i < 2; i++) {//preventing comeing into ground
			Camera.main.transform.position = lookAt - Camera.main.transform.forward * DistancePlayerToCamera;
			if (Camera.main.transform.position.y < 1 && DistancePlayerToCamera > 0f) {
				DistancePlayerToCamera -= 0.1f;
				i = 0;
			}
			if (Camera.main.transform.position.y > 1.2f && DistancePlayerToCamera < 10f) {
				DistancePlayerToCamera += 0.1f;
				i = 0;
			}
		}


	}

	void Impact(){
		RotationY += (Input.GetAxisRaw ("Mouse X") * RotationSpeedX * Time.deltaTime);
		transform.rotation = Quaternion.Euler (0, RotationY, 0);

		model.transform.forward = -ImpactVector;
		//ImpactVector = new Vector3 (0, 0, 0);
		//if (isDamage==50) GravityY = 1.5f;

		//GravityY += Physics.gravity.y * Time.deltaTime / 2;
		//ImpactVector.y = GravityY;

		if (isDamage>40) controller.Move (ImpactVector * 10 * Time.deltaTime);

		if (isDamage == 50) {
			var info = anim.GetCurrentAnimatorStateInfo (0);
			anim.SetInteger ("Motion", 3);
			anim.Play (info.fullPathHash, 0, 0.0f);
			audio.DamageCall ();
		}
		/*if (controller.isGrounded) {
			GravityY = 0;
		}*/
		isDamage-=80*Time.deltaTime;
		if (isDamage <= 0)
			isDamage = 0;
	}

	//indicating GUI process(called once per frame)
	void OnGUI () {
		//GUI.Label(new Rect(0, 100, 300, 400), "AnimTime:"+AnimTime);
		//GUI.Label(new Rect(0, 120, 300, 420), "AtkFlame:"+AtkFlame);
	}
}
