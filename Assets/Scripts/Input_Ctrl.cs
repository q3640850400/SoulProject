using UnityEngine;
using System.Collections;

public class Input_Ctrl : MonoBehaviour {
	public static Input_Ctrl Instance=null;

	public int unitid = 0;
	// Use this for initialization
	void Awake(){
		Instance = this;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//move ();
		jump ();
		atk ();
	}
	//移动
//	void move(){
//		Vector3 dr = new Vector3 (Input.GetAxis ("Horizontal"), 0f, 0f);
//		if (dr != Vector3.zero) {
//			if (dr.x < 0 && facedr==1){
//				sp.transform.Rotate(0f,180f,0f);
//				//transform.localRotation =new Vector3(0f,180f,0f);
//				facedr = 0;
//			}
//			if (dr.x > 0 && facedr == 0) {
//				sp.transform.Rotate(0f,180f,0f);
//				facedr = 1;
//			}
//			transform.Translate (dr * SPD*Time.deltaTime);
//		}
//	}
	//跳跃
	void jump(){
		if (Input.GetButtonDown("Jump")) {
			//Rbody2D.velocity =new Vector2(0f,JUMPW);
			// 讯息格式: "jump:poid/unitid"
			Net_Ctrl.Instance.ag.Send("jump:"+Net_Ctrl.Instance.ag.poid.ToString()+"/"+unitid.ToString());
		}
	}
	//攻击
	void atk(){
		int atkid=-1;
		if (Input.GetButtonDown ("Fire1")) {
			atkid = 0;
		}
		if (Input.GetButtonDown ("Fire2")) {
			atkid = 1;
		}
		if (Input.GetButtonDown ("Fire3")) {
			atkid = 2;
		}
		if (atkid != -1) {
			// 讯息格式: "atk:poid/unitid/atkid"
			Net_Ctrl.Instance.ag.Send("atk:"+Net_Ctrl.Instance.ag.poid.ToString()+"/"+unitid.ToString()+"/"+atkid.ToString());
		}
	}
	void offatk(){
		int atkid=-1;
		if (Input.GetButtonDown ("Fire1")) {
			atkid = 0;
		}
		if (Input.GetButtonDown ("Fire2")) {
			atkid = 1;
		}
		if (Input.GetButtonDown ("Fire3")) {
			atkid = 2;
		}
		if (atkid != -1) {
			// 讯息格式: "atk:poid/unitid/atkid"
			string msg="atk:"+Net_Ctrl.Instance.ag.poid.ToString()+"/"+unitid.ToString()+"/"+atkid.ToString();
			Net_Ctrl.Instance.OnGameMessageIn(msg,0,null);
		}
	}
}
