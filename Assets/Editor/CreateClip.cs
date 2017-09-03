using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;


public class CreateClip {

	// Use this for initialization
	[MenuItem("GameObject/Mecanim/CreateClip")]
	static void DoCreateClip () 
	{
		GameObject gameObject = Selection.activeGameObject;

		Animator animator = gameObject.GetComponent<Animator>();
		if (animator == null)
			return;

		UnityEditor.Animations.AnimatorController controller = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
		if (controller == null)
			return;
		
		// Go forward with presenting user a save clip dialog
		string message = string.Format("Create a new animation for the game object '{0}':", gameObject.name);
		string newClipPath = EditorUtility.SaveFilePanelInProject ("Create New Animation", "New Animation", "anim", message);
		
		// If user canceled or save path is invalid, we can't create a clip
		if (newClipPath == "") 
			return;
		
		// At this point we know that we can create a clip
		AnimationClip newClip = new AnimationClip ();
		AssetDatabase.CreateAsset(newClip, newClipPath);

		controller.AddMotion(newClip);
	}
}
