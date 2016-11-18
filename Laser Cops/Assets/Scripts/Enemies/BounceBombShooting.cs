using UnityEngine;
using System.Collections;

public class BounceBombShooting : TrackShotScrolling
 {
    void FixedUpdate()
    {
        if (active)
        {
            faceTarget(playerToTrack.position);
            if (shotCounter < Time.time)
            {
                shotCounter = Time.time + shotDelay;

                if (playerToTrack != null)
                {
                    shoot();
                    int randInt = Random.Range(0, players.Length);
                    playerToTrack = players[randInt].transform;
                }

                else
                {
                    players = GameState.game_state.PlayerObjects;
                    if (players.Length > 0)
                    {
                        int randInt = Random.Range(0, players.Length);
                        playerToTrack = players[randInt].transform;
                        if (playerToTrack.gameObject.name == "Player 1")
                        {
                            bulletColour = _Colour.Pink;
                        }
                        else
                        {
                            bulletColour = _Colour.Blue;
                        }
                        shoot();
                    }
                }
            }
        }
        else
        {
            checkActive();
        }
    }

    new public void shoot()
    {
        if (!playerCloseDisable)
        {
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, new Quaternion(0, 0, 0, 0));
            bulletSpawned.GetComponent<Rigidbody2D>().velocity = (playerToTrack.position - transform.position).normalized*5;
            if (bulletColour == _Colour.Red)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (bulletColour == _Colour.Pink)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
            if (bulletColour == _Colour.Blue)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            if (bulletColour == _Colour.Yellow)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            SoundMixer.sound_manager.PlayLazerShot();
        }

        else
        {
            bool fire = true;
            for (int i = 0; i < GameState.game_state.Players.Count; i++)
            {
                if ((GameState.game_state.Players[i].transform.position - transform.position).magnitude < disableDistance)
                {
                    fire = false;
                }
            }

            if (fire)
            {
                GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position,new Quaternion(0,0, 0, 0));
                bulletSpawned.GetComponent<Rigidbody2D>().velocity = (playerToTrack.position - transform.position).normalized*5;
                if (bulletColour == _Colour.Red)
                {
                    bulletSpawned.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else if (bulletColour == _Colour.Pink)
                {
                    bulletSpawned.GetComponent<SpriteRenderer>().color = Color.magenta;
                }
                if (bulletColour == _Colour.Blue)
                {
                    bulletSpawned.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (bulletColour == _Colour.Yellow)
                {
                    bulletSpawned.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                SoundMixer.sound_manager.PlayLazerShot();
            }
        }
    }
}
