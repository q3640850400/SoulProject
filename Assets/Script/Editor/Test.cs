using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;

// Create a menu item that causes a new controller and statemachine to be created.

public class SM : MonoBehaviour {
	[MenuItem ("MyMenu/CreateController")]
	static void CreateController () {

		// Creates the controller
		var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath ("Assets/Mecanim/StateMachineTransitions.controller");

		// Add parameters
		controller.AddParameter("TransitionNow", AnimatorControllerParameterType.Trigger);
		controller.AddParameter("Reset", AnimatorControllerParameterType.Trigger);
		controller.AddParameter("GotoB1", AnimatorControllerParameterType.Trigger);
		controller.AddParameter("GotoC", AnimatorControllerParameterType.Trigger);

		// Add StateMachines
		var rootStateMachine = controller.layers[0].stateMachine;
		var stateMachineA = rootStateMachine.AddStateMachine("smA");
		var stateMachineB = rootStateMachine.AddStateMachine("smB");
		var stateMachineC = stateMachineB.AddStateMachine("smC");

		// Add States
		var stateA1 = stateMachineA.AddState("stateA1");
		var stateB1 = stateMachineB.AddState("stateB1");
		var stateB2 = stateMachineB.AddState("stateB2");
		stateMachineC.AddState("stateC1");
		var stateC2 = stateMachineC.AddState("stateC2"); // don’t add an entry transition, should entry to state by default

		// Add Transitions
		var exitTransition = stateA1.AddExitTransition();
		exitTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "TransitionNow");
		exitTransition.duration = 0;

		var resetTransition = stateMachineA.AddAnyStateTransition(stateA1);
		resetTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "Reset");
		resetTransition.duration = 0;

		var transitionB1 = stateMachineB.AddEntryTransition(stateB1);
		transitionB1.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "GotoB1");
		stateMachineB.AddEntryTransition(stateB2);
		stateMachineC.defaultState = stateC2;
		var exitTransitionC2 = stateC2.AddExitTransition();
		exitTransitionC2.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "TransitionNow");
		exitTransitionC2.duration = 0;

		var stateMachineTransition = rootStateMachine.AddStateMachineTransition(stateMachineA, stateMachineC);
		stateMachineTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "GotoC");
		rootStateMachine.AddStateMachineTransition(stateMachineA, stateMachineB);	
	}
	[MenuItem ("MyMenu/AddSwordAnimation")]
	static void AddSwordAnimation(){
		string rolename;
		string actionname;
		AnimatorState stateA1;
		AnimationClip newClip;

		// Creates the controller
		rolename = "sword";
		var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath ("Assets/Mecanim/"+rolename+".controller");

		// Add parameters
		controller.AddParameter("stat", AnimatorControllerParameterType.Int);

		// Add StateMachines
		var rootStateMachine = controller.layers[0].stateMachine;

		// Add States
		AnimatorState idle=MyAddState(rootStateMachine,"sword","idle");
		AnimatorState run=MyAddState(rootStateMachine,"sword","run");
		AnimatorState gun=MyAddState(rootStateMachine,"sword","gun");
		AnimatorState atkidle=MyAddState(rootStateMachine,"sword","atkidle");
		AnimatorState hardhit=MyAddState(rootStateMachine,"sword","hardhit");
		AnimatorState jump=MyAddState(rootStateMachine,"sword","jump");
		AnimatorState lighthit=MyAddState(rootStateMachine,"sword","lighthit");
		AnimatorState takeweapon=MyAddState(rootStateMachine,"sword","takeweapon");
		AnimatorState putweapon=MyAddState(rootStateMachine,"sword","putweapon");

		// Add Transitions
		var transition_idle2run=idle.AddTransition(run);
		transition_idle2run.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "stat");
		transition_idle2run.hasExitTime = false;

		var transition_idle2gun=idle.AddTransition(gun);
		transition_idle2gun.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 2, "stat");
		transition_idle2gun.hasExitTime = false;

		var transition_run2gun=run.AddTransition(gun);
		transition_run2gun.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 3, "stat");
		transition_run2gun.hasExitTime = false;


	}
	static AnimatorState MyAddState(AnimatorStateMachine sm,string rolename,string actionname)
	{
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath("Assets/Resources/主角/"+rolename+"@"+actionname+".FBX", typeof(AnimationClip)) as AnimationClip;
		AnimatorState d = sm.AddState(actionname);
		d.motion = newClip;
		return d;
	}

}