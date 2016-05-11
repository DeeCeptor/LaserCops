using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Loads the specified scene. This should be the last component you want, as all conversations will be lost after this.
public class LoadSceneNode : Node 
{
	public string level_to_load;

	public override void Run_Node()
	{
		// Simply loads the specified scene
		Debug.Log("Switching level: " + level_to_load);
		UnityEngine.SceneManagement.SceneManager.LoadScene(level_to_load);
	}
	
	
	public override void Button_Pressed()
	{
	
	}
	
	
	public override void Finish_Node()
	{
		StopAllCoroutines();
		
		base.Finish_Node();
	}
}
