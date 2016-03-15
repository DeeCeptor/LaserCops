using UnityEngine;
using System.Collections;

public class reboundableBullet : BulletScript
{
    public float damageToBoss = 2f;
    public Color reboundColor = Color.blue;
    public bool rebounding = false;
    public int reboundLayer = 12;
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Tether")
        {
            gameObject.tag = "ReboundingBullet";
            rebounding = true;
            dir =  (Vector2) transform.position - (Vector2)collision.contacts[0].point;
            gameObject.layer = reboundLayer;
            GetComponent<SpriteRenderer>().color = reboundColor;
        }

        else if(collision.gameObject.tag == "Enemy" && rebounding)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        else if(collision.gameObject.tag == "Boss" && rebounding)
        {
            //Todo when making boss
        }


    }
}
