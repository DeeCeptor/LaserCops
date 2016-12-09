using UnityEngine;
using System.Collections;

public enum TetherBossBehaviour
{
    TargetRandomPlayer = 1, Escape = 0, Circle = 2, Charge = 3
};
//note that this script should be attached to the parent of the bonny and clyde bosses since they need to move as a team
public class BonnieAndClydeBehaviour : MonoBehaviour {

    //distance the cars will try to keep from the player they are targeting 
    public float idealDistanceToPlayer = 3f;
    //some behaviours will do nothing after a target has been assigned
    public bool targetAssigned = false;
    public bool boosting = false;

    public Transform playerToTrack;
    public GameObject[] players;

    public float behaviourChangeRate = 4f;
    public float behaviourChangeCounter = 0f;
    //time to add at the start of the level to make sure they don't jump the player
    public float inactiveTime = 4f;

    public TetherBossBehaviour currentBehaviour = TetherBossBehaviour.TargetRandomPlayer;

    public BossHealthScript Health;
    public float secondStageHealthThreshold = 5000f;
    public float thirdStageHealthThreshold = 4000f;
    public float fourthStageHealthThreshold = 3000f;
    public float fifthStageHealthThreshold = 2000f;
    public float sixthStageHealthThreshold = 1000f;
    public int currentStage = 1;

    public TetherBossCar clydeScript;
    public TetherBossCar bonnieScript;
    // Use this for initialization
    void Start () {
        Health = GetComponentInParent<BossHealthScript>();
        behaviourChangeCounter = behaviourChangeRate + Time.time + inactiveTime;

        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(behaviourChangeCounter < Time.time)
        {
            behaviourChangeCounter = Time.time + behaviourChangeRate;
            ChangeBehaviours();
        }

        if(currentBehaviour == TetherBossBehaviour.TargetRandomPlayer)
        {
            TargetRandomPlayer();
        }
        else if (currentBehaviour == TetherBossBehaviour.Escape)
        {
            Escape();
        }
        else if (currentBehaviour == TetherBossBehaviour.Circle)
        {
            Circle();
        }
        else if (currentBehaviour == TetherBossBehaviour.Charge)
        {
            Charge();
        }
    }

    //the available list of behaviours is different with each stage of the boss
    public void ChangeBehaviours()
    {
        boosting = false;
        clydeScript.boosting = false;
        bonnieScript.boosting = false;
        if(currentStage == 1)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(0,4);
        }
        else if (currentStage == 2)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(1, 4);
        }
        else if (currentStage == 3)
        {

        }
        else if (currentStage == 4)
        {

        }
        else if (currentStage == 5)
        {

        }
        else if (currentStage == 6)
        {

        }
        //the individual cars do not have their targets yet so
        targetAssigned = false;
        //not used for all behaviours but for most of them
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
       
    }

    public void TargetRandomPlayer()
    {
            Vector3 ClydeTarget = playerToTrack.position + new Vector3(idealDistanceToPlayer,idealDistanceToPlayer,0);
            Vector3 BonnieTarget = playerToTrack.position - new Vector3(idealDistanceToPlayer, idealDistanceToPlayer,0);
            clydeScript.travelDirection = ClydeTarget - clydeScript.transform.position;
            bonnieScript.travelDirection = BonnieTarget - bonnieScript.transform.position;
    }

    public void Escape()
    {
        Vector3 ClydeTarget = playerToTrack.position;
        Vector3 BonnieTarget = playerToTrack.position;
        clydeScript.travelDirection = -(ClydeTarget - clydeScript.transform.position);
        bonnieScript.travelDirection = -(BonnieTarget - bonnieScript.transform.position);
    }

    public void Circle()
    {
        Vector3 direction = playerToTrack.position;
        clydeScript.travelDirection = (Quaternion.Euler(0,0, -45) * ((direction - clydeScript.transform.position).normalized));
        bonnieScript.travelDirection = (Quaternion.Euler(0,0, 45) * ((direction- bonnieScript.transform.position).normalized));
    }

    public void Charge()
    {
        if (targetAssigned == false)
        {
            boosting = true;
            clydeScript.boosting = true;
            bonnieScript.boosting = true;
            Vector3 ClydeTarget = playerToTrack.position + new Vector3(idealDistanceToPlayer, idealDistanceToPlayer, 0);
            Vector3 BonnieTarget = playerToTrack.position - new Vector3(idealDistanceToPlayer, idealDistanceToPlayer, 0);
            clydeScript.travelDirection = ClydeTarget - clydeScript.transform.position;
            bonnieScript.travelDirection = BonnieTarget - bonnieScript.transform.position;
            behaviourChangeCounter = behaviourChangeCounter - 1;
            targetAssigned = true;
        }

    }
}
