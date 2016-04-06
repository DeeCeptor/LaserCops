using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour
{
    SpriteRenderer sprite;

	void Start ()
    {
        sprite = this.GetComponent<SpriteRenderer>();
	}


    void Update ()
    {
        if (sprite.IsVisibleFrom(Camera.main))
        {
            GameState.game_state.Victory();

            Destroy(this.gameObject);
        }
	}
}
