using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {
	private Animator animator=null;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Idle(){
		animator.SetInteger ("stat", 0);
	}
}
