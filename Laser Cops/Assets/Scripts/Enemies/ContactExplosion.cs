using UnityEngine;
using System.Collections;

public class ContactExplosion : MonoBehaviour 
{
    public float radius = 6f;
    public bool flat_dmg = true;
    public float dmg = 30;
    public float explosion_delay_time = 1.5f;
    public GameObject danger_radius;

    bool activated = false;

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (!activated)
        {
            activated = true;
            StartCoroutine(Countdown(col.gameObject));
        }
    }


    public IEnumerator Countdown(GameObject col)
    {
        // Modify radius by difficulty
        if (GameState.game_state.current_difficulty == GameState.Difficulty.Normal)
        {
            radius += 0.5f;
        }
        else if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
        {
            radius += 1;
        }


        // Show explosion radius
        danger_radius.transform.localScale = new Vector3(radius, radius, 1);
        danger_radius.SetActive(true);

        if (explosion_delay_time != 0 && (col.gameObject.layer != LayerMask.NameToLayer("Obstacles")))
            yield return new WaitForSeconds(explosion_delay_time);

        Explode();
      
        yield return null;
    }


    public void Explode()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.FireballNoGravity(this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);

        for (int x = 0; x < GameState.game_state.Players.Count; x++)
        {
            float distance = Vector2.Distance(this.transform.position, GameState.game_state.Players[x].transform.position);
            if (distance <= radius)
            {
                if (flat_dmg)
                    GameState.game_state.Players[x].TakeHit(dmg);
                else
                {
                    // Not flat dmg, make the damage fall off based on distance
                    float dmg_dealt = (distance / radius) * dmg;
                    GameState.game_state.Players[x].TakeHit(dmg_dealt);
                }
            }
        }
        EffectsManager.effects.CutSprite(this.gameObject);

        Destroy(this.gameObject);
    }
}
