using UnityEngine;
using System.Collections.Generic;
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

    public float easySpeed = 2f;
    public float hardSpeed = 4f;

    public EnemyBossTetherScript tether;
    public GameObject EnemySpawner;

    public GameObject ClydeTurret;
    public GameObject BonnieTurret;

    public float escapeBoostCountdown = 2f;
    public float escapeBoostTimer = 0f;

    public float boosterCooldown = 0.7f;
    public float boosterCounter = 0f;

    public Transform playerToTrack;
    public GameObject[] players;

    public float behaviourChangeRate = 4f;
    public float behaviourChangeCounter = 0f;
    //time to add at the start of the level to make sure they don't jump the player
    public float inactiveTime = 4f;

    public List<ParticleSystem> booster_particles = new List<ParticleSystem>();

    public TetherBossBehaviour currentBehaviour = TetherBossBehaviour.TargetRandomPlayer;

    public BossHealthScript Health;
    public float healthThreshold;
    public float healthBetweenStages = 1000f;
    public int currentStage = 1;
    public ConversationManager formChangeConversation1;
    public ConversationManager formChangeConversation2;
    public ConversationManager formChangeConversation3;
    public ConversationManager formChangeConversation4;
    public ConversationManager formChangeConversation5;

    public TetherBossCar clydeScript;
    public TetherBossCar bonnieScript;

    //only spawn thelma and Louise if there are more than 2 players
    public TetherBossCar thelma;
    public TetherBossCar louise;
    public Vector3 thelmaStartLocation;
    public Vector3 louiseStartLocation;

    // Use this for initialization
    void Start () {
        
        Health = GetComponentInParent<BossHealthScript>();
        healthThreshold = Health.overallHealth - healthBetweenStages;

        behaviourChangeCounter = behaviourChangeRate + Time.time + inactiveTime;

        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
        
        
        if(players.Length >2)
        {
            if(players.Length == 3)
            {

            }
            else if(players.Length == 4)
            {

            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(boosting)
        {
            if (boosterCounter < Time.time)
            {
                foreach (ParticleSystem p in booster_particles)
                {
                    p.Play();
                }
                SoundMixer.sound_manager.PlayCarRev();
                boosterCounter = Time.time + boosterCooldown;
            }
        }

        if (!Health.dying)
        {
            if (behaviourChangeCounter < Time.time)
            {
                behaviourChangeCounter = Time.time + behaviourChangeRate;
                ChangeBehaviours();
            }

            if (currentBehaviour == TetherBossBehaviour.TargetRandomPlayer)
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

            if (Health.overallHealth < healthThreshold)
            {
                ChangeForms();
                healthThreshold = healthThreshold - healthBetweenStages;
            }
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
            currentBehaviour = (TetherBossBehaviour)Random.Range(1,3);
        }
        else if (currentStage == 2)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(0, 4);
        }
        else if (currentStage == 3)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(0, 1);
        }
        else if (currentStage == 4)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(2, 4);
        }
        else if (currentStage == 5)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(0, 1);
        }
        else if (currentStage == 6)
        {
            currentBehaviour = (TetherBossBehaviour)Random.Range(3, 4);
        }
        //the individual cars do not have their targets yet so
        targetAssigned = false;
        //not used for all behaviours but for most of them
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
       
    }

    public void TargetRandomPlayer()
    {
        if (targetAssigned == false)
        {
            Vector3 ClydeTarget = playerToTrack.position + new Vector3(idealDistanceToPlayer, idealDistanceToPlayer, 0);
            Vector3 BonnieTarget = playerToTrack.position - new Vector3(idealDistanceToPlayer, idealDistanceToPlayer, 0);
            clydeScript.travelDirection = ClydeTarget - clydeScript.transform.position;
            bonnieScript.travelDirection = BonnieTarget - bonnieScript.transform.position;
            targetAssigned = true;
        }
    }

    public void Escape()
    {
        if (targetAssigned == false)
        {
            Vector3 ClydeTarget = playerToTrack.position;
            Vector3 BonnieTarget = playerToTrack.position;
            clydeScript.travelDirection = -(ClydeTarget - clydeScript.transform.position);
            bonnieScript.travelDirection = -(BonnieTarget - bonnieScript.transform.position);
            targetAssigned = true;
        }
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

    public void PlayConversation(ConversationManager conversation)
    {

        if (conversation != null)
        {
            conversation.transform.SetParent(null);
            conversation.Start_Conversation();
        }
    }

    public void ChangeForms()
    {
        if (currentStage == 1)
        {
            PlayConversation(formChangeConversation1);
        }
        else if (currentStage == 2)
        {
            tether.DisableTether();
            EnemySpawner.SetActive(true);
            PlayConversation(formChangeConversation2);
        }
        else if (currentStage == 3)
        {
            //reactivate the tether
            
            tether.EnableTether();
            EnemySpawner.SetActive(false);
            PlayConversation(formChangeConversation3);
        }
        else if (currentStage == 4)
        {
            ClydeTurret.SetActive(true);
            BonnieTurret.SetActive(true);
            PlayConversation(formChangeConversation4);
        }
        else if (currentStage == 5)
        {
            ClydeTurret.SetActive(false);
            BonnieTurret.SetActive(false);
            PlayConversation(formChangeConversation5);
        }

        currentStage = currentStage + 1;
    }
}
