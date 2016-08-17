using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour
{
    bool activated = false;
    SpriteRenderer sprite;

	void Start ()
    {
        sprite = this.GetComponent<SpriteRenderer>();
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera") && !activated)
        {
            activated = true;
            GameState.game_state.Victory();
            //Destroy(this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera") && !activated)
        {
            activated = true;
            GameState.game_state.Victory();
        }
    }

    void Update ()
    {
	}
}
