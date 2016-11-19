using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	public static GameCamera Instance = null;
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
		Vector3 pos = new Vector3 (plr.transform.position.x,plr.transform.position.y,-10f);
		transform.position = pos;
	}
}
