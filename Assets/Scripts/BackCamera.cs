using UnityEngine;
using System.Collections;

public class BackCamera : MonoBehaviour {
	public static BackCamera Instance=null;
	private GameObject plr = null;
	// Use this for initialization
	void Awake(){
		Instance = this;
	}
	void Start () {
		plr = GameObject.FindGameObjectWithTag ("Player");
		if (plr == null) {
			Debug.Log ("找不到玩家");
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3 (plr.transform.position.x/6,plr.transform.position.y/6,-10f);
		transform.position = pos;
	}
}
