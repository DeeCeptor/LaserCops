using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour
{
    SpriteRenderer sprite;

	void Start ()
    {
        sprite = this.GetComponent<SpriteRenderer>();
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera"))
        {
            GameState.game_state.Victory();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
            if (collider.gameObject.tag.Equals("MainCamera"))
            {
            GameState.game_state.Victory();
        }
    }

    void Update ()
    {
	}
}
