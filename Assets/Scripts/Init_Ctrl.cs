using UnityEngine;
using System.Collections;
using DragonBones;
using UnityEditorInternal;
using UnityEditor;

public class Init_Ctrl : MonoBehaviour {
	public static Init_Ctrl Instance = null;
	// Use this for initialization
	void Awake(){
		Instance = this;
	}
	void Start () {
		//DBinit ();
		//Animationinit();
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
	void Animationinit(){
		AddAnimation ("sword", "hardhit");
	}
	public void AddAnimation(string rolename,string actionname)
	{
		//path=AddStateTransition("Assets/Resources/airenlieren@Idle.FBX",layer);
		string path="Assets/Resources/主角/"+rolename+"@"+actionname+".FBX";

		// Creates the controller
		var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath ("Assets/Mecanim/"+rolename+".controller");

		// Add parameters
		//controller.AddParameter("TransitionNow", AnimatorControllerParameterType.Trigger);
		//controller.AddParameter("Reset", AnimatorControllerParameterType.Trigger);
		//controller.AddParameter("GotoB1", AnimatorControllerParameterType.Trigger);
		//controller.AddParameter("GotoC", AnimatorControllerParameterType.Trigger);

		// Add StateMachines
		var rootStateMachine = controller.layers[0].stateMachine;
		//var stateMachineA = rootStateMachine.AddStateMachine("smA");
		//var stateMachineB = rootStateMachine.AddStateMachine("smB");
		//var stateMachineC = stateMachineB.AddStateMachine("smC");

		// Add States
		var stateA1 = rootStateMachine.AddState(actionname);
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
		stateA1.motion = newClip;
	}
}
