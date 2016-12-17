using UnityEngine;
using System.Collections;

public class HealOnTouch : MonoBehaviour
{
    public float healing_amount = 33f;
    public float movement_speed = 1f;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")
            || coll.gameObject.layer == LayerMask.NameToLayer("CaptureTether"))
        {
            GameState.game_state.Heal_All_Players(healing_amount);
            SoundMixer.sound_manager.PlayCollectSound();
            EffectsManager.effects.BulletReflected(this.transform.position);
            EffectsManager.effects.PlayersHealed();
            Destroy(this.gameObject);
        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("DestructiveTether"))
        {
            EffectsManager.effects.ViolentExplosion(this.transform.position);
            SoundMixer.sound_manager.PlayGettingHitExplosion();
            Destroy(this.gameObject);
        }
    }


    void Update()
    {
        rigid.velocity = new Vector2(-movement_speed, 0);
    }
}
