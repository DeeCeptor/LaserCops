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
            StartCoroutine(Countdown());
    }


    public IEnumerator Countdown()
    {
        // Show explosion radius
        danger_radius.transform.localScale = new Vector3(radius, radius, 1);
        danger_radius.SetActive(true);


        if (explosion_delay_time != 0)
            yield return new WaitForSeconds(explosion_delay_time);

        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.FireballNoGravity(this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);

        for(int x = 0; x < GameState.game_state.Players.Count; x++)
        {
            float distance = Vector2.Distance(this.transform.position, GameState.game_state.Players[x].transform.position);
            if (distance <= radius)
            {
                Debug.Log("Contact explosion hurt: " + GameState.game_state.Players[x].player_number);

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
        Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)(radius * Vector2.left), Color.red, 5f);

        EffectsManager.effects.CutSprite(this.gameObject);

        Destroy(this.gameObject);

        yield return null;
    }


    public void Explode()
    {

    }
}
