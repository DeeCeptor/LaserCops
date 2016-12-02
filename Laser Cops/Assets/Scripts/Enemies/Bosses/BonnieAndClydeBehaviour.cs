using UnityEngine;
using System.Collections;

public enum TetherBossBehaviour
{
    TargetRandomPlayer = 0, Escape = 1, Circle = 2, Charge = 3
};
//note that this script should be attached to the parent of the bonny and clyde bosses since they need to move as a team
public class BonnieAndClydeBehaviour : MonoBehaviour {

    //distance the cars will try to keep from the player they are targeting 
    public float idealDistanceToPlayer = 3f;
    //some behaviours will do nothing after a target has been assigned
    public bool targetAssigned = false;

    public Transform playerToTrack;
    public GameObject[] players;

    public float behaviourChangeRate = 4f;
    public float behaviourChangeCounter = 0f;

    public TetherBossBehaviour currentBehaviour = TetherBossBehaviour.TargetRandomPlayer;

    public BossHealthScript Health;
    public float secondStageHealthThreshold = 5000f;
    public float thirdStageHealthThreshold = 4000f;
    public float fourthStageHealthThreshold = 3000f;
    public float fifthStageHealthThreshold = 2000f;
    public float sixthStageHealthThreshold = 1000f;
    public float currentStage = 1f;

    public TetherBossCar clydeScript;
    public TetherBossCar bonnieScript;
    // Use this for initialization
    void Start () {
        Health = GetComponentInParent<BossHealthScript>();
        behaviourChangeCounter = behaviourChangeRate + Time.time;

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
        if(currentStage == 1)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(0,2);
        }
        else if (currentStage == 2)
        {

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
        Vector3 ClydeTarget = playerToTrack.position + new Vector3(idealDistanceToPlayer, idealDistanceToPlayer, 0);
        Vector3 BonnieTarget = playerToTrack.position - new Vector3(idealDistanceToPlayer, idealDistanceToPlayer, 0);
        clydeScript.travelDirection = -(ClydeTarget - clydeScript.transform.position);
        bonnieScript.travelDirection = -(BonnieTarget - bonnieScript.transform.position);
    }

    public void Circle()
    {

    }

    public void Charge()
    {

    }
}
