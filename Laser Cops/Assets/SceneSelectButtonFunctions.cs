using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSelectButtonFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SelectScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
