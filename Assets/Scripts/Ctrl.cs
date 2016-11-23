using UnityEngine;
using System.Collections;

public class Ctrl : MonoBehaviour {
	public static Ctrl Instance = null;
	// Use this for initialization
	void Awake(){
		Instance = this;
		DontDestroyOnLoad (this);
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
