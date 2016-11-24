using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player Instance = null;
	//private CharacterController cc=null;
	public float JUMPW = 5f;
	public float SPD= 5f;
	public float ATK;
	public float Gravity= 9.8f;
	private int facedr = 1;//面向右边;//面向0左边，1右边
	public GameObject sp=null;
	private Rigidbody2D Rbody2D = null;
	public int unitid = 0;
	public bool online=false;
	private Animator animator = null;
	// Use this for initialization
	void Awake(){
		Instance = this;
	}
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		move ();
		jump ();
		atk ();
	}
	void init(){
		Gravity = 9.8f;
		//cc = gameObject.GetComponent<CharacterController> ();//没有用到CharacterController
		Rbody2D = gameObject.GetComponent<Rigidbody2D> ();
		animator = this.GetComponentInChildren<Animator> ();
	}
	void example(){
		Net_Ctrl.Instance.ag.Send("jump:"+Net_Ctrl.Instance.ag.poid.ToString()+"/"+unitid+"/"+
			transform.position.x.ToString()+","+transform.position.y.ToString()+","+transform.position.z.ToString());
	}
	//暂时未用
	void gravity(){
		//cc.Move (new Vector3(0f, -Gravity, 0f));
	}
	//位置更新
	public void resetPos(float posx,float posy,float posz,int facedr){
		transform.position = new Vector3 (posx, posy, posz);
		sp.transform.localRotation = Quaternion.Euler(0, 180-facedr*180, 0);
	}
	public void onjump(){
		Rbody2D.velocity =new Vector2(0f,JUMPW);
		animator.SetInteger ("stat", 1);
	}
	//移动
	void move(){
		int stat = animator.GetInteger ("stat");
		if ( stat== 0 || stat==1) {
			Vector3 dr = new Vector3 (Input.GetAxis ("Horizontal"), 0f, 0f);
			if (dr != Vector3.zero) {
				if (dr.x < 0 && facedr == 1) {
					//sp.transform.Rotate(0f,180f,0f);
					sp.transform.localRotation = Quaternion.Euler (0, 180, 0);
					facedr = 0;
				}
				if (dr.x > 0 && facedr == 0) {
					//sp.transform.Rotate(0f,-180f,0f);
					sp.transform.localRotation = Quaternion.Euler (0, 0, 0);
					facedr = 1;
				}
				transform.Translate (dr * SPD * Time.deltaTime);
			}
		}
		if (online) {
			// 讯息格式: "pos:poid/unitid/posx/posy/posz/facedr"
			Net_Ctrl.Instance.ag.Send ("pos:" + Net_Ctrl.Instance.ag.poid.ToString () + "/" + unitid.ToString ()+transform.position.x.ToString()+"/"+transform.position.y.ToString()+"/"+transform.position.z.ToString()+"/"+facedr.ToString());
		}
	}
	//跳跃
	void jump(){
		if (Input.GetButtonDown("Jump")) {
			Rbody2D.velocity =new Vector2(0f,JUMPW);
			animator.SetInteger ("stat", 1);
			if (online) {
				// 讯息格式: "jump:poid/unitid"
				Net_Ctrl.Instance.ag.Send ("jump:" + Net_Ctrl.Instance.ag.poid.ToString () + "/" + unitid.ToString ());
			}
		}
	}
	//攻击
	void atk(){
		int atkid=-1;
		if (Input.GetButtonDown ("Fire1")) {
			animator.SetInteger ("stat", 2);
			atkid = 0;
		}
		if (Input.GetButtonDown ("Fire2")) {
			animator.SetInteger ("stat", 3);
			atkid = 1;
		}
		if (Input.GetButtonDown ("Fire3")) {
			animator.SetInteger ("stat", 4);
			atkid = 2;
		}
		if (online && atkid != -1) {
			// 讯息格式: "atk:poid/unitid/atkid"
			Net_Ctrl.Instance.ag.Send("atk:"+Net_Ctrl.Instance.ag.poid.ToString()+"/"+unitid.ToString()+"/"+atkid.ToString());
		}
	}
}
