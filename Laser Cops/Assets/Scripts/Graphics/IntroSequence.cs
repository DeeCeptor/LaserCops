using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour 
{
    Camera cam;
    Vector3 starting_pos;

    void Start () 
	{
        starting_pos = this.transform.position;
        cam = this.GetComponent<Camera>();

        StartCoroutine(IntroCamera());
	}
	
    IEnumerator IntroCamera()
    {
        yield return new WaitForSeconds(0.1f);

        // Focus on player 1
        //this.transform.parent = GameState.game_state.PlayerObjects[0].transform;
        this.transform.position = GameState.game_state.PlayerObjects[0].transform.position;
        cam.orthographicSize = 2f;
        Debug.Log("A");

        yield return new WaitForSeconds(1);

        //this.transform.parent = GameState.game_state.PlayerObjects[1].transform;
        this.transform.position = GameState.game_state.PlayerObjects[1].transform.position;
        Debug.Log("b");

        yield return new WaitForSeconds(1);

        cam.orthographicSize = 4f;
        this.transform.parent = null;
        this.transform.position = starting_pos;
        this.GetComponent<CameraManager>().enabled = true;

        yield return new WaitForSeconds(2);

        Destroy(this);
    }

    bool lerp_to_starting_point = false;

    void Update () 
	{
	    if (lerp_to_starting_point && this.transform.position != starting_pos)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, starting_pos, 0.1f);
        }
	}
}
