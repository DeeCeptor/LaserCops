using UnityEngine;
using System.Collections;

public class ContactExplosion : MonoBehaviour 
{
    public float radius = 4f;
    public float dmg = 30;

    public void OnCollisionEnter2D(Collision2D col)
    {
        Explode();
    }


    public void Explode()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.FireballNoGravity(this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);

        for(int x = 0; x < GameState.game_state.Players.Count; x++)
        {
            if (Vector2.Distance(this.transform.position, GameState.game_state.Players[x].transform.position) <= radius)
            {
                Debug.Log("Contact explosion hurt: " + GameState.game_state.Players[x].player_number);
                GameState.game_state.Players[x].TakeHit(dmg);
            }
        }
        Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)(radius * Vector2.left), Color.red, 5f);

        EffectsManager.effects.CutSprite(this.gameObject);

        Destroy(this.gameObject);
    }
}
