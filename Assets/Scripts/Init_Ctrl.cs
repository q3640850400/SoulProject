using UnityEngine;
using System.Collections;
using DragonBones;

public class Init_Ctrl : MonoBehaviour {
	public static Init_Ctrl Instance = null;
	// Use this for initialization
	void Awake(){
		Instance = this;
	}
	void Start () {
		DBinit ();
	}
	
	// Update is called once per frame
	void Update () {
	
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
