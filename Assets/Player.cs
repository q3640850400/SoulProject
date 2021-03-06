﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player Instance = null;
	private CharacterController cc=null;
	public float JUMPW;
	public float SPD;
	public float ATK;
	public float Gravity;
	private int facedr;//面向0左边，1右边
	public GameObject sp=null;
	private Rigidbody2D Rbody2D = null;
	// Use this for initialization
	void Awake(){
		Instance = this;
	}
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dr = new Vector3 (Input.GetAxis ("Horizontal"), 0f, 0f);
		if (dr != Vector3.zero) {
			move (dr);
			//turn (dr);
		}
		jump ();
	}
	void init(){
		JUMPW = 5f;
		facedr = 1;//面向右边
		SPD = 5f;
		Gravity = 9.8f;
		cc = gameObject.GetComponent<CharacterController> ();
		Rbody2D = gameObject.GetComponent<Rigidbody2D> ();
	}
	void gravity(){
		cc.Move (new Vector3(0f, -Gravity, 0f));
	}
	void move(Vector3 dr){
		if (dr.x < 0 && facedr==1){
			sp.transform.Rotate(0f,180f,0f);
			//transform.localRotation =new Vector3(0f,180f,0f);
			facedr = 0;
		}
		if (dr.x > 0 && facedr == 0) {
			sp.transform.Rotate(0f,180f,0f);
			facedr = 1;
		}
		transform.Translate (dr * SPD*Time.deltaTime);
	}
	void jump(){
		if (Input.GetButtonDown("Jump")) {
			Rbody2D.velocity =new Vector2(0f,JUMPW);
		}
	}
}
