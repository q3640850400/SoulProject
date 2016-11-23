using UnityEngine;
using System.Collections;
using DragonBones;

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
		DBinit ();
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
	void DBinit(){
		// Load data.
		UnityFactory.factory.LoadDragonBonesData("DragonBones/Dragon_ske"); // DragonBones file path (without suffix)
		UnityFactory.factory.LoadTextureAtlasData("DragonBones/Dragon_tex"); //Texture atlas file path (without suffix) 
		// Create armature.
		var armatureComponent =UnityFactory.factory.BuildArmatureComponent("Dragon"); // Input armature name
		// Play animation.
		armatureComponent.animation.Play("walk");
		//UnityFactory.factory.ParseDragonBonesData("DragonBones/Dragon_ske");
		//UnityFactory.factory.ParseTextureAtlasData("DragonBones/Dragon_tex");
		UnityFactory.factory.ReplaceSlotDisplay(null,"Dragon", "legL","parts/legL", armatureComponent.armature.GetSlot("handL"));

		// Change armatureposition.
		armatureComponent.transform.localPosition = new Vector3(0.0f, 0.0f,0.0f);

	}
}
