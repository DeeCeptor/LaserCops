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
        if ((collider.gameObject.tag.Equals("Player")|| collider.gameObject.tag.Equals("Player")) && !activated)
        {
            Crossed_Finish_Line(collider.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.gameObject.tag.Equals("Player") || collider.gameObject.tag.Equals("Player")) && !activated)
        {
            Crossed_Finish_Line(collider.gameObject);
        }
    }


    public void Crossed_Finish_Line (GameObject victorious_player)
    {
        // Spawn party effects for the player!
        EffectsManager.effects.Fireworks(victorious_player.transform.position, "Red").transform.parent = victorious_player.transform;
        EffectsManager.effects.Fireworks(victorious_player.transform.position, "Green").transform.parent = victorious_player.transform;
        EffectsManager.effects.Fireworks(victorious_player.transform.position, "Blue").transform.parent = victorious_player.transform;
        
        // Cut up the finish line
        EffectsManager.effects.CutSprite(this.GetComponentInChildren<SpriteRenderer>().gameObject);
        Destroy(this.GetComponentInChildren<SpriteRenderer>());

        activated = true;
        GameState.game_state.Victory();
    }


    void Update ()
    {
	}
}
