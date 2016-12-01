using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;

// Create a menu item that causes a new controller and statemachine to be created.

public class SM : Editor {
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
		//string actionname;
		//AnimatorState stateA1;
		//AnimationClip newClip;

		// Creates the controller
		rolename = "sword";
		var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath ("Assets/Mecanim/"+rolename+".controller");

		// Add parameters
		controller.AddParameter("act", AnimatorControllerParameterType.Int);
		controller.AddParameter("stat", AnimatorControllerParameterType.Int);//0收刀，1拔刀
		// Add StateMachines
		var rootStateMachine = controller.layers[0].stateMachine;

		// Add States
		AnimatorState idle=MyAddState(rootStateMachine,"sword","idle",WrapMode.Loop);//from putweapon
		AnimatorState run=MyAddState(rootStateMachine,"sword","run",WrapMode.Loop);//1
		AnimatorState gun=MyAddState(rootStateMachine,"sword","gun",WrapMode.Once);//3
		AnimatorState atkidle=MyAddState(rootStateMachine,"sword","atkidle",WrapMode.Loop);//from takeweapon
		AnimatorState hardhit=MyAddState(rootStateMachine,"sword","hardhit",WrapMode.Once);//5
		AnimatorState jump=MyAddState(rootStateMachine,"sword","jump",WrapMode.Once);//2
		AnimatorState lighthit=MyAddState(rootStateMachine,"sword","lighthit",WrapMode.Once);//4
		AnimatorState takeweapon=MyAddState(rootStateMachine,"sword","takeweapon",WrapMode.Once);//7
		AnimatorState putweapon=MyAddState(rootStateMachine,"sword","putweapon",WrapMode.Once);//6

		// Add Events
		AnimationEvent[] ae=new AnimationEvent[1];
		ae[0]=new AnimationEvent(); 
		ae[0].time = 0f;
		ae[0].functionName = "Idle";//返回idle状态
		AnimationUtility.SetAnimationEvents (lighthit.motion as AnimationClip,ae);
		AnimationUtility.SetAnimationEvents (hardhit.motion as AnimationClip,ae);
		AnimationUtility.SetAnimationEvents (gun.motion as AnimationClip,ae);
		//AnimationUtility.SetAnimationEvents (jump.motion as AnimationClip,ae);
		AnimationUtility.SetAnimationEvents (takeweapon.motion as AnimationClip,ae);
		AnimationUtility.SetAnimationEvents (putweapon.motion as AnimationClip,ae);

		// Add Transitions

		//idle to others
		var transition_idle2run=idle.AddTransition(run);
		transition_idle2run.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "act");
		transition_idle2run.hasExitTime = false;
		AnimatorStateTransition transition;
		transition = idle.AddTransition (takeweapon);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 7, "act");
		transition.hasExitTime = false;

		//run to others
		transition = run.AddTransition (idle);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "act");
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "stat");
		transition.hasExitTime = false;
		transition = run.AddTransition (atkidle);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "act");
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "stat");
		transition.hasExitTime = false;
		transition = run.AddTransition (jump);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 2, "act");
		transition.hasExitTime = false;
		transition = run.AddTransition (gun);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 3, "act");
		transition.hasExitTime = false;

		//atkidle to others
		transition = atkidle.AddTransition (run);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "act");
		transition.hasExitTime = false;
		transition = atkidle.AddTransition (putweapon);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 6, "act");
		transition.hasExitTime = false;
		transition = atkidle.AddTransition (lighthit);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 4, "act");
		transition.hasExitTime = false;
		transition = atkidle.AddTransition (hardhit);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 5, "act");
		transition.hasExitTime = false;

		//others to others
		transition = takeweapon.AddTransition (atkidle);
		transition.hasExitTime = true;
		transition = putweapon.AddTransition (idle);
		transition.hasExitTime = true;
		transition = gun.AddTransition (idle);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "stat");
		transition.hasExitTime = true;
		transition = jump.AddTransition (idle);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "stat");
		transition.hasExitTime = true;
		transition = gun.AddTransition (atkidle);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "stat");
		transition.hasExitTime = true;
		transition = jump.AddTransition (atkidle);
		transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "stat");
		transition.hasExitTime = true;
		transition = lighthit.AddTransition (atkidle);
		//transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "stat");
		transition.hasExitTime = true;
		transition = hardhit.AddTransition (atkidle);
		//transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 1, "stat");
		transition.hasExitTime = true;
	}
	static AnimatorState MyAddState(AnimatorStateMachine sm,string rolename,string actionname,WrapMode wm)
	{
		AnimationClip newClip = AssetDatabase.LoadAssetAtPath("Assets/Resources/主角/"+rolename+"@"+actionname+".FBX", typeof(AnimationClip)) as AnimationClip;
		newClip.wrapMode = wm;//是否重复播放
		AnimatorState d = sm.AddState(actionname);
		d.motion = newClip;
		return d;
	}

}