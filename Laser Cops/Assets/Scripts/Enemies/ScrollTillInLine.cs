using UnityEngine;
using System.Collections;

//this class scrolls down the side of the sceen then charges at the player when it's in line with them
public class ScrollTillInLine : scrollThenChase {
    public float chargeSpeed = 4f;
    public bool charging = false;
    //direction to charge gets set when charging is set to true defaults to up
    private direction chargeDirection = direction.up;

	// Use this for initialization
	void Start () {
        players = GameState.game_state.PlayerObjects;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!active)
        {
            CheckActive();
            moveInactive();
        }
        else
        {
            CheckDeath();
            if (!charging)
            {
                checkCharge();
                moveActive();
            }
            else
            {
                Charge();
            }
        }
    }
    
    //check if it's time to charge the player
    public void checkCharge()
    {
        if(travelDirection == direction.left)
        {
            for(int i = 0; i < players.Length;i++)
            {
                if(players[i].transform.position.x >= transform.position.x)
                {
                    charging = true;
                    if(players[i].transform.position.y >= transform.position.y)
                    {
                        chargeDirection = direction.up;
                    }
                    else if (players[i].transform.position.y < transform.position.y)
                    {
                        chargeDirection = direction.down;
                    }
                }
            }
        }

        else if(travelDirection == direction.right)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].transform.position.x <= transform.position.x)
                {
                    charging = true;
                    if (players[i].transform.position.y >= transform.position.y)
                    {
                        chargeDirection = direction.up;
                    }
                    else if (players[i].transform.position.y < transform.position.y)
                    {
                        chargeDirection = direction.down;
                    }
                }
            }
        }

        else if (travelDirection == direction.up)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].transform.position.y <= transform.position.y)
                {
                    charging = true;
                    if (players[i].transform.position.x >= transform.position.x)
                    {
                        chargeDirection = direction.right;
                    }
                    else if (players[i].transform.position.x < transform.position.x)
                    {
                        chargeDirection = direction.left;
                    }
                }
            }
        }

        else if (travelDirection == direction.down)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].transform.position.y >= transform.position.y)
                {
                    charging = true;
                    if (players[i].transform.position.x >= transform.position.x)
                    {
                        chargeDirection = direction.right;
                    }
                    else if (players[i].transform.position.x < transform.position.x)
                    {
                        chargeDirection = direction.left;
                    }
                }
            }
        }
    }

    public void Charge()
    {
        if (chargeDirection == direction.left)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-chargeSpeed, 0);
        }
        else if (chargeDirection == direction.up)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, chargeSpeed);
        }
        else if (chargeDirection == direction.right)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(chargeSpeed, 0);
        }
        else if (chargeDirection == direction.down)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -chargeSpeed);
        }
    }
}
