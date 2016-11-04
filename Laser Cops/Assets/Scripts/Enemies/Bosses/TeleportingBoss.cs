using UnityEngine;
using System;
using System.Collections;

//this is the script for a boss which will
//-need to be hit by the tether to kill
//-will teleport randomly around the screen
//-shoot a burst of bullets each time that the boss teleports
//-should not shoot all the time in order to give the player a chance to attack

public class TeleportingBoss : MonoBehaviour
{
    //How often the boss can teleport
    public float teleportTimer = 7f;
    public float teleportCounter = 0;

    //time between the teleport animation starting and the actual gameobject moving
    public float teleportDuration = 1f;
    public float teleportDurationCounter = float.PositiveInfinity;

    //teleport animation gameObject
    public GameObject teleportAnim;

    //How long the turrets stay active and firing for after teleporting
    public float firingDuration = 2f;
    public float firingCounter = 0;
    public bool turretsActive = false;
    public bool teleporting = false;

    //Random alteration to teleport timer in order to make it unpredictable
    public float randomRangeToTeleportTimer = 2f;

    public float xLeftOfScreen;
    public float xRightOfScreen;
    public float yBottomOfScreen;
    public float yTopOfScreen;
    public float sizeX;
    public float sizeY;
    public float yToTeleportTo;
    public float xToTeleportTo;

    public float UIHeight = 2f;
    public float BottomToHighway = 1f;

    public GameObject animDest;
    public GameObject animStart;

    //the minimum distance from the player when selecting a teleport destination
    public float minDistanceFromPlayer = 6f;

    void Start()
    {
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        yTopOfScreen = maxScreenBounds.y - UIHeight;
        yBottomOfScreen = minScreenBounds.y + BottomToHighway;
        xRightOfScreen = maxScreenBounds.x;
        xLeftOfScreen = minScreenBounds.x;

        teleportCounter = teleportTimer + Time.time + UnityEngine.Random.Range(0,randomRangeToTeleportTimer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 12 && !teleporting)
        {
            teleportCounter = teleportTimer + Time.time + UnityEngine.Random.Range(0, randomRangeToTeleportTimer);
            yToTeleportTo = UnityEngine.Random.Range(yBottomOfScreen + sizeY, yTopOfScreen - sizeY);
            xToTeleportTo = UnityEngine.Random.Range(xLeftOfScreen + sizeX, xRightOfScreen - sizeX);
            teleportDurationCounter = teleportDuration + Time.time;
            animDest = (GameObject)Instantiate(teleportAnim, new Vector3(xToTeleportTo, yToTeleportTo, 0), transform.rotation);
            animStart = (GameObject)Instantiate(teleportAnim, transform.position, transform.rotation);
            teleporting = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(teleportCounter < Time.time)
        {
            teleportCounter = teleportTimer + Time.time + UnityEngine.Random.Range(0, randomRangeToTeleportTimer);

            //set to true when the teleport destination is far enough from players
            bool goodSpot = false;

            //failsafe to avoid infinite loop, max iterations before moving on incase the restrictions on teleporting are too strict
            int i = 0;

            while (!goodSpot && i < 500)
            {
                goodSpot = true;
                yToTeleportTo = UnityEngine.Random.Range(yBottomOfScreen + sizeY, yTopOfScreen - sizeY);
                xToTeleportTo = UnityEngine.Random.Range(xLeftOfScreen + sizeX, xRightOfScreen - sizeX);
                Vector3 teleportSpot = new Vector3(xToTeleportTo, yToTeleportTo, 0);
                for(int x = 0;x < GameState.game_state.number_of_players;x++)
                {
                    if((GameState.game_state.Players[x].transform.position - teleportSpot).magnitude < minDistanceFromPlayer)
                    {
                        goodSpot = false;
                    }
                }
                i++;
            }
            teleportDurationCounter = teleportDuration + Time.time;
            animDest = (GameObject)Instantiate(teleportAnim,new Vector3(xToTeleportTo,yToTeleportTo,0),transform.rotation);
            animStart = (GameObject)Instantiate(teleportAnim, transform.position, transform.rotation);
            teleporting = true;
        }
        
        if(teleportDurationCounter < Time.time)
        {
            teleportDurationCounter = float.PositiveInfinity;
            transform.position = new Vector3(xToTeleportTo, yToTeleportTo, 0);
            ActivateTurrets();
            firingCounter = firingDuration + Time.time;
            turretsActive = true;
            Destroy(animDest);
            Destroy(animStart);
            teleporting = false;
        }

        if(firingCounter < Time.time && turretsActive == true)
        {
            turretsActive = false;
            DeActivateTurrets();
        }

    }

    //call shoot scripts on all turrets place on the boss
    public void ActivateTurrets()
    {
        OnlyActivateOnCallTurret[] turrets = transform.GetComponentsInChildren<OnlyActivateOnCallTurret>();
        for(int i = 0; i < turrets.Length;i++)
        {
            turrets[i].active = true;
        }
    }

    //set all of the turrets on the boss to be disabled
    public void DeActivateTurrets ()
    {
        OnlyActivateOnCallTurret[] turrets = transform.GetComponentsInChildren<OnlyActivateOnCallTurret>();
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].active = false;
        }
    }
}
